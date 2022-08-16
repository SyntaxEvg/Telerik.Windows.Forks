using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class TimeConverter
	{
		static TimeConverter()
		{
			Dictionary<Tuple<TimeUnit, TimeUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<TimeUnit, TimeUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Year, TimeUnit.Year), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Year, TimeUnit.Day), (double v) => v * 365.25);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Year, TimeUnit.Hour), (double v) => v * 8766.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Year, TimeUnit.Minute), (double v) => v * 525960.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Year, TimeUnit.Second), (double v) => v * 31557600.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Day, TimeUnit.Year), (double v) => v * 0.0027378507871321);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Day, TimeUnit.Day), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Day, TimeUnit.Hour), (double v) => v * 24.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Day, TimeUnit.Minute), (double v) => v * 1440.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Day, TimeUnit.Second), (double v) => v * 86400.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Hour, TimeUnit.Year), (double v) => v * 0.000114077116130504);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Hour, TimeUnit.Day), (double v) => v * 0.0416666666666667);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Hour, TimeUnit.Hour), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Hour, TimeUnit.Minute), (double v) => v * 60.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Hour, TimeUnit.Second), (double v) => v * 3600.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Minute, TimeUnit.Year), (double v) => v * 1.90128526884174E-06);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Minute, TimeUnit.Day), (double v) => v * 0.000694444444444444);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Minute, TimeUnit.Hour), (double v) => v * 0.0166666666666667);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Minute, TimeUnit.Minute), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Minute, TimeUnit.Second), (double v) => v * 60.0);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Second, TimeUnit.Year), (double v) => v * 3.16880878140289E-08);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Second, TimeUnit.Day), (double v) => v * 1.15740740740741E-05);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Second, TimeUnit.Hour), (double v) => v * 0.000277777777777778);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Second, TimeUnit.Minute), (double v) => v * 0.0166666666666667);
			dictionary.Add(Tuple.Create<TimeUnit, TimeUnit>(TimeUnit.Second, TimeUnit.Second), (double v) => v * 1.0);
			TimeConverter.Lookup = dictionary;
		}

		public static double Convert(double number, TimeUnit from_unit, TimeUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return TimeConverter.Lookup[Tuple.Create<TimeUnit, TimeUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<TimeUnit, TimeUnit>, Func<double, double>> Lookup;
	}
}
