using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class FeatureRecord
	{
		public uint FeatureTag { get; set; }

		public ushort FeatureOffset { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.FeatureTag = reader.ReadULong();
			this.FeatureOffset = reader.ReadUShort();
		}

		public override string ToString()
		{
			return Tags.GetStringFromTag(this.FeatureTag);
		}
	}
}
