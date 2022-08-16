using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CFFFormat.CFFTables
{
	class Header : CFFTable
	{
		public Header(ICFFFontFile file)
			: base(file, 0L)
		{
		}

		public byte HeaderSize { get; set; }

		public byte OffSize { get; set; }

		public override void Read(CFFFontReader reader)
		{
			reader.ReadCard8();
			reader.ReadCard8();
			this.HeaderSize = reader.ReadCard8();
			this.OffSize = reader.ReadOffSize();
		}
	}
}
