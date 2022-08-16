using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Fonts.OpenType;
using Telerik.Windows.Documents.Core.Utilities;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class GlyphDescription
	{
		public ushort Flags { get; set; }

		public ushort GlyphIndex { get; set; }

		public Matrix Transform { get; set; }

		internal bool CheckFlag(byte bit)
		{
			return BitsHelper.GetBit((int)this.Flags, bit);
		}

		public void Read(OpenTypeFontReader reader)
		{
			this.Flags = reader.ReadUShort();
			this.GlyphIndex = reader.ReadUShort();
			int num = 0;
			int num2 = 0;
			if (this.CheckFlag(0))
			{
				if (this.CheckFlag(1))
				{
					num = (int)reader.ReadShort();
					num2 = (int)reader.ReadShort();
				}
				else
				{
					reader.ReadUShort();
					reader.ReadUShort();
				}
			}
			else if (this.CheckFlag(1))
			{
				num = (int)reader.ReadChar();
				num2 = (int)reader.ReadChar();
			}
			else
			{
				reader.ReadChar();
				reader.ReadChar();
			}
			float num3 = 1f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 1f;
			if (this.CheckFlag(3))
			{
				num6 = (num3 = reader.Read2Dot14());
			}
			else if (this.CheckFlag(6))
			{
				num3 = reader.Read2Dot14();
				num6 = reader.Read2Dot14();
			}
			else if (this.CheckFlag(7))
			{
				num3 = reader.Read2Dot14();
				num4 = reader.Read2Dot14();
				num5 = reader.Read2Dot14();
				num6 = reader.Read2Dot14();
			}
			this.Transform = new Matrix((double)num3, (double)num4, (double)num5, (double)num6, (double)num, (double)num2);
		}

		internal const int ARG_1_AND_2_ARE_WORDS = 0;

		internal const int ARGS_ARE_XY_VALUES = 1;

		internal const int ROUND_XY_TO_GRID = 2;

		internal const int WE_HAVE_A_SCALE = 3;

		internal const int MORE_COMPONENTS = 5;

		internal const int WE_HAVE_AN_X_AND_Y_SCALE = 6;

		internal const int WE_HAVE_A_TWO_BY_TWO = 7;

		internal const int WE_HAVE_INSTRUCTIONS = 8;

		internal const int USE_MY_METRICS = 9;

		internal const int OVERLAP_COMPOUND = 10;
	}
}
