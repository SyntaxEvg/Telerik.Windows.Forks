using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class TableRecord
	{
		public uint Tag { get; set; }

		public uint CheckSum { get; set; }

		public uint Offset { get; set; }

		public uint Length { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.Tag = reader.ReadULong();
			this.CheckSum = reader.ReadULong();
			this.Offset = reader.ReadULong();
			this.Length = reader.ReadULong();
		}

		public override string ToString()
		{
			return Tags.GetStringFromTag(this.Tag);
		}
	}
}
