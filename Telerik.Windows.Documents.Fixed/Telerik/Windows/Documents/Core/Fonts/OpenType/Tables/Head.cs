using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class Head : TrueTypeTableBase
	{
		public Head(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public ushort Flags { get; set; }

		public short GlyphDataFormat { get; set; }

		public ushort UnitsPerEm { get; set; }

		public Rect BBox { get; set; }

		public short IndexToLocFormat { get; set; }

		public bool IsBold
		{
			get
			{
				return this.CheckMacStyle(0);
			}
		}

		public bool IsItalic
		{
			get
			{
				return this.CheckMacStyle(1);
			}
		}

		internal override uint Tag
		{
			get
			{
				return Tags.HEAD_TABLE;
			}
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadFixed();
			reader.ReadFixed();
			reader.ReadULong();
			reader.ReadULong();
			this.Flags = reader.ReadUShort();
			this.UnitsPerEm = reader.ReadUShort();
			reader.ReadLongDateTime();
			reader.ReadLongDateTime();
			this.BBox = new Rect(new Point((double)reader.ReadShort(), (double)reader.ReadShort()), new Point((double)reader.ReadShort(), (double)reader.ReadShort()));
			this.macStyle = reader.ReadUShort();
			reader.ReadUShort();
			reader.ReadShort();
			this.IndexToLocFormat = reader.ReadShort();
			reader.ReadShort();
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(this.UnitsPerEm);
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			this.UnitsPerEm = reader.ReadUShort();
		}

		bool CheckMacStyle(byte bit)
		{
			return ((int)this.macStyle & (1 << (int)bit)) != 0;
		}

		ushort macStyle;
	}
}
