# The Basics

Working with Amounts is easy and should be self explanatory:

    var myWeight = new Amount(70, MassUnits.KiloGram);
    var myWeightOnTheMoon = myWeight * 0.65;
    Console.WriteLine(myWeightOnTheMoon); // 45.5 Kg
    Console.WriteLine(myWeightOnTheMoon == new Amount(45.5, MassUnits.KiloGram)); // True

Amounts can be used as would be expected:

## Comparison

    var weightOfCar = new Amount(2000, MassUnits.KiloGram);
    var weightOfTruck = new Amount(10000, MassUnits.KiloGram);
    Console.WriteLine(weightOfCar < weightOfTruck); // true

All operators are supported: `<`, `<=`, `==`, `>=`, `>`, `!=`

## Maths

```
var length1 = new Amount(3, LengthUnits.Meter);
var length2 = new Amount(7, LengthUnits.Meter);
Console.WriteLine(length1 + length2); // 10 m
```

All operators are supported: `/`, `*`, `+`, `-`

## There's more!

 - There are some interesting [helper methods](./helper-methods.md). 
 - Read more on using [composite units](./composite-units.md) 
 - You can [register your own units](./registering-your-own-units.md).
