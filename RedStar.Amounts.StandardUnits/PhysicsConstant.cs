using System;

namespace RedStar.Amounts.StandardUnits
{
    // Based on:
    // http://physics.nist.gov/cgi-bin/cuu/Category?view=html&Frequently+used+constants.x=90&Frequently+used+constants.y=21
    public static class PhysicsConstant
    {
        /// <summary>
        /// 1.660 538 921 x 10-27 kg
        /// </summary>
        public static readonly Amount AtomicMass = new Amount(1.660538921 * Math.Pow(10, -27), MassUnits.KiloGram);

        /// <summary>
        /// 6.022 141 29 x 1023 mol-1
        /// </summary>
        public static readonly Amount Avogadro = new Amount(6.02214129 * Math.Pow(10, 23), 1.0 / AmountOfSubstanceUnits.Mole);

        /// <summary>
        /// 1.380 6488 x 10-23 J K-1
        /// </summary>
        public static readonly Amount Boltzmann = new Amount(1.3806488 * Math.Pow(10, -23), EnergyUnits.Joule / TemperatureUnits.Kelvin);

        /// <summary>
        /// 8.854 187 817... x 10-12 F m-1
        /// </summary>
        public static readonly Amount Electric = new Amount(8.854187817 * Math.Pow(10, -12), ElectricUnits.Farad / LengthUnits.Meter);

        /// <summary>
        /// 9.109 382 91 x 10-31 kg
        /// </summary>        
        public static readonly Amount ElectronMass = new Amount(9.10938291 * Math.Pow(10, -31), MassUnits.KiloGram);

        /// <summary>
        /// 1.602 176 565 x 10-19 J
        /// </summary>
        public static readonly Amount ElectronVolt = new Amount(1.602176565 * Math.Pow(10, -19), EnergyUnits.Joule);

        /// <summary>
        /// 1.602 176 565 x 10-19 C
        /// </summary>
        public static readonly Amount ElementaryCharge = new Amount(1.602176565 * Math.Pow(10, -19), ElectricUnits.Coulomb);

        /// <summary>
        /// 96 485.3365 C mol-1
        /// </summary>
        public static readonly Amount Faraday = new Amount(96485.3365, ElectricUnits.Coulomb / AmountOfSubstanceUnits.Mole);

        /// <summary>
        /// 12.566 370 614... x 10-7 N A-2
        /// </summary>
        public static readonly Amount Magnetic = new Amount(12.566370614 * Math.Pow(10, -7), ForceUnits.Newton * ElectricUnits.Ampere.Power(-2));

        /// <summary>
        /// 8.314 4621 J mol-1 K-1
        /// </summary>
        public static readonly Amount MolarGas = new Amount(8.3144621, EnergyUnits.Joule / AmountOfSubstanceUnits.Mole / TemperatureUnits.Kelvin);

        /// <summary>
        /// 6.673 84 x 10-11 m3 kg-1 s-2
        /// </summary>
        public static readonly Amount NewtonianGravitation = new Amount(6.67384 * Math.Pow(10, -11), LengthUnits.Meter.Power(3) / MassUnits.KiloGram / TimeUnits.Second.Power(2));

        /// <summary>
        /// 6.626 069 57 x 10-34 J s
        /// </summary>
        public static readonly Amount Planck = new Amount(6.62606957 * Math.Pow(10, -34), EnergyUnits.Joule * TimeUnits.Second);

        /// <summary>
        /// 1.672 621 777 x 10-27 kg
        /// </summary>
        public static readonly Amount ProtonMass = new Amount(1.672621777 * Math.Pow(10, -27), MassUnits.KiloGram);

        /// <summary>
        /// 10 973 731.568 539 m-1
        /// </summary>
        public static readonly Amount Rydberg = new Amount(10973731.568539, 1 / LengthUnits.Meter);

        /// <summary>
        /// 299 792 458 m s-1
        /// </summary>
        public static readonly Amount SpeedOfLight = new Amount(299792458, LengthUnits.Meter / TimeUnits.Second);

        /// <summary>
        /// 5.670 373 x 10-8 W m-2 K-4
        /// </summary>
        public static readonly Amount StefanBoltzmann = new Amount(5.670373 * Math.Pow(10, -8), EnergyUnits.Watt * LengthUnits.Meter.Power(-2) * TemperatureUnits.Kelvin.Power(-4));
    }
}
