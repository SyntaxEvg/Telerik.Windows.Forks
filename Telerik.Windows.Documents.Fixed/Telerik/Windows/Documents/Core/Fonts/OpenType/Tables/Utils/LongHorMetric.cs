using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class LongHorMetric
	{
		public ushort AdvanceWidth { get; set; }

		public short LSB { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.AdvanceWidth = reader.ReadUShort();
			this.LSB = reader.ReadShort();
		}

		public void Write(FontWriter writer)
		{
			writer.WriteUShort(this.AdvanceWidth);
			writer.WriteShort(this.LSB);
		}

		public override string ToString()
		{
			return string.Format("AdvancedWidth: {0}; LSB: {1}", this.AdvanceWidth, this.LSB);
		}
	}
}
