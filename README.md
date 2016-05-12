# RedStar.Amounts

This library contains classes to easily work with unit and amounts.

Too often, applications use integers, doubles, etc. to indicate distances,
weights, and other measured values. This can cause problems because assumptions
are made on the units, conversions have to be done, etc.

This can cause hard-to-trace bugs and leads to less readable code.

Say you have a public service (WCF, REST, ...) that takes in measurements from
an external service. Which of the following do you prefer?

    public class SomeService
    {
        public void ReportMeasurement(double length, double height)
        {
            // external system sends values in meters, but we work with cm
            var lengthInCm = length * 100;
            var heightInCm = height * 100;
            var area = lengthInCm * heightInCm;
            ...
        }
    }

Or this:

    public class SomeService
    {
        public void ReportMeasurement(Amount length, Amount height)
        {
            var area = length * height;
            ...
        }
    }

In the second example, the `area` variable, will automatically have a value and
a unit. The unit will be m².

This makes it easier and more fail-proof to do calculations later.

## Source, license

This code was taken and built upon from [CodeProject](http://www.codeproject.com/Articles/611731/Working-with-Units-and-Amounts).
Credit is due to Rudi Breedenraedt. The code is licensed under the CPOL.
