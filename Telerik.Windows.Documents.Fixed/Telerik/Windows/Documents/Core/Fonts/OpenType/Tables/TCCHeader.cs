using System;
using System.IO;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class TCCHeader
	{
		static OpenTypeFontSource ReadTrueTypeFontFile(OpenTypeFontReader reader, uint offset)
		{
			reader.BeginReadingBlock();
			reader.Seek((long)((ulong)offset), SeekOrigin.Begin);
			OpenTypeFontSource result = new OpenTypeFontSource(reader);
			reader.EndReadingBlock();
			return result;
		}

		public TCCHeader(TrueTypeCollection collection)
		{
			this.collection = collection;
		}

		protected OpenTypeFontReader Reader
		{
			get
			{
				return this.collection.Reader;
			}
		}

		public OpenTypeFontSourceBase[] Fonts
		{
			get
			{
				if (this.fonts == null)
				{
					this.fonts = new OpenTypeFontSourceBase[this.offsetTable.Length];
					for (int i = 0; i < this.offsetTable.Length; i++)
					{
						this.fonts[i] = TCCHeader.ReadTrueTypeFontFile(this.Reader, this.offsetTable[i]);
					}
				}
				return this.fonts;
			}
		}

		public void Read(OpenTypeFontReader reader)
		{
			reader.ReadULong();
			reader.ReadFixed();
			uint num = reader.ReadULong();
			this.offsetTable = new uint[num];
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				this.offsetTable[num2] = reader.ReadULong();
				num2++;
			}
		}

		readonly TrueTypeCollection collection;

		uint[] offsetTable;

		OpenTypeFontSourceBase[] fonts;
	}
}
