using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Converter
	{
		static Converter()
		{
			Converter.MultiplierLookup = new Dictionary<Tuple<MultiplierPrefix, string>, double>
			{
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Atto, "a"),
					1E-18
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Centi, "c"),
					0.01
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Deci, "d"),
					0.1
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Dekao, "da"),
					10.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Dekao, "e"),
					10.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Exa, "E"),
					1E+18
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Femto, "f"),
					1E-15
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Giga, "G"),
					1000000000.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Hecto, "h"),
					100.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Kilo, "k"),
					1000.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Mega, "M"),
					1000000.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Micro, "u"),
					1E-06
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Milli, "m"),
					0.001
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Nano, "n"),
					1E-09
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Peta, "P"),
					1000000000000000.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Pico, "p"),
					1E-12
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Tera, "T"),
					1000000000000.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Yocto, "y"),
					1E-24
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Yotta, "Y"),
					1E+24
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Zepto, "z"),
					1E-21
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Zetta, "Z"),
					1E+21
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Kibi, "ki"),
					1024.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Mebi, "Mi"),
					1048576.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Gibi, "Gi"),
					1073741824.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Tebi, "Ti"),
					1099511627776.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Pebi, "Pi"),
					1125899906842624.0
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Exbi, "Ei"),
					1.152921504606847E+18
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Zebi, "Zi"),
					1.18059E+21
				},
				{
					Tuple.Create<MultiplierPrefix, string>(MultiplierPrefix.Yobi, "Yi"),
					1.20893E+24
				}
			};
		}

		public static double Convert(double number, string from_unit, string to_unit)
		{
			Tuple<MultiplierPrefix, object> unit = Converter.GetUnit(from_unit);
			Tuple<MultiplierPrefix, object> unit2 = Converter.GetUnit(to_unit);
			if (unit.Item2.GetType().Name != unit2.Item2.GetType().Name)
			{
				throw new Exception("The supplied types do not belong the same measurement category.");
			}
			double num = Converter.MultiplierPrefixValue(unit.Item1);
			object item = unit.Item2;
			double num2 = Converter.MultiplierPrefixValue(unit2.Item1);
			object item2 = unit2.Item2;
			if (item is AreaUnit)
			{
				return AreaConverter.Convert(number * num, (AreaUnit)item, (AreaUnit)item2) / num2;
			}
			if (item is DistanceUnit)
			{
				return DistanceConverter.Convert(number * num, (DistanceUnit)item, (DistanceUnit)item2) / num2;
			}
			if (item is EnergyUnit)
			{
				return EnergyConverter.Convert(number * num, (EnergyUnit)item, (EnergyUnit)item2) / num2;
			}
			if (item is ForceUnit)
			{
				return ForceConverter.Convert(number * num, (ForceUnit)item, (ForceUnit)item2) / num2;
			}
			if (item is InformationUnit)
			{
				return InformationConverter.Convert(number * num, (InformationUnit)item, (InformationUnit)item2) / num2;
			}
			if (item is MagnetismUnit)
			{
				return MagnetismConverter.Convert(number * num, (MagnetismUnit)item, (MagnetismUnit)item2) / num2;
			}
			if (item is MassUnit)
			{
				return MassConverter.Convert(number * num, (MassUnit)item, (MassUnit)item2) / num2;
			}
			if (item is PowerUnit)
			{
				return PowerConverter.Convert(number * num, (PowerUnit)item, (PowerUnit)item2) / num2;
			}
			if (item is PressureUnit)
			{
				return PressureConverter.Convert(number * num, (PressureUnit)item, (PressureUnit)item2) / num2;
			}
			if (item is SpeedUnit)
			{
				return SpeedConverter.Convert(number * num, (SpeedUnit)item, (SpeedUnit)item2) / num2;
			}
			if (item is TemperatureUnit)
			{
				return TemperatureConverter.Convert(number * num, (TemperatureUnit)item, (TemperatureUnit)item2) / num2;
			}
			if (item is TimeUnit)
			{
				return TimeConverter.Convert(number * num, (TimeUnit)item, (TimeUnit)item2) / num2;
			}
			if (item is VolumeUnit)
			{
				return VolumeConverter.Convert(number * num, (VolumeUnit)item, (VolumeUnit)item2) / num2;
			}
			throw new Exception(string.Format("Type '{0}' could not be casted to a known converter.", item.GetType().Name));
		}

		static Tuple<MultiplierPrefix, object> GetUnit(string unit)
		{
			MultiplierPrefix? multiplierPrefix = null;
			object item;
			if (Converter.AllUnits.ContainsKey(unit))
			{
				item = Converter.AllUnits[unit];
			}
			else
			{
				string text = Converter.AllUnits.Keys.FirstOrDefault((string u) => unit.EndsWith(u, false, CultureInfo.InvariantCulture));
				if (text == null)
				{
					throw new Exception(string.Format("Unit '{0}' is not supported.", unit));
				}
				item = Converter.AllUnits[text];
				string mult = unit.Substring(0, unit.Length - text.Length);
				Tuple<MultiplierPrefix, string> tuple = Converter.MultiplierLookup.Keys.FirstOrDefault((Tuple<MultiplierPrefix, string> t) => t.Item2 == mult);
				if (tuple == null)
				{
					throw new Exception(string.Format("Unit '{0}' is not supported.", unit));
				}
				multiplierPrefix = new MultiplierPrefix?(tuple.Item1);
			}
			return Tuple.Create<MultiplierPrefix, object>((multiplierPrefix != null) ? multiplierPrefix.Value : MultiplierPrefix.None, item);
		}

		public static double MultiplierPrefixValue(MultiplierPrefix prefix)
		{
			if (prefix == MultiplierPrefix.None)
			{
				return 1.0;
			}
			return Converter.MultiplierLookup.First((KeyValuePair<Tuple<MultiplierPrefix, string>, double> v) => v.Key.Item1 == prefix).Value;
		}

		public static string MultiplierPrefixAbbreviation(MultiplierPrefix prefix)
		{
			return Converter.MultiplierLookup.First((KeyValuePair<Tuple<MultiplierPrefix, string>, double> v) => v.Key.Item1 == prefix).Key.Item2;
		}

		public static MultiplierPrefix MultiplierPrefixOfAbbreviation(string abbreviation)
		{
			if (string.IsNullOrEmpty(abbreviation))
			{
				throw new ArgumentNullException("abbreviation");
			}
			if (!Converter.MultiplierLookup.Any((KeyValuePair<Tuple<MultiplierPrefix, string>, double> v) => v.Key.Item2 == abbreviation))
			{
				throw new Exception(string.Format("The given abbreviation '{0}' is not supported.", abbreviation));
			}
			return Converter.MultiplierLookup.Single((KeyValuePair<Tuple<MultiplierPrefix, string>, double> v) => v.Key.Item2 == abbreviation).Key.Item1;
		}

		static readonly Dictionary<Tuple<MultiplierPrefix, string>, double> MultiplierLookup;

		static Dictionary<string, object> AllUnits = new Dictionary<string, object>
		{
			{
				"u",
				MassUnit.AtomicMassUnit
			},
			{
				"grain",
				MassUnit.Grain
			},
			{
				"g",
				MassUnit.Gram
			},
			{
				"uk_ton",
				MassUnit.ImperialTon
			},
			{
				"uk_cwt",
				MassUnit.ImperialWeight
			},
			{
				"ozm",
				MassUnit.Ounce
			},
			{
				"lbm",
				MassUnit.Pound
			},
			{
				"cwt",
				MassUnit.ShortWeight
			},
			{
				"sg",
				MassUnit.Slug
			},
			{
				"stone",
				MassUnit.Stone
			},
			{
				"ton",
				MassUnit.Ton
			},
			{
				"ang",
				DistanceUnit.Angstrom
			},
			{
				"ell",
				DistanceUnit.Ell
			},
			{
				"ft",
				DistanceUnit.Foot
			},
			{
				"in",
				DistanceUnit.Inch
			},
			{
				"ly",
				DistanceUnit.LightYear
			},
			{
				"m",
				DistanceUnit.Meter
			},
			{
				"Nmi",
				DistanceUnit.NauticalMile
			},
			{
				"parsec",
				DistanceUnit.Parsec
			},
			{
				"pica",
				DistanceUnit.Pica6
			},
			{
				"Pica",
				DistanceUnit.Pica72
			},
			{
				"mi",
				DistanceUnit.StatuteMile
			},
			{
				"survey_mi",
				DistanceUnit.SurveyMile
			},
			{
				"yd",
				DistanceUnit.Yard
			},
			{
				"uk_acre",
				AreaUnit.InternationalAcre
			},
			{
				"us_acre",
				AreaUnit.USSurveyAcre
			},
			{
				"ang2",
				AreaUnit.SquareAngstrom
			},
			{
				"ang^2",
				AreaUnit.SquareAngstrom
			},
			{
				"ar",
				AreaUnit.Are
			},
			{
				"ft2",
				AreaUnit.SquareFeet
			},
			{
				"ft^2",
				AreaUnit.SquareFeet
			},
			{
				"ha",
				AreaUnit.Hectare
			},
			{
				"in2",
				AreaUnit.SquareInches
			},
			{
				"in^2",
				AreaUnit.SquareInches
			},
			{
				"ly2",
				AreaUnit.SquareLightYear
			},
			{
				"ly^2",
				AreaUnit.SquareLightYear
			},
			{
				"m2",
				AreaUnit.SquareMeters
			},
			{
				"m^2",
				AreaUnit.SquareMeters
			},
			{
				"Morgen",
				AreaUnit.Morgen
			},
			{
				"mi2",
				AreaUnit.SquareMiles
			},
			{
				"mi^2",
				AreaUnit.SquareMiles
			},
			{
				"Nmi2",
				AreaUnit.SquareNauticalMiles
			},
			{
				"Nmi^2",
				AreaUnit.SquareNauticalMiles
			},
			{
				"Pica2",
				AreaUnit.SquarePica
			},
			{
				"Pica^2",
				AreaUnit.SquarePica
			},
			{
				"Picapt2",
				AreaUnit.SquarePica
			},
			{
				"Picapt^2",
				AreaUnit.SquarePica
			},
			{
				"yd2",
				AreaUnit.SquareYards
			},
			{
				"yd^2",
				AreaUnit.SquareYards
			},
			{
				"J",
				EnergyUnit.Joule
			},
			{
				"e",
				EnergyUnit.Erg
			},
			{
				"c",
				EnergyUnit.ThermodynamicCalorie
			},
			{
				"cal",
				EnergyUnit.ITCalorie
			},
			{
				"eV",
				EnergyUnit.ElectronVolt
			},
			{
				"ev",
				EnergyUnit.ElectronVolt
			},
			{
				"HPh",
				EnergyUnit.HorsePowerPerHour
			},
			{
				"hh",
				EnergyUnit.HorsePowerPerHour
			},
			{
				"Wh",
				EnergyUnit.WattPerHour
			},
			{
				"wh",
				EnergyUnit.WattPerHour
			},
			{
				"flb",
				EnergyUnit.FootPound
			},
			{
				"BTU",
				EnergyUnit.BTU
			},
			{
				"btu",
				EnergyUnit.BTU
			},
			{
				"N",
				ForceUnit.Newton
			},
			{
				"dyn",
				ForceUnit.Dyne
			},
			{
				"dy",
				ForceUnit.Dyne
			},
			{
				"lbf",
				ForceUnit.Pound
			},
			{
				"pond",
				ForceUnit.Pond
			},
			{
				"HP",
				PowerUnit.HorsePower
			},
			{
				"h",
				PowerUnit.HorsePower
			},
			{
				"PS",
				PowerUnit.PferdeStarke
			},
			{
				"W",
				PowerUnit.Watt
			},
			{
				"w",
				PowerUnit.Watt
			},
			{
				"Pa",
				PressureUnit.Pascal
			},
			{
				"p",
				PressureUnit.Pascal
			},
			{
				"atm",
				PressureUnit.Atmosphere
			},
			{
				"at",
				PressureUnit.Atmosphere
			},
			{
				"mmHg",
				PressureUnit.Mercury
			},
			{
				"psi",
				PressureUnit.PSI
			},
			{
				"Torr",
				PressureUnit.Torr
			},
			{
				"admkn",
				SpeedUnit.AdmiralityKnot
			},
			{
				"kn",
				SpeedUnit.Knot
			},
			{
				"m/h",
				SpeedUnit.MetersPerHour
			},
			{
				"m/hr",
				SpeedUnit.MetersPerHour
			},
			{
				"m/s",
				SpeedUnit.MetersPerSecond
			},
			{
				"m/sec",
				SpeedUnit.MetersPerSecond
			},
			{
				"mph",
				SpeedUnit.MilesPerHour
			},
			{
				"C",
				TemperatureUnit.Celsius
			},
			{
				"cel",
				TemperatureUnit.Celsius
			},
			{
				"F",
				TemperatureUnit.Fahrenheit
			},
			{
				"fah",
				TemperatureUnit.Fahrenheit
			},
			{
				"K",
				TemperatureUnit.Kelvin
			},
			{
				"kel",
				TemperatureUnit.Kelvin
			},
			{
				"Rank",
				TemperatureUnit.Rankine
			},
			{
				"Reau",
				TemperatureUnit.Reaumur
			},
			{
				"yr",
				TimeUnit.Year
			},
			{
				"d",
				TimeUnit.Day
			},
			{
				"hr",
				TimeUnit.Hour
			},
			{
				"min",
				TimeUnit.Minute
			},
			{
				"sec",
				TimeUnit.Second
			},
			{
				"tsp",
				VolumeUnit.Teaspoon
			},
			{
				"tspm",
				VolumeUnit.ModernTeaspoon
			},
			{
				"tbs",
				VolumeUnit.TableSpoon
			},
			{
				"oz",
				VolumeUnit.FluidOunce
			},
			{
				"cup",
				VolumeUnit.Cup
			},
			{
				"us_pt",
				VolumeUnit.USPint
			},
			{
				"pt",
				VolumeUnit.USPint
			},
			{
				"uk_pt",
				VolumeUnit.UKPint
			},
			{
				"qt",
				VolumeUnit.Quart
			},
			{
				"uk_qt",
				VolumeUnit.ImperialQuart
			},
			{
				"gal",
				VolumeUnit.Gallon
			},
			{
				"uk_gal",
				VolumeUnit.ImperialGallon
			},
			{
				"lt",
				VolumeUnit.Liter
			},
			{
				"L",
				VolumeUnit.Liter
			},
			{
				"l",
				VolumeUnit.Liter
			},
			{
				"ang3",
				VolumeUnit.CubicAngstrom
			},
			{
				"ang^3",
				VolumeUnit.CubicAngstrom
			},
			{
				"barrel",
				VolumeUnit.USOilBarrel
			},
			{
				"bushel",
				VolumeUnit.USBushel
			},
			{
				"ft3",
				VolumeUnit.CubicFeet
			},
			{
				"ft^3",
				VolumeUnit.CubicFeet
			},
			{
				"in3",
				VolumeUnit.CubicInch
			},
			{
				"in^3",
				VolumeUnit.CubicInch
			},
			{
				"ly3",
				VolumeUnit.CubicLightYear
			},
			{
				"ly^3",
				VolumeUnit.CubicLightYear
			},
			{
				"m3",
				VolumeUnit.CubicMeter
			},
			{
				"m^3",
				VolumeUnit.CubicMeter
			},
			{
				"mi3",
				VolumeUnit.CubicMile
			},
			{
				"mi^3",
				VolumeUnit.CubicMile
			},
			{
				"yd3",
				VolumeUnit.CubicYard
			},
			{
				"yd^3",
				VolumeUnit.CubicYard
			},
			{
				"Nmi3",
				VolumeUnit.CubicNauticalMile
			},
			{
				"Nmi^3",
				VolumeUnit.CubicNauticalMile
			},
			{
				"Pica3",
				VolumeUnit.CubicPica
			},
			{
				"Pica^3",
				VolumeUnit.CubicPica
			},
			{
				"Picapt3",
				VolumeUnit.CubicPica
			},
			{
				"Picapt^3",
				VolumeUnit.CubicPica
			},
			{
				"GRT",
				VolumeUnit.GrossRegisteredTon
			},
			{
				"MTON",
				VolumeUnit.MeasurementTon
			}
		};
	}
}
