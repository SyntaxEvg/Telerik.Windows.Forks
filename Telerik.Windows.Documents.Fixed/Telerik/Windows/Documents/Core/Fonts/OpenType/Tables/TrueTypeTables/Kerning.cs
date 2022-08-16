using System;
using System.Collections.Generic;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeTables
{
	class Kerning : TrueTypeTableBase
	{
		public Kerning(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		internal override uint Tag
		{
			get
			{
				return Tags.KERN_TABLE;
			}
		}

		public KerningInfo GetKerning(ushort leftGlyphIndex, ushort rightGlyphIndex)
		{
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double num4 = 0.0;
			short? num5 = null;
			short? num6 = null;
			short? num7 = null;
			short? num8 = null;
			foreach (KerningSubTable kerningSubTable in this.subTables)
			{
				short value = kerningSubTable.GetValue(leftGlyphIndex, rightGlyphIndex);
				if (kerningSubTable.IsHorizontal)
				{
					if (kerningSubTable.HasMinimumValues)
					{
						if (kerningSubTable.IsCrossStream)
						{
							num6 = new short?(value);
						}
						else
						{
							num5 = new short?(value);
						}
					}
					else if (kerningSubTable.IsCrossStream)
					{
						if (kerningSubTable.Override)
						{
							num3 = (double)value;
						}
						else
						{
							num3 += (double)value;
						}
					}
					else if (kerningSubTable.Override)
					{
						num += (double)value;
					}
					else
					{
						num = (double)value;
					}
				}
				else if (kerningSubTable.HasMinimumValues)
				{
					if (kerningSubTable.IsCrossStream)
					{
						num8 = new short?(value);
					}
					else
					{
						num7 = new short?(value);
					}
				}
				else if (kerningSubTable.IsCrossStream)
				{
					if (kerningSubTable.Override)
					{
						num4 = (double)value;
					}
					else
					{
						num4 += (double)value;
					}
				}
				else if (kerningSubTable.Override)
				{
					num2 = (double)value;
				}
				else
				{
					num2 += (double)value;
				}
			}
			if (num5 != null)
			{
				short? num9 = num5;
				double num10 = num;
				if ((double)num9.GetValueOrDefault() > num10 && num9 != null)
				{
					num = (double)num5.Value;
				}
			}
			if (num6 != null)
			{
				short? num11 = num6;
				double num12 = num3;
				if ((double)num11.GetValueOrDefault() > num12 && num11 != null)
				{
					num3 = (double)num6.Value;
				}
			}
			if (num7 != null)
			{
				short? num13 = num7;
				double num14 = num2;
				if ((double)num13.GetValueOrDefault() > num14 && num13 != null)
				{
					num2 = (double)num7.Value;
				}
			}
			if (num8 != null)
			{
				short? num15 = num8;
				double num16 = num4;
				if ((double)num15.GetValueOrDefault() > num16 && num15 != null)
				{
					num4 = (double)num8.Value;
				}
			}
			return new KerningInfo
			{
				HorizontalKerning = new Point(num, num3),
				VerticalKerning = new Point(num2, num4)
			};
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.subTables = new List<KerningSubTable>((int)num);
			for (int i = 0; i < (int)num; i++)
			{
				KerningSubTable kerningSubTable = KerningSubTable.ReadSubTable(base.FontSource, reader);
				if (kerningSubTable != null)
				{
					this.subTables.Add(kerningSubTable);
				}
			}
		}

		internal override void Write(FontWriter writer)
		{
			if (this.subTables == null)
			{
				writer.WriteUShort(0);
				return;
			}
			writer.WriteUShort((ushort)this.subTables.Count);
			for (int i = 0; i < this.subTables.Count; i++)
			{
				this.subTables[i].Write(writer);
			}
		}

		internal override void Import(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			if (num > 0)
			{
				this.subTables = new List<KerningSubTable>((int)num);
				for (int i = 0; i < (int)num; i++)
				{
					this.subTables.Add(KerningSubTable.ImportSubTable(base.FontSource, reader));
				}
			}
		}

		List<KerningSubTable> subTables;
	}
}
