using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class Lookup : TableBase
	{
		internal static bool IsSupported(ushort type)
		{
			switch (type)
			{
			case 1:
			case 2:
			case 4:
				return true;
			}
			return false;
		}

		public Lookup(OpenTypeFontSourceBase fontFile, ushort type)
			: base(fontFile)
		{
			this.Type = type;
		}

		public ushort Type { get; set; }

		public ushort Flags { get; set; }

		public IEnumerable<SubTable> SubTables
		{
			get
			{
				if (this.subTables == null)
				{
					this.subTables = new SubTable[this.subTableOffsets.Length];
					for (int i = 0; i < this.subTableOffsets.Length; i++)
					{
						this.subTables[i] = this.ReadSubtable(base.Reader, this.subTableOffsets[i]);
					}
				}
				return this.subTables;
			}
		}

		SubTable ReadSubtable(OpenTypeFontReader reader, ushort offset)
		{
			reader.BeginReadingBlock();
			long offset2 = base.Offset + (long)((ulong)offset);
			reader.Seek(offset2, SeekOrigin.Begin);
			SubTable subTable = SubTable.ReadSubTable(base.FontSource, reader, this.Type);
			subTable.Offset = offset2;
			reader.EndReadingBlock();
			return subTable;
		}

		public GlyphsSequence Apply(GlyphsSequence glyphIDs)
		{
			GlyphsSequence glyphsSequence = glyphIDs;
			foreach (SubTable subTable in this.SubTables)
			{
				glyphsSequence = subTable.Apply(glyphsSequence);
			}
			return glyphsSequence;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			this.Flags = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.subTableOffsets = new ushort[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.subTableOffsets[i] = reader.ReadUShort();
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(this.Type);
			writer.WriteUShort(this.Flags);
			writer.WriteUShort((ushort)this.subTableOffsets.Length);
			foreach (SubTable subTable in this.SubTables)
			{
				subTable.Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.Flags = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.subTables = new SubTable[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				SubTable subTable = SubTable.ImportSubTable(base.FontSource, reader, this.Type);
				this.subTables[i] = subTable;
			}
		}

		ushort[] subTableOffsets;

		SubTable[] subTables;
	}
}
