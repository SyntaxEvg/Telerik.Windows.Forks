using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.Data
{
	interface IEncoding
	{
		string GetGlyphName(ICFFFontFile file, ushort index);
	}
}
