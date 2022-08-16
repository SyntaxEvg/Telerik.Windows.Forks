using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	class RtfPadding
	{
		public RtfPadding()
		{
			this.Left = new RtfPaddingValue();
			this.Top = new RtfPaddingValue();
			this.Right = new RtfPaddingValue();
			this.Bottom = new RtfPaddingValue();
		}

		public RtfPaddingValue Left { get; set; }

		public RtfPaddingValue Top { get; set; }

		public RtfPaddingValue Right { get; set; }

		public RtfPaddingValue Bottom { get; set; }
	}
}
