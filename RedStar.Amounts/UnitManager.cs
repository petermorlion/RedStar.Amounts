﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace RedStar.Amounts
{
    /// <summary>
    /// An event handler called whenever unit names cannot be resolved. Provides a last chance
    /// to resolve units.
    /// </summary>
    public delegate Unit UnitResolveEventHandler(object sender, ResolveEventArgs args);

    /// <summary>
    /// Delegate representiong a unitdirectional unit conversion function.
    /// </summary>
    /// <param name="originalAmount">The amount to be converted.</param>
    /// <returns>The resulting amount.</returns>
    public delegate Amount ConversionFunction(Amount originalAmount);

    /// <summary>
    /// The UnitManager class provides services around unit naming and identification.
    /// </summary>
    /// <remarks>
    /// The UnitManager class contains static methods that access a singleton instance 
    /// of the class.
    /// </remarks>
    public sealed class UnitManager
    {
        #region Fields

        private static UnitManager _instance;

        // Stores for named units:
        private readonly List<Unit> _allUnits = new List<Unit>();
        private readonly Dictionary<UnitType, List<Unit>> _unitsByType = new Dictionary<UnitType, List<Unit>>();
        private readonly Dictionary<string, Unit> _unitsByName = new Dictionary<string, Unit>();
        private readonly Dictionary<string, Unit> _unitsBySymbol = new Dictionary<string, Unit>();

        // Store for conversion functions:
        private readonly Dictionary<UnitConversionKeySlot, UnitConversionValueSlot> _conversions = new Dictionary<UnitConversionKeySlot, UnitConversionValueSlot>();

        #endregion Fields

        #region Public properties

        /// <summary>
        /// The instance of the currently used UnitManager.
        /// </summary>
        public static UnitManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UnitManager();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        #endregion Public properties

        #region Public methods - Registrations

        /// <summary>
        /// Registers both units and conversions based on the assemblies public types marked
        /// with [UnitDefinitionsClass] and [UnitConversionsClass] attributes.
        /// </summary>
        public static void RegisterByAssembly(Assembly assembly)
        {
            RegisterUnits(assembly);
            RegisterConversions(assembly);
        }

        /// <summary>
        /// Register a conversion function.
        /// </summary>
        /// <param name="fromUnit">The unit from which this conversion function allows conversion.</param>
        /// <param name="toUnit">The unit to which this conversion function allows conversion to.</param>
        /// <param name="conversionFunction">The unit conversion function.</param>
        /// <remarks>
        /// A unit conversion function is registered to convert from one unit to another. It will
        /// however be applied to convert from any unit of the same family of the fromUnit, to any
        /// unit family of the toUnit. For reverse conversion, a separate function must be registered.
        /// </remarks>
        public static void RegisterConversion(Unit fromUnit, Unit toUnit, ConversionFunction conversionFunction)
        {
            Instance._conversions[new UnitConversionKeySlot(fromUnit, toUnit)] = new UnitConversionValueSlot(fromUnit, toUnit, conversionFunction);
        }

        /// <summary>
        /// Registers a set of conversion functions by executing all public static void methods of 
        /// the given type. The methods are supposed to call the RegisterConversion method to register
        /// individual conversion functions.
        /// </summary>
        public static void RegisterConversions(Type unitConversionsClass)
        {
            object[] none = new object[0];
            foreach (MethodInfo method in unitConversionsClass.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static))
            {
                if ((method.ReturnType == typeof(void)) && (method.GetParameters().Length == 0))
                {
                    method.Invoke(null, none);
                }
            }
        }

        /// <summary>
        /// Registers a set of conversion function by executing all public static void methods
        /// of public types marked with the [UnitConversionsClass] attribute in the given assembly.
        /// The methods are supposed to call the RegisterConversion method to register individual
        /// conversion functions.
        /// </summary>
        public static void RegisterConversions(Assembly assembly)
        {
            foreach (Type t in assembly.GetExportedTypes())
            {
                if (t.GetCustomAttributes(typeof(UnitConversionClassAttribute), false).Length > 0)
                {
                    RegisterConversions(t);
                }
            }
        }

        /// <summary>
        /// Event raised whenever a unit can not be resolved.
        /// </summary>
        public event UnitResolveEventHandler UnitResolve;

        /// <summary>
        /// Registers a unit.
        /// </summary>
        public static void RegisterUnit(Unit unit)
        {
            // Check precondition: unit <> null
            if (unit == null) throw new ArgumentNullException("unit");

            // Check if unit already registered:
            lock (Instance._allUnits)
            {
                foreach (var u in Instance._allUnits)
                {
                    if (ReferenceEquals(u, unit)) return;
                }

                // Register unit in allUnits:
                Instance._allUnits.Add(unit);

                // Register unit in unitsByType:
                try
                {
                    Instance._unitsByType[unit.UnitType].Add(unit);
                }
                catch (KeyNotFoundException)
                {
                    Instance._unitsByType[unit.UnitType] = new List<Unit>();
                    Instance._unitsByType[unit.UnitType].Add(unit);
                }

                // Register unit by name and symbol:
                Instance._unitsByName[unit.Name] = unit;
                Instance._unitsBySymbol[unit.Symbol] = unit;
            }
        }

        /// <summary>
        /// Register all public static fields of type Unit of the given class.
        /// </summary>
        public static void RegisterUnits(Type unitDefinitionClass)
        {
            foreach (var field in unitDefinitionClass.GetFields(BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(Unit))
                {
                    RegisterUnit((Unit)field.GetValue(null));
                }
            }
        }

        /// <summary>
        /// Registers all public static fields of type Unit of classes
        /// marked with the [UnitDefinitionClass] attribute in the given
        /// assembly.
        /// </summary>
        public static void RegisterUnits(Assembly assembly)
        {
            foreach (var t in assembly.GetExportedTypes())
            {
                if (t.GetCustomAttributes(typeof(UnitDefinitionClassAttribute), false).Length > 0)
                {
                    RegisterUnits(t);
                }
            }
        }

        #endregion Public methods - Registrations

        #region Public methods - Named units

        /// <summary>
        /// Retrieves the unit based on its name.
        /// If the unit is not found, a UnitResolve event is fired as last chance
        /// to resolve the unit.
        /// If the unit cannot be resolved, an UnknownUnitException is raised.
        /// </summary>
        public static Unit GetUnitByName(string name)
        {
            Unit result = null;
            if (!TryGetUnitByName(name, out result))
            {
                throw new UnknownUnitException(string.Format("No unit found named '{0}'.", name));
            }

            return result;
        }

        /// <summary>
        /// Retrieves the unit based on its name.
        /// If the unit is not found, a UnitResolve event is fired as last chance
        /// to resolve the unit.
        /// If the unit cannot be resolved, null is returned.
        /// </summary>
        public static bool TryGetUnitByName(string name, out Unit unit)
        {
            // Try resolve unit by unitsByName:
            Instance._unitsByName.TryGetValue(name, out unit);

            // Try resolve unit by UnitResolve event:
            if (unit != null)
            {
                return true;
            }

            if (Instance.UnitResolve == null)
            {
                return false;
            }

            foreach (UnitResolveEventHandler handler in Instance.UnitResolve.GetInvocationList())
            {
                unit = handler(Instance, new ResolveEventArgs(name));
                if (unit == null)
                {
                    continue;
                }

                RegisterUnit(unit);
                break;
            }

            // Return result:
            return unit != null;
        }

        /// <summary>
        /// Retrieves the unit based on its symbol.
        /// If the unit is not found, an UnknownUnitException is raised.
        /// </summary>
        public static Unit GetUnitBySymbol(string symbol)
        {
            Unit result = null;

            // Try resolve unit by unitsBySymbol:
            Instance._unitsBySymbol.TryGetValue(symbol, out result);

            // Throw exception if unit resolution failed:
            if (result == null)
            {
                throw new UnknownUnitException(string.Format("No unit found with symbol '{0}'.", symbol));
            }

            // Return result:
            return result;
        }

        /// <summary>
        /// Returns the unit types for which one or more units are registered.
        /// </summary>
        public static ICollection<UnitType> GetUnitTypes()
        {
            return Instance._unitsByType.Keys;
        }

        /// <summary>
        /// Returns all registered units.
        /// </summary>
        public static IList<Unit> GetUnits()
        {
            return Instance._allUnits;
        }

        /// <summary>
        /// Whether the given unit is already registered to the UnitManager.
        /// </summary>
        public static bool IsRegistered(Unit unit)
        {
            return (Instance._allUnits.Contains(unit));
        }

        /// <summary>
        /// Returns all registered units of the given type.
        /// </summary>
        public static IList<Unit> GetUnits(UnitType unitType)
        {
            return Instance._unitsByType[unitType];
        }

        /// <summary>
        /// Returns a registered unit that matches the given unit.
        /// </summary>
        /// <param name="unit">The unit for which to find a registered match.</param>
        /// <param name="selfIfNone">
        /// If true, returns the passed unit if no match is found,
        /// otherwise return null if no match is found.
        /// </param>
        /// <remarks>
        /// If the passed unit is named, the passed unit will be returned without
        /// checking if it is registered.
        /// </remarks>
        public static Unit ResolveToNamedUnit(Unit unit, bool selfIfNone)
        {
            if (unit.IsNamed) return unit;
            double factor = unit.Factor;
            if (Instance._unitsByType.ContainsKey(unit.UnitType))
            {
                foreach (Unit m in Instance._unitsByType[unit.UnitType])
                {
                    if (m.Factor == factor) return m;
                }
            }
            return (selfIfNone) ? unit : null;
        }

        #endregion Public methods - Named units

        #region Public methods - Unit conversions

        /// <summary>
        /// Converts the given amount to the given unit.
        /// </summary>
        public static Amount ConvertTo(Amount amount, Unit toUnit)
        {
            try
            {
                // Performance optimalization:
                if (Object.ReferenceEquals(amount.Unit, toUnit))
                {
                    return amount;
                }

                // Perform conversion:
                if (amount.Unit.IsCompatibleTo(toUnit))
                {
                    return new Amount(amount.Value * amount.Unit.Factor / toUnit.Factor, toUnit);
                }
                else
                {
                    UnitConversionKeySlot expectedSlot = new UnitConversionKeySlot(amount.Unit, toUnit);
                    return Instance._conversions[expectedSlot].Convert(amount).ConvertedTo(toUnit);
                }
            }
            catch (KeyNotFoundException)
            {
                throw new UnitConversionException(amount.Unit, toUnit);
            }
        }

        #endregion Public methods - Unit conversions

        #region Private classes to represent slots in conversion dictionary

        /// <summary>
        /// Key slot in the internal conversions dictionary.
        /// </summary>
        private class UnitConversionKeySlot
        {
            private UnitType fromType, toType;

            internal UnitConversionKeySlot(Unit from, Unit to)
            {
                this.fromType = from.UnitType;
                this.toType = to.UnitType;
            }

            public override bool Equals(object obj)
            {
                UnitConversionKeySlot other = obj as UnitConversionKeySlot;
                return (this.fromType == other.fromType) && (this.toType == other.toType);
            }

            public override int GetHashCode()
            {
                return this.fromType.GetHashCode() ^ this.toType.GetHashCode();
            }
        }

        /// <summary>
        /// Value slot in the internal conversions dictionary.
        /// </summary>
        private class UnitConversionValueSlot
        {
            private Unit from, to;
            private ConversionFunction conversionFunction;

            internal UnitConversionValueSlot(Unit from, Unit to, ConversionFunction conversionFunction)
            {
                this.from = from;
                this.to = to;
                this.conversionFunction = conversionFunction;
            }

            internal Amount Convert(Amount amount)
            {
                return this.conversionFunction(amount.ConvertedTo(from));
            }
        }

        #endregion Private classes to represent slots in conversion dictionary
    }
}
