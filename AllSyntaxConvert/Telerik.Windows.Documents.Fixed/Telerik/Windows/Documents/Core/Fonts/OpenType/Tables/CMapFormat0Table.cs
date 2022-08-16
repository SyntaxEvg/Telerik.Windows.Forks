using System;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CMapFormat0Table : CMapTable
	{
		public override ushort FirstCode
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			byte b;
			bool result = this.glyphIdToCharId.TryGetToValue((ushort)unicode, out b);
			glyphId = (ushort)b;
			return result;
		}

		public override bool TryGetCharId(ushort glyphId, out ushort charCode)
		{
			return this.glyphIdToCharId.TryGetFromValue((byte)glyphId, out charCode);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			reader.ReadUShort();
			this.glyphIdToCharId = new ValueMapper<ushort, byte>();
			for (ushort num = 0; num < 256; num += 1)
			{
				this.glyphIdToCharId.AddPair(num, reader.Read());
			}
		}

		public override void Write(FontWriter writer)
		{
		}

		public override void Import(OpenTypeFontReader reader)
		{
		}

		const ushort GLYPH_IDS = 256;

		ValueMapper<ushort, byte> glyphIdToCharId;
	}
}
