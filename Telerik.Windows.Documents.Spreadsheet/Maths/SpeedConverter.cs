using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class SpeedConverter
	{
		static SpeedConverter()
		{
			Dictionary<Tuple<SpeedUnit, SpeedUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<SpeedUnit, SpeedUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.AdmiralityKnot, SpeedUnit.AdmiralityKnot), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.AdmiralityKnot, SpeedUnit.Knot), (double v) => v * 1.00063930885529);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.AdmiralityKnot, SpeedUnit.MetersPerHour), (double v) => v * 1853.184);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.AdmiralityKnot, SpeedUnit.MetersPerSecond), (double v) => v * 0.514773333333333);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.AdmiralityKnot, SpeedUnit.MilesPerHour), (double v) => v * 1.15151515151515);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.Knot, SpeedUnit.AdmiralityKnot), (double v) => v * 0.999361099599392);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.Knot, SpeedUnit.Knot), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.Knot, SpeedUnit.MetersPerHour), (double v) => v * 1852.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.Knot, SpeedUnit.MetersPerSecond), (double v) => v * 0.514444444444444);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.Knot, SpeedUnit.MilesPerHour), (double v) => v * 1.15077944802354);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerHour, SpeedUnit.AdmiralityKnot), (double v) => v * 0.000539611824837685);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerHour, SpeedUnit.Knot), (double v) => v * 0.000539956803455723);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerHour, SpeedUnit.MetersPerHour), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerHour, SpeedUnit.MetersPerSecond), (double v) => v * 0.000277777777777778);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerHour, SpeedUnit.MilesPerHour), (double v) => v * 0.000621371192237334);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerSecond, SpeedUnit.AdmiralityKnot), (double v) => v * 1.94260256941567);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerSecond, SpeedUnit.Knot), (double v) => v * 1.9438444924406);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerSecond, SpeedUnit.MetersPerHour), (double v) => v * 3600.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerSecond, SpeedUnit.MetersPerSecond), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MetersPerSecond, SpeedUnit.MilesPerHour), (double v) => v * 2.2369362920544);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MilesPerHour, SpeedUnit.AdmiralityKnot), (double v) => v * 0.868421052631579);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MilesPerHour, SpeedUnit.Knot), (double v) => v * 0.868976241900648);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MilesPerHour, SpeedUnit.MetersPerHour), (double v) => v * 1609.344);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MilesPerHour, SpeedUnit.MetersPerSecond), (double v) => v * 0.44704);
			dictionary.Add(Tuple.Create<SpeedUnit, SpeedUnit>(SpeedUnit.MilesPerHour, SpeedUnit.MilesPerHour), (double v) => v * 1.0);
			SpeedConverter.Lookup = dictionary;
		}

		public static double Convert(double number, SpeedUnit from_unit, SpeedUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return SpeedConverter.Lookup[Tuple.Create<SpeedUnit, SpeedUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<SpeedUnit, SpeedUnit>, Func<double, double>> Lookup;
	}
}
