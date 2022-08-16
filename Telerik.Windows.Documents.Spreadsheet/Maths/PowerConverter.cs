using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class PowerConverter
	{
		static PowerConverter()
		{
			Dictionary<Tuple<PowerUnit, PowerUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<PowerUnit, PowerUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.HorsePower, PowerUnit.HorsePower), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.HorsePower, PowerUnit.PferdeStarke), (double v) => v * 1.013869665424);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.HorsePower, PowerUnit.Watt), (double v) => v * 745.69987158227);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.PferdeStarke, PowerUnit.HorsePower), (double v) => v * 0.986320070619531);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.PferdeStarke, PowerUnit.PferdeStarke), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.PferdeStarke, PowerUnit.Watt), (double v) => v * 735.49875);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.Watt, PowerUnit.HorsePower), (double v) => v * 0.00134102208959503);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.Watt, PowerUnit.PferdeStarke), (double v) => v * 0.0013596216173039);
			dictionary.Add(Tuple.Create<PowerUnit, PowerUnit>(PowerUnit.Watt, PowerUnit.Watt), (double v) => v * 1.0);
			PowerConverter.Lookup = dictionary;
		}

		public static double Convert(double number, PowerUnit from_unit, PowerUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return PowerConverter.Lookup[Tuple.Create<PowerUnit, PowerUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<PowerUnit, PowerUnit>, Func<double, double>> Lookup;
	}
}
