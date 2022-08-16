using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class EncodingRecord
	{
		public ushort PlatformId { get; set; }

		public ushort EncodingId { get; set; }

		public uint Offset { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.PlatformId = reader.ReadUShort();
			this.EncodingId = reader.ReadUShort();
			this.Offset = reader.ReadULong();
		}
	}
}
