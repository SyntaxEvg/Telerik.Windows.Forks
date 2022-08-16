using System;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CMapFormat6Table : CMapTable
	{
		public override ushort FirstCode
		{
			get
			{
				return this.firstCode;
			}
		}

		public override bool TryGetGlyphId(int charCode, out ushort glyphId)
		{
			return this.glyphIdToCharId.TryGetToValue((ushort)(charCode - (int)this.firstCode), out glyphId);
		}

		public override bool TryGetCharId(ushort glyphId, out ushort charId)
		{
			return this.glyphIdToCharId.TryGetFromValue((ushort)((byte)glyphId), out charId);
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			reader.ReadUShort();
			this.firstCode = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.glyphIdToCharId = new ValueMapper<ushort, ushort>();
			for (ushort num2 = 0; num2 < num; num2 += 1)
			{
				this.glyphIdToCharId.AddPair(num2, reader.ReadUShort());
			}
		}

		public override void Write(FontWriter writer)
		{
		}

		public override void Import(OpenTypeFontReader reader)
		{
		}

		ushort firstCode;

		ValueMapper<ushort, ushort> glyphIdToCharId;
	}
}
