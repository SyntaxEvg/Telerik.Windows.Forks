using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class NameRecord
	{
		public ushort PlatformID { get; set; }

		public ushort EncodingID { get; set; }

		public ushort LanguageID { get; set; }

		public ushort NameID { get; set; }

		public ushort Length { get; set; }

		public ushort Offset { get; set; }

		public override bool Equals(object obj)
		{
			NameRecord nameRecord = obj as NameRecord;
			return nameRecord != null && (this.EncodingID == nameRecord.EncodingID && this.LanguageID == nameRecord.LanguageID && this.Length == nameRecord.Length && this.NameID == nameRecord.NameID) && this.PlatformID == nameRecord.PlatformID;
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.PlatformID.GetHashCode();
			num = num * 23 + this.EncodingID.GetHashCode();
			num = num * 23 + this.LanguageID.GetHashCode();
			num = num * 23 + this.NameID.GetHashCode();
			return num * 23 + this.Length.GetHashCode();
		}

		public void Read(OpenTypeFontReader reader)
		{
			this.PlatformID = reader.ReadUShort();
			this.EncodingID = reader.ReadUShort();
			this.LanguageID = reader.ReadUShort();
			this.NameID = reader.ReadUShort();
			this.Length = reader.ReadUShort();
			this.Offset = reader.ReadUShort();
		}
	}
}
