using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class CoverageFormat2 : Coverage
	{
		public CoverageFormat2(OpenTypeFontSourceBase fontFile)
			: base(fontFile)
		{
		}

		public override void Read(OpenTypeFontReader reader)
		{
			ushort num = reader.ReadUShort();
			this.rangeRecords = new RangeRecord[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				this.rangeRecords[i] = new RangeRecord();
				this.rangeRecords[i].Read(reader);
			}
		}

		internal override void Write(FontWriter writer)
		{
			writer.WriteUShort(2);
			writer.WriteUShort((ushort)this.rangeRecords.Length);
			for (int i = 0; i < this.rangeRecords.Length; i++)
			{
				this.rangeRecords[i].Write(writer);
			}
		}

		public override int GetCoverageIndex(ushort glyphIndex)
		{
			foreach (RangeRecord rangeRecord in this.rangeRecords)
			{
				if (rangeRecord.Start <= glyphIndex && glyphIndex <= rangeRecord.End)
				{
					return rangeRecord.GetCoverageIndex(glyphIndex);
				}
			}
			return -1;
		}

		RangeRecord[] rangeRecords;
	}
}
