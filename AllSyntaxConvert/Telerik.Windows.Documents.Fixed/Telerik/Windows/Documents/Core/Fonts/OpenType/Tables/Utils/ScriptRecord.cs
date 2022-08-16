using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class ScriptRecord
	{
		public uint ScriptTag { get; set; }

		public ushort ScriptOffset { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.ScriptTag = reader.ReadULong();
			this.ScriptOffset = reader.ReadUShort();
		}

		public override string ToString()
		{
			return Tags.GetStringFromTag(this.ScriptTag);
		}
	}
}
