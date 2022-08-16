using System;
using System.IO;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CMapFormat4Table : CMapTable
	{
		public override ushort FirstCode
		{
			get
			{
				return this.firstCode;
			}
		}

		public override bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			glyphId = 0;
			foreach (Segment segment in this.segments)
			{
				if (segment.IsInside(unicode))
				{
					return segment.TryGetGlyphId(unicode, out glyphId);
				}
			}
			return false;
		}

		public override bool TryGetCharId(ushort glyphId, out ushort charId)
		{
			charId = 0;
			if (glyphId == 0)
			{
				return false;
			}
			foreach (Segment segment in this.segments)
			{
				if (segment.Contains(glyphId))
				{
					charId = segment.GetCharId(glyphId);
					return true;
				}
			}
			return false;
		}

		public override void Read(OpenTypeFontReader reader)
		{
			reader.ReadUShort();
			reader.ReadUShort();
			ushort num = (ushort)(reader.ReadUShort() / 2);
			reader.ReadUShort();
			reader.ReadUShort();
			reader.ReadUShort();
			ushort[] array = new ushort[(int)num];
			ushort[] array2 = new ushort[(int)num];
			short[] array3 = new short[(int)num];
			ushort[] array4 = new ushort[(int)num];
			this.segments = new Segment[(int)num];
			this.firstCode = ushort.MaxValue;
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = reader.ReadUShort();
			}
			reader.ReadUShort();
			for (int j = 0; j < (int)num; j++)
			{
				array2[j] = reader.ReadUShort();
				if (this.firstCode > array2[j])
				{
					this.firstCode = array2[j];
				}
			}
			for (int k = 0; k < (int)num; k++)
			{
				array3[k] = reader.ReadShort();
			}
			for (int l = 0; l < (int)num; l++)
			{
				long num2 = reader.Position;
				array4[l] = reader.ReadUShort();
				if (array4[l] <= 0)
				{
					this.segments[l] = new Segment(array2[l], array[l], array3[l]);
				}
				else
				{
					num2 += (long)((ulong)array4[l]);
					int num3 = (int)(array[l] - array2[l] + 1);
					ushort[] array5 = new ushort[num3];
					reader.BeginReadingBlock();
					reader.Seek(num2, SeekOrigin.Begin);
					for (int m = 0; m < array5.Length; m++)
					{
						array5[m] = reader.ReadUShort();
					}
					this.segments[l] = new Segment(array2[l], array[l], array3[l], array5);
					reader.EndReadingBlock();
				}
			}
		}

		public override void Write(FontWriter writer)
		{
			writer.WriteUShort(4);
			writer.WriteUShort(this.firstCode);
			writer.WriteUShort((ushort)this.segments.Length);
			for (int i = 0; i < this.segments.Length; i++)
			{
				this.segments[i].Write(writer);
			}
		}

		public override void Import(OpenTypeFontReader reader)
		{
			this.firstCode = reader.ReadUShort();
			ushort num = reader.ReadUShort();
			this.segments = new Segment[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				Segment segment = new Segment();
				segment.Import(reader);
				this.segments[i] = segment;
			}
		}

		Segment[] segments;

		ushort firstCode;
	}
}
