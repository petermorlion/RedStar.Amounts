# JSON.NET

There is a separate package that include support for [JSON.NET](http://www.newtonsoft.com/json) by providing custom converters. You can install is by running:

    Install-Package RedStar.Amounts.JsonNet

In this package, there are two custom JSON.NET converters: the [`ObjectAmountJsonConverter`](https://github.com/petermorlion/RedStar.Amounts/blob/master/RedStar.Amounts.JsonNet/ObjectAmountJsonConverter.cs), and the [`StringAmountJsonConverter`](https://github.com/petermorlion/RedStar.Amounts/blob/master/RedStar.Amounts.JsonNet/StringAmountJsonConverter.cs).

## ObjectAmountJsonConverter

This converter will convert an Amount to and from a JSON object with two properties: value and unit.

So this JSON:

```
{
    value: 3.4,
    unit: m
}
```

will convert to `new Amount(3.4, LengthUnits.Meter)`, and vice-versa. Make sure you have [registered the units](Getting-started) though.

## StringAmountJsonConverter

This converter will convert an Amount to and from a regular string. So `3.4 m` will convert to `new Amount(3.4, LengthUnits.Meter)`, and vice-versa. Once again, don't forget to [register the units](Getting-started).

## Usages

The `StringAmountJsonConverter` can be useful if you are serializing a large amount of Amounts. This can mean a significant optimization because the string representation takes considerably less characters than the object notation.

If the size of your JSON string isn't an issue, the `ObjectAmountJsonConverter` is the more interesting choice, because an application with no knowledge of RedStar.Amounts can read the value as a double without needing to parse it.
