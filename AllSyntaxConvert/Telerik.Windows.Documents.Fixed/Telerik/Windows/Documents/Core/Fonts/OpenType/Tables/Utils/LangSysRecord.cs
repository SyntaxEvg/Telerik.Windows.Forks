using System;

namespace Telerik.Windows.Documents.Core.Fonts.OpenType.Tables.Utils
{
	class LangSysRecord
	{
		public uint LangSysTag { get; set; }

		public uint LangSys { get; set; }

		public void Read(OpenTypeFontReader reader)
		{
			this.LangSysTag = reader.ReadULong();
			this.LangSys = reader.ReadULong();
		}
	}
}
