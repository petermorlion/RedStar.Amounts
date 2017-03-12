namespace RedStar.Amounts.StandardUnits
{
    [UnitDefinitionClass]
    public static class EnergyUnits
    {
        public static readonly Unit Joule = new Unit("joule", "J", LengthUnits.Meter.Power(2) * MassUnits.KiloGram * TimeUnits.Second.Power(-2));
        public static readonly Unit KiloJoule = new Unit("kilojoule", "kJ", 1000.0 * Joule);
        public static readonly Unit MegaJoule = new Unit("megajoule", "MJ", 1000000.0 * Joule);
        public static readonly Unit GigaJoule = new Unit("gigajoule", "GJ", 1000000000.0 * Joule);

        public static readonly Unit Watt = new Unit("watt", "W", Joule / TimeUnits.Second);
        public static readonly Unit KiloWatt = new Unit("kilowatt", "kW", 1000.0 * Watt);
        public static readonly Unit MegaWatt = new Unit("megawatt", "MW", 1000000.0 * Watt);

        public static readonly Unit WattSecond = new Unit("watt-second", "Wsec", Watt * TimeUnits.Second);
        public static readonly Unit WattHour = new Unit("watt-hour", "Wh", Watt * TimeUnits.Hour);
        public static readonly Unit KiloWattHour = new Unit("kilowatt-hour", "kWh", 1000.0 * WattHour);

        public static readonly Unit Calorie = new Unit("calorie", "cal", 4.1868 * Joule);
        public static readonly Unit KiloCalorie = new Unit("kilocalorie", "kcal", 1000.0 * Calorie);

        public static readonly Unit HorsePower = new Unit("horsepower", "hp", 0.73549875 * KiloWatt);
    }
}