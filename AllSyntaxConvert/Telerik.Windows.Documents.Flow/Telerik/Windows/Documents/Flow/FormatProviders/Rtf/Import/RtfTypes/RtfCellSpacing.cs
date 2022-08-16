using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes
{
	class RtfCellSpacing
	{
		public RtfCellSpacing()
		{
			this.Left = new RtfCellSpacingValue();
			this.Top = new RtfCellSpacingValue();
			this.Right = new RtfCellSpacingValue();
			this.Bottom = new RtfCellSpacingValue();
		}

		public RtfCellSpacingValue Left { get; set; }

		public RtfCellSpacingValue Top { get; set; }

		public RtfCellSpacingValue Right { get; set; }

		public RtfCellSpacingValue Bottom { get; set; }
	}
}
