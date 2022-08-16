using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Data
{
	interface IBuildCharHolder
	{
		byte[] GetSubr(int index);

		byte[] GetGlobalSubr(int index);

		GlyphData GetGlyphData(string glyphName);
	}
}
