using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	abstract class CMapTable
	{
		internal static CMapTable ReadCMapTable(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			ushort num2 = num;
			CMapTable cmapTable;
			if (num2 != 0)
			{
				switch (num2)
				{
				case 4:
					cmapTable = new CMapFormat4Table();
					goto IL_3F;
				case 6:
					cmapTable = new CMapFormat6Table();
					goto IL_3F;
				}
				return null;
			}
			cmapTable = new CMapFormat0Table();
			IL_3F:
			cmapTable.Read(reader);
			return cmapTable;
		}

		internal static CMapTable ImportCMapTable(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			ushort num2 = num;
			CMapTable cmapTable;
			if (num2 != 0)
			{
				if (num2 != 4)
				{
					return null;
				}
				cmapTable = new CMapFormat4Table();
			}
			else
			{
				cmapTable = new CMapFormat0Table();
			}
			cmapTable.Import(reader);
			return cmapTable;
		}

		public abstract ushort FirstCode { get; }

		public abstract bool TryGetGlyphId(int unicode, out ushort glyphId);

		public abstract bool TryGetCharId(ushort glyphId, out ushort charCode);

		public abstract void Read(OpenTypeFontReader reader);

		public abstract void Write(FontWriter writer);

		public abstract void Import(OpenTypeFontReader reader);
	}
}
