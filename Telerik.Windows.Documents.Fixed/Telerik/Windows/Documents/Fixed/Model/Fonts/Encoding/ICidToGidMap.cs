using System;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding
{
	interface ICidToGidMap
	{
		ushort GetGlyphId(int charCode);
	}
}
