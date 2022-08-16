using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class RangeRecord
	{
		public ushort Start { get; set; }

		public ushort End { get; set; }

		public ushort StartCoverageIndex { get; set; }

		public int GetCoverageIndex(ushort glyphIndex)
		{
			return (int)(this.StartCoverageIndex + glyphIndex - this.Start);
		}

		public void Read(OpenTypeFontReader reader)
		{
			this.Start = reader.ReadUShort();
			this.End = reader.ReadUShort();
			this.StartCoverageIndex = reader.ReadUShort();
		}

		internal void Write(FontWriter writer)
		{
			writer.WriteUShort(this.Start);
			writer.WriteUShort(this.End);
			writer.WriteUShort(this.StartCoverageIndex);
		}
	}
}
