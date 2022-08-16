using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class PostFormat2 : Post
	{
		public PostFormat2(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		void CreateGlyphNamesMapping(ushort[] glyphNameIndex, string[] names)
		{
			this.glyphNames = new Dictionary<string, ushort>(glyphNameIndex.Length);
			for (int i = 0; i < glyphNameIndex.Length; i++)
			{
				ushort num = glyphNameIndex[i];
				if ((int)num < Post.macintoshStandardOrderNames.Length)
				{
					this.glyphNames[Post.macintoshStandardOrderNames[(int)num]] = (ushort)i;
				}
				else
				{
					num = (ushort)((int)num - Post.macintoshStandardOrderNames.Length);
					this.glyphNames[names[(int)num]] = (ushort)i;
				}
			}
		}

		public override ushort GetGlyphId(string name)
		{
			ushort result;
			if (this.glyphNames.TryGetValue(name, out result))
			{
				return result;
			}
			return 0;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			base.Read(reader);
			ushort num = reader.ReadUShort();
			ushort[] array = new ushort[(int)num];
			int num2 = Post.macintoshStandardOrderGlyphIds.Count - 1;
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = reader.ReadUShort();
				if ((int)array[i] > num2)
				{
					num2 = (int)array[i];
				}
			}
			int num3 = num2 - Post.macintoshStandardOrderGlyphIds.Count + 1;
			string[] array2 = new string[num3];
			for (int j = 0; j < num3; j++)
			{
				array2[j] = reader.ReadString();
			}
			this.CreateGlyphNamesMapping(array, array2);
		}

		Dictionary<string, ushort> glyphNames;
	}
}
