# RedStar.Amounts

RedStar.Amounts is a .NET library to help you work more easily and correctly with units
and amounts and units.

## Installing RedStar.Amounts

Getting started is easy. Install via Nuget:

```
Install-Package RedStar.Amounts
```

This will install the package with the basic classes. The most important of those are `Amount` and `Unit`.

In fact, RedStar.Amounts doesn't require anything else, although you probably want to work with the standard units. In that case, install the RedStar.Amounts.StandardUnits package:

```
Install-Package RedStar.Amounts.StandardUnits
```

Then, all you need is one line of code, before you can start using Amounts:

```
UnitManager.RegisterByAssembly(typeof(LengthUnits).Assembly);
```

Usually, you would do this on startup of your application. This registers all units in the RedStar.Amounts.StandardUnits assembly. These will be the units you most likely want to work with. You can find [an overview in the code](https://github.com/petermorlion/RedStar.Amounts/tree/master/RedStar.Amounts.StandardUnits).

After the UnitManager knows all the units to use, you can start working with amounts. 

More docs:
 - [The Basics](./basics.md)
 - There are some interesting [helper methods](./helper-methods.md). 
 - Read more on using [composite units](./composite-units.md) 
 - You can [register your own units](./registering-your-own-units.md).
 - [JSON.NET](./json-net.md)
