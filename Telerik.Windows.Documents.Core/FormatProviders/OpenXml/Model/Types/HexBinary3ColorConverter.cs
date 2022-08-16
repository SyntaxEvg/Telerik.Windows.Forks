using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class HexBinary3ColorConverter : IStringConverter<Color>
	{
		public Color ConvertFromString(string hexString)
		{
			return ColorsHelper.HexStringToColor(hexString);
		}

		public string ConvertToString(Color color)
		{
			if (color.A == 255)
			{
				return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
			}
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { color.A, color.R, color.G, color.B });
		}
	}
}
