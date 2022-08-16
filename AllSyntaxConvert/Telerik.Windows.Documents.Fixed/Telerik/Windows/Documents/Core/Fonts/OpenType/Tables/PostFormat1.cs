using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class PostFormat1 : Post
	{
		public PostFormat1(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public override ushort GetGlyphId(string name)
		{
			ushort result;
			if (Post.macintoshStandardOrderGlyphIds.TryGetValue(name, out result))
			{
				return result;
			}
			return 0;
		}
	}
}
