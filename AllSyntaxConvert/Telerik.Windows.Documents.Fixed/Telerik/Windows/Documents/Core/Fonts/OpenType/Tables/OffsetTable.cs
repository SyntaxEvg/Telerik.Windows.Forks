using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables
{
	class OffsetTable
	{
		public ushort NumTables { get; set; }

		public bool HasOpenTypeOutlines { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.HasOpenTypeOutlines = Tags.OTTO_TAG == reader.ReadULong();
			this.NumTables = reader.ReadUShort();
			reader.ReadUShort();
			reader.ReadUShort();
			reader.ReadUShort();
		}
	}
}
