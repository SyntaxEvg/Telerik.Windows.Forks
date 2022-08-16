using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class UniversalMeasureToDipConverter : IStringConverter<double>
	{
		static UniversalMeasureToDipConverter()
		{
			UniversalMeasureToDipConverter.RegisterConvertMethod("cm", new Func<double, double>(Unit.CmToDip));
			UniversalMeasureToDipConverter.RegisterConvertMethod("mm", new Func<double, double>(Unit.MmToDip));
			UniversalMeasureToDipConverter.RegisterConvertMethod("in", new Func<double, double>(Unit.InchToDip));
			UniversalMeasureToDipConverter.RegisterConvertMethod("pt", new Func<double, double>(Unit.PointToDip));
			UniversalMeasureToDipConverter.RegisterConvertMethod("pc", new Func<double, double>(Unit.PicaToDip));
			UniversalMeasureToDipConverter.RegisterConvertMethod("pi", new Func<double, double>(Unit.PicaToDip));
		}

		public static bool IsValidUnit(string unit)
		{
			return UniversalMeasureToDipConverter.unitToConvertMethodMapping.ContainsKey(unit);
		}

		public double ConvertFromString(string value)
		{
			if (value != null && value.Length > 1)
			{
				string unit = value.Substring(value.Length - 2);
				double units;
				if (double.TryParse(value.Substring(0, value.Length - 2), NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out units))
				{
					return UniversalMeasureToDipConverter.Convert(unit, units);
				}
			}
			return 0.0;
		}

		public string ConvertToString(double units)
		{
			throw new NotSupportedException();
		}

		static void RegisterConvertMethod(string unit, Func<double, double> convertMethod)
		{
			UniversalMeasureToDipConverter.unitToConvertMethodMapping[unit] = convertMethod;
		}

		static double Convert(string unit, double units)
		{
			return UniversalMeasureToDipConverter.unitToConvertMethodMapping[unit](units);
		}

		static readonly Dictionary<string, Func<double, double>> unitToConvertMethodMapping = new Dictionary<string, Func<double, double>>();
	}
}
