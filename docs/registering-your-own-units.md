# Registering Your Own Units

RedStar.Amounts contains many [standard units](https://github.com/petermorlion/RedStar.Amounts/tree/master/RedStar.Amounts.StandardUnits), but it is entirely possible to register your own units.

## Units

Start by defining your units:

```
[UnitDefinitionClass]
public static class CustomUnits
{
    public static readonly Unit XP = new Unit("Experience Point", "XP", new UnitType("gaming currency"));
    public static readonly Unit Diamond = new Unit("Diamond", "D", 0.1 * XP);
}
```

What happens here is we define two units, and give them a name and an symbol. The third parameter in the constructor is either a unit type or a base unit. The `UnitType` is used to check if different `Unit` instances can be converted to each other. And the base unit (i.e. `0.1 * XP`) is used to define relative conversions. So in the above example, 10 Diamonds would equal 1 XP.

The `UnitDefinitionClassAttribute` is used to identify a class that contains Unit fields. They **must** be `public static` fields when using this attribute. 

## Conversions

It is also possible to register custom conversion functions. You do not need to register these functions for units in the same unit type. This is usually the case when one unit is a certain fraction of another unit, e.g. meters and millimeters. But with, for example, temperatures, this isn't quite as straight forward:

1 °C = (1 * 9 / 5 + 32) F (Celsius to Fahrenheit)
1 °C = (1 + 273.15) K (Celsius to Kelvin)

To define such custom conversions, use the `UnitManager.RegisterConversion` method:

```
[UnitConversionClass]
public static class CustomConversions
{
    public static void RegisterConversions()
    {
        // Register conversion functions:
        // Convert Celcius to Fahrenheit:
        UnitManager.RegisterConversion(DegreeCelcius, DegreeFahrenheit, delegate(Amount amount)
        {
            return new Amount(amount.Value * 9.0 / 5.0 + 32.0, DegreeFahrenheit);
        });

        // etc.
    }
}
```

The method that registers the conversions **must** be a `public static void` method. Please note that all `public static void` methods in the `UnitConversionClass` class will be invoked.

## Registering it all

You can then register all units and conversions by calling:

```
UnitManager.RegisterByAssembly(myUnitAssembly)
```

Where `myUnitAssembly` is the assembly containing your unit definition classes and unit conversion classes. If you want, you can combine unit definition and unit conversion classes of course.

The `RegisterByAssembly` method just makes a call to `RegisterUnits(assembly)` and `RegisterConversions(assembly)` methods, which you can call separately if you want. These in turn call the `RegisterUnits(type)` and `RegisterConversions(type)` methods, which  you can also call separately. And finally, you can call the `RegisterUnit(unit)` and `RegisterConversion(fromUnit, toUnit, conversionFunction)` methods too.

So if the above example with the attributes and the `RegisterByAssembly` call doesn't suit your needs, you can call the methods in any way that fits you best.

## Checking registrations

If you want to know if a unit has already been registered, you can do so easily by calling

```
UnitManager.IsRegistered(unit)
```

Units are compared by checking the factor and the unit type:

```
UnitManager.IsRegistered(new Unit("foo", "f", new UnitType("bar")); //false
UnitManager.RegisterUnit(new Unit("foo", "f", new UnitType("bar")); 
UnitManager.IsRegistered(new Unit("foo", "f", new UnitType("bar")); //true
UnitManager.IsRegistered(new Unit("millifoo", "mf", 0.001 * Foo); //false (where Foo is a reference to our first unit)
```
