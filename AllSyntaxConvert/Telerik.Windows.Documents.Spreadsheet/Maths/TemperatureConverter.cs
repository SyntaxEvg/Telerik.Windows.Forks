using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	sealed class TemperatureConverter
	{
		static TemperatureConverter()
		{
			Dictionary<Tuple<TemperatureUnit, TemperatureUnit>, Func<double, double>> dictionary = new Dictionary<Tuple<TemperatureUnit, TemperatureUnit>, Func<double, double>>();
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Celsius, TemperatureUnit.Celsius), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit), (double v) => v * 9.0 / 5.0 + 32.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Celsius, TemperatureUnit.Kelvin), (double v) => v + 273.15);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Celsius, TemperatureUnit.Rankine), (double v) => (v + 273.15) * 9.0 / 5.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Celsius, TemperatureUnit.Reaumur), (double v) => v * 4.0 / 5.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius), (double v) => (v - 32.0) * 5.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Fahrenheit, TemperatureUnit.Fahrenheit), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin), (double v) => (v + 459.67) * 5.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Fahrenheit, TemperatureUnit.Rankine), (double v) => v + 459.67);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Fahrenheit, TemperatureUnit.Reaumur), (double v) => (v - 32.0) * 4.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Kelvin, TemperatureUnit.Celsius), (double v) => v - 273.15);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit), (double v) => v * 9.0 / 5.0 - 459.67);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Kelvin, TemperatureUnit.Kelvin), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Kelvin, TemperatureUnit.Rankine), (double v) => v * 9.0 / 5.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Kelvin, TemperatureUnit.Reaumur), (double v) => (v - 273.15) * 4.0 / 5.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Rankine, TemperatureUnit.Celsius), (double v) => (v - 491.67) * 5.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Rankine, TemperatureUnit.Fahrenheit), (double v) => v - 459.67);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Rankine, TemperatureUnit.Kelvin), (double v) => v * 5.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Rankine, TemperatureUnit.Rankine), (double v) => v * 1.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Rankine, TemperatureUnit.Reaumur), (double v) => (v - 491.67) * 4.0 / 9.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Reaumur, TemperatureUnit.Celsius), (double v) => v * 5.0 / 4.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Reaumur, TemperatureUnit.Fahrenheit), (double v) => v * 9.0 / 4.0 + 32.0);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Reaumur, TemperatureUnit.Kelvin), (double v) => v * 5.0 / 9.0 + 273.15);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Reaumur, TemperatureUnit.Rankine), (double v) => v * 9.0 / 4.0 + 491.67);
			dictionary.Add(Tuple.Create<TemperatureUnit, TemperatureUnit>(TemperatureUnit.Reaumur, TemperatureUnit.Reaumur), (double v) => v * 1.0);
			TemperatureConverter.Lookup = dictionary;
		}

		public static double Convert(double number, TemperatureUnit from_unit, TemperatureUnit to_unit)
		{
			if (from_unit != to_unit)
			{
				return TemperatureConverter.Lookup[Tuple.Create<TemperatureUnit, TemperatureUnit>(from_unit, to_unit)](number);
			}
			return number;
		}

		static readonly Dictionary<Tuple<TemperatureUnit, TemperatureUnit>, Func<double, double>> Lookup;
	}
}
