using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class UnsignedIntHexConverter : IStringConverter<UnsignedIntHex>
	{
		public UnsignedIntHex ConvertFromString(string value)
		{
			SpreadColor color = ColorsHelper.HexStringToColor(value);
			return new UnsignedIntHex(color);
		}

		public string ConvertToString(UnsignedIntHex value)
		{
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
			{
				value.Color.A,
				value.Color.R,
				value.Color.G,
				value.Color.B
			});
		}
	}
}
