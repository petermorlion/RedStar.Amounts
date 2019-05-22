# Helper Methods

There are some useful helper methods, that should be self-explanatory:

## AmountMath

```
var max = AmountMath.Max(new Amount(1, LengthUnits.Meter), new Amount(2, LengthUnits.Meter));
Console.WriteLine(max); // 2 m

var min = AmountMath.Min(new Amount(1, LengthUnits.Meter), new Amount(2, LengthUnits.Meter));
Console.WriteLine(min); // 1 m

var rounded = AmountMath.Round(new Amount(1.3455, LengthUnits.Meter), 2);
Console.WriteLine(rounded); // 1.35 m

var abs = AmountMath.Abs(new Amount(-3, LengthUnits.Meter));
Console.WriteLine(abs); // 3 m
```

## Extension methods

```
var amounts = new[] { new Amount(1, LengthUnits.Meter), new Amount(2, LengthUnits.Meter) };
Console.WriteLine(amounts.Sum()); // 3 m
Console.WriteLine(amounts.Average()); // 1.5 m

var amount = new Amount(11, LengthUnits.Meter);
var limited = amount.Limit(new Amount(1, LengthUnits.Meter), new Amount(10, LengthUnits.Meter));
Console.WriteLine(limited); // 10 m
```
