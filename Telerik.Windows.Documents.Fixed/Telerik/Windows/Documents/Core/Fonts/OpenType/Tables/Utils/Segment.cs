using System;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class Segment
	{
		internal Segment()
		{
		}

		public Segment(ushort startCode, ushort endCode, short idDelta)
		{
			this.startCode = startCode;
			this.endCode = endCode;
			this.idDelta = idDelta;
		}

		public Segment(ushort startCode, ushort endCode, short idDelta, ushort[] map)
			: this(startCode, endCode, idDelta)
		{
			this.map = new ValueMapper<ushort, ushort>();
			ushort num = 0;
			while ((int)num < map.Length)
			{
				this.map.AddPair(num, map[(int)num]);
				num += 1;
			}
		}

		public bool IsInside(int unicode)
		{
			return (int)this.startCode <= unicode && (int)this.endCode >= unicode;
		}

		public bool Contains(ushort glyphId)
		{
			if (this.map != null)
			{
				return this.map.ContainsToValue(glyphId);
			}
			ushort num = this.startCode + (ushort)this.idDelta;
			ushort num2 = this.endCode + (ushort)this.idDelta;
			return num <= glyphId && glyphId <= num2;
		}

		internal ushort GetCharId(ushort glyphId)
		{
			if (this.map != null)
			{
				return this.startCode + this.map.GetFromValue(glyphId - (ushort)this.idDelta);
			}
			return glyphId - (ushort)this.idDelta;
		}

		public bool TryGetGlyphId(int unicode, out ushort glyphId)
		{
			glyphId = 0;
			if (this.map != null)
			{
				ushort num = (ushort)(unicode - (int)this.startCode);
				if ((int)num > this.map.Count || this.map.GetToValue(num) == 0)
				{
					return false;
				}
				glyphId = this.map.GetToValue(num) + (ushort)this.idDelta;
			}
			else
			{
				glyphId = (ushort)(unicode + (int)this.idDelta);
			}
			return true;
		}

		public void Write(FontWriter writer)
		{
		}

		public void Import(OpenTypeFontReader reader)
		{
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.startCode.GetHashCode();
			return num * 23 + this.endCode.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Segment segment = obj as Segment;
			return segment != null && this.startCode == segment.startCode && this.endCode == segment.endCode;
		}

		readonly ushort startCode;

		readonly ushort endCode;

		readonly short idDelta;

		readonly ValueMapper<ushort, ushort> map;
	}
}
