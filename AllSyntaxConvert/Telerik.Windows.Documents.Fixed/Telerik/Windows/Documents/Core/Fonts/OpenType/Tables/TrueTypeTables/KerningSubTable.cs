using System;
using System.IO;
using Telerik.Windows.Documents.Core.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeTables
{
	abstract class KerningSubTable : TableBase
	{
		internal static KerningSubTable ReadSubTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			long position = reader.Position;
			reader.ReadUShort();
			ushort num = reader.ReadUShort();
			ushort num2 = reader.ReadUShort();
			byte b = BitConverter.GetBytes(num2)[1];
			byte b2 = b;
			if (b2 == 0)
			{
				KerningSubTable kerningSubTable = new Format0KerningSubTable(fontSource);
				kerningSubTable.Coverage = num2;
				kerningSubTable.Read(reader);
				return kerningSubTable;
			}
			reader.Seek(position + (long)((ulong)num), SeekOrigin.Begin);
			return null;
		}

		internal static KerningSubTable ImportSubTable(OpenTypeFontSourceBase fontSource, OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			byte b = BitConverter.GetBytes(num)[1];
			byte b2 = b;
			if (b2 == 0)
			{
				KerningSubTable kerningSubTable = new Format0KerningSubTable(fontSource);
				kerningSubTable.Coverage = num;
				kerningSubTable.Import(reader);
				return kerningSubTable;
			}
			return null;
		}

		public KerningSubTable(OpenTypeFontSourceBase fontSource)
			: base(fontSource)
		{
		}

		public ushort Coverage { get; set; }

		public bool IsHorizontal
		{
			get
			{
				return this.GetBit(0);
			}
		}

		public bool HasMinimumValues
		{
			get
			{
				return this.GetBit(1);
			}
		}

		public bool IsCrossStream
		{
			get
			{
				return this.GetBit(2);
			}
		}

		public bool Override
		{
			get
			{
				return this.GetBit(3);
			}
		}

		bool GetBit(byte bit)
		{
			return BitsHelper.GetBit((int)this.Coverage, bit);
		}

		public abstract short GetValue(ushort leftGlyphIndex, ushort rightGlyphIndex);

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(this.Coverage);
		}
	}
}
