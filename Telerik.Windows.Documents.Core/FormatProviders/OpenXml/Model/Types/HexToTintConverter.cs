using System;
using System.Globalization;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class HexToTintConverter : IStringConverter<double>
	{
		public double ConvertFromString(string str)
		{
			byte b = byte.Parse(str.Substring(0, 2), NumberStyles.HexNumber);
			double value = 1.0 - (double)b / 255.0;
			return Math.Round(value, 2);
		}

		public string ConvertToString(double val)
		{
			byte b = (byte)Math.Round((1.0 - val) * 255.0);
			return string.Format("{0:X2}", b);
		}
	}
}
