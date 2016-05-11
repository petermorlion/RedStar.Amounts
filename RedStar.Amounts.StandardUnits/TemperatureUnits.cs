namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass, UnitConversionClass]
    public static class TemperatureUnits
    {
        public static readonly Unit Kelvin = new Unit("Kelvin", "K", SIUnitTypes.ThermodynamicTemperature);
        public static readonly Unit DegreeCelcius = new Unit("degree celcius", "°C", new UnitType("celcius temperature"));
        public static readonly Unit DegreeFahrenheit = new Unit("degree fahrenheit", "°F", new UnitType("fahrenheit temperature"));

        #region Conversion functions

        public static void RegisterConversions()
        {
            // Register conversion functions:

            // Convert Celcius to Fahrenheit:
            UnitManager.RegisterConversion(DegreeCelcius, DegreeFahrenheit, delegate(Amount amount)
            {
                return new Amount(amount.Value * 9.0 / 5.0 + 32.0, DegreeFahrenheit);
            }
                );

            // Convert Fahrenheit to Celcius:
            UnitManager.RegisterConversion(DegreeFahrenheit, DegreeCelcius, delegate(Amount amount)
            {
                return new Amount((amount.Value - 32.0) / 9.0 * 5.0, DegreeCelcius);
            }
                );

            // Convert Celcius to Kelvin:
            UnitManager.RegisterConversion(DegreeCelcius, Kelvin, delegate(Amount amount)
            {
                return new Amount(amount.Value + 273.15, Kelvin);
            }
                );

            // Convert Kelvin to Celcius:
            UnitManager.RegisterConversion(Kelvin, DegreeCelcius, delegate(Amount amount)
            {
                return new Amount(amount.Value - 273.15, DegreeCelcius);
            }
                );

            // Convert Fahrenheit to Kelvin:
            UnitManager.RegisterConversion(DegreeFahrenheit, Kelvin, delegate(Amount amount)
            {
                return amount.ConvertedTo(DegreeCelcius).ConvertedTo(Kelvin);
            }
                );

            // Convert Kelvin to Fahrenheit:
            UnitManager.RegisterConversion(Kelvin, DegreeFahrenheit, delegate(Amount amount)
            {
                return amount.ConvertedTo(DegreeCelcius).ConvertedTo(DegreeFahrenheit);
            }
                );
        }

        #endregion Conversion functions
    }
}