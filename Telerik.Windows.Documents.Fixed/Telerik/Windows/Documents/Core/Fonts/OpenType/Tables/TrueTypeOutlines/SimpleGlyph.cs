using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Utils;
using Telerik.Windows.Documents.Core.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.TrueTypeOutlines
{
	class SimpleGlyph : GlyphData
	{
		static bool XIsByte(byte[] flags, int index)
		{
			return BitsHelper.GetBit((int)flags[index], 1);
		}

		static bool YIsByte(byte[] flags, int index)
		{
			return BitsHelper.GetBit((int)flags[index], 2);
		}

		static bool Repeat(byte[] flags, int index)
		{
			return BitsHelper.GetBit((int)flags[index], 3);
		}

		static bool XIsSame(byte[] flags, int index)
		{
			return BitsHelper.GetBit((int)flags[index], 4);
		}

		static bool YIsSame(byte[] flags, int index)
		{
			return BitsHelper.GetBit((int)flags[index], 5);
		}

		public SimpleGlyph(OpenTypeFontSourceBase fontFile, ushort glyphIndex)
			: base(fontFile, glyphIndex)
		{
		}

		internal override IEnumerable<OutlinePoint[]> Contours
		{
			get
			{
				return this.contours;
			}
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort[] array = new ushort[(int)base.NumberOfContours];
			for (int i = 0; i < (int)base.NumberOfContours; i++)
			{
				array[i] = reader.ReadUShort();
			}
			int num = (int)(array[(int)(base.NumberOfContours - 1)] + 1);
			ushort num2 = reader.ReadUShort();
			byte[] array2 = new byte[(int)num2];
			for (int j = 0; j < (int)num2; j++)
			{
				array2[j] = reader.Read();
			}
			byte[] array3 = new byte[num];
			for (int k = 0; k < num; k++)
			{
				array3[k] = reader.Read();
				if (SimpleGlyph.Repeat(array3, k))
				{
					byte b = array3[k];
					byte b2 = reader.Read();
					for (int l = 0; l < (int)b2; l++)
					{
						array3[++k] = b;
					}
				}
			}
			int[] array4 = new int[num];
			for (int m = 0; m < num; m++)
			{
				if (m > 0)
				{
					array4[m] = array4[m - 1];
				}
				if (SimpleGlyph.XIsByte(array3, m))
				{
					int num3 = (int)reader.Read();
					if (!SimpleGlyph.XIsSame(array3, m))
					{
						num3 = -num3;
					}
					array4[m] += num3;
				}
				else if (!SimpleGlyph.XIsSame(array3, m))
				{
					array4[m] += (int)reader.ReadShort();
				}
			}
			int[] array5 = new int[num];
			for (int n = 0; n < num; n++)
			{
				if (n > 0)
				{
					array5[n] = array5[n - 1];
				}
				if (SimpleGlyph.YIsByte(array3, n))
				{
					int num4 = (int)reader.Read();
					if (!SimpleGlyph.YIsSame(array3, n))
					{
						num4 = -num4;
					}
					array5[n] += num4;
				}
				else if (!SimpleGlyph.YIsSame(array3, n))
				{
					array5[n] += (int)reader.ReadShort();
				}
			}
			this.contours = new List<OutlinePoint[]>();
			int num5 = 0;
			int num6 = 0;
			OutlinePoint[] array6 = new OutlinePoint[(int)(array[0] + 1)];
			this.contours.Add(array6);
			for (int num7 = 0; num7 < num; num7++)
			{
				array6[num6++] = new OutlinePoint((double)array4[num7], (double)array5[num7], array3[num7]);
				if (num7 == (int)array[num5])
				{
					if (num5 == array.Length - 1)
					{
						return;
					}
					num5++;
					array6 = new OutlinePoint[(int)(array[num5] - array[num5 - 1])];
					this.contours.Add(array6);
					num6 = 0;
				}
			}
		}

		List<OutlinePoint[]> contours;
	}
}
