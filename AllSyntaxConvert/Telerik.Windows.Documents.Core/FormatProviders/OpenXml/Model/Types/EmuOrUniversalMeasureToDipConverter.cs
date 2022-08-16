using System;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class EmuOrUniversalMeasureToDipConverter : IStringConverter<double>
	{
		public double ConvertFromString(string value)
		{
			string unit = ((value.Length > 2) ? value.Substring(value.Length - 2) : string.Empty);
			if (UniversalMeasureToDipConverter.IsValidUnit(unit))
			{
				return Converters.UniversalMeasureToDipConverter.ConvertFromString(value);
			}
			return Converters.EmuToDipConverter.ConvertFromString(value);
		}

		public string ConvertToString(double dip)
		{
			return Converters.EmuToDipConverter.ConvertToString(dip);
		}
	}
}
