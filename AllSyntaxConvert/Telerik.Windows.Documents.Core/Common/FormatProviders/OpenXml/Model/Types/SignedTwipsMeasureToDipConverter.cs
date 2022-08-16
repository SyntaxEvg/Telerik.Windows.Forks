using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Common.FormatProviders.OpenXml.Model.Types
{
	class SignedTwipsMeasureToDipConverter : IStringConverter<double>
	{
		public double ConvertFromString(string value)
		{
			if (value != null && value.Length > 1)
			{
				string unit = value.Substring(value.Length - 2);
				if (UniversalMeasureToDipConverter.IsValidUnit(unit))
				{
					return Converters.UniversalMeasureToDipConverter.ConvertFromString(value);
				}
			}
			return Converters.TwipToDipConverter.ConvertFromString(value);
		}

		public string ConvertToString(double value)
		{
			return Converters.TwipToDipConverter.ConvertToString(value);
		}
	}
}
