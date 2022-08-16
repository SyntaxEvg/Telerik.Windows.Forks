using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class ForceConverter
	{
		static ForceConverter()
		{
			Dictionary<Tuple<ForceUnit, ForceUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<ForceUnit, ForceUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Newton, ForceUnit.Newton), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Newton, ForceUnit.Dyne), (double v) => v * 100000.0);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Newton, ForceUnit.Pound), (double v) => v * 0.22480894309971);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Newton, ForceUnit.Pond), (double v) => v * 101.971621297793);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Dyne, ForceUnit.Newton), (double v) => v * 1E-05);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Dyne, ForceUnit.Dyne), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Dyne, ForceUnit.Pound), (double v) => v * 2.2480894309971E-06);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Dyne, ForceUnit.Pond), (double v) => v * 0.00101971621297793);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pound, ForceUnit.Newton), (double v) => v * 4.4482216152605);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pound, ForceUnit.Dyne), (double v) => v * 444822.16152605);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pound, ForceUnit.Pound), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pound, ForceUnit.Pond), (double v) => v * 453.59237);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pond, ForceUnit.Newton), (double v) => v * 0.00980665);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pond, ForceUnit.Dyne), (double v) => v * 980.665);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pond, ForceUnit.Pound), (double v) => v * 0.00220462262184878);
			dictionary.Add(Tuple.Create<ForceUnit, ForceUnit>(ForceUnit.Pond, ForceUnit.Pond), (double v) => v * 1.0);
			ForceConverter.Lookup = dictionary;
		}

		public static double Convert(double number, ForceUnit from_unit, ForceUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return ForceConverter.Lookup[Tuple.Create<ForceUnit, ForceUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<ForceUnit, ForceUnit>, Func<double, double>> Lookup;
	}
}
