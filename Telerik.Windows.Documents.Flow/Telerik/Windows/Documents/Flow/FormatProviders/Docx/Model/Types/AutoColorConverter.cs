using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	class AutoColorConverter : IStringConverter<AutoColor>
	{
		public AutoColor ConvertFromString(string hexString)
		{
			if (hexString == "auto")
			{
				return new AutoColor(true);
			}
			return new AutoColor(Converters.HexBinary3Converter.ConvertFromString(hexString), false);
		}

		public string ConvertToString(AutoColor autoColor)
		{
			if (autoColor.IsAutomatic)
			{
				return "auto";
			}
			return Converters.HexBinary3Converter.ConvertToString(autoColor.Color);
		}
	}
}
