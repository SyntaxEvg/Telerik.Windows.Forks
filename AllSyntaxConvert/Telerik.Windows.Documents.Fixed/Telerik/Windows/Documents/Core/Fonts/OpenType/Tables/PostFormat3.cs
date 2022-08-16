using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class PostFormat3 : Post
	{
		public PostFormat3(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public override ushort GetGlyphId(string name)
		{
			return 0;
		}
	}
}
