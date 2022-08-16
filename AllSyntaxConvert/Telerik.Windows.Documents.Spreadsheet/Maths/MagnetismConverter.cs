using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class MagnetismConverter
	{
		static MagnetismConverter()
		{
			Dictionary<Tuple<MagnetismUnit, MagnetismUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<MagnetismUnit, MagnetismUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<MagnetismUnit, MagnetismUnit>(MagnetismUnit.Gauss, MagnetismUnit.Tesla), (double v) => v * 0.0001);
			dictionary.Add(Tuple.Create<MagnetismUnit, MagnetismUnit>(MagnetismUnit.Tesla, MagnetismUnit.Gauss), (double v) => v * 10000.0);
			MagnetismConverter.Lookup = dictionary;
		}

		public static double Convert(double number, MagnetismUnit from_unit, MagnetismUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return MagnetismConverter.Lookup[Tuple.Create<MagnetismUnit, MagnetismUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<MagnetismUnit, MagnetismUnit>, Func<double, double>> Lookup;
	}
}
