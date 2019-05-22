# Composite Units

RedStar.Amounts supports combining units to create new ones. An example of this is speed:

```
var maximumSpeed = new Amount(120, LengthUnits.KiloMeter / TimeUnits.Hour);
```

[SpeedUnits](https://github.com/petermorlion/RedStar.Amounts/blob/master/RedStar.Amounts.StandardUnits/SpeedUnits.cs) are included in RedStar.Amounts however, so you could just do:

```
var maximumSpeed = new Amount(120, SpeedUnits.KilometerPerHour);
```

But not all your scenario's may be covered, so you can combine anything you like:

```
var flow = new Amount(3, VolumeUnits.Meter3 / TimeUnits.Second);
```

Or go totally crazy:

```
var something = new Amount(8, PressureUnits.Bar / VolumeUnits.Meter3 / TimeUnits.Hour);
```

If you're often using the same composite unit that isn't included by default in RedStar.Amounts, consider [registering your own units](Registering-your-own-units).
