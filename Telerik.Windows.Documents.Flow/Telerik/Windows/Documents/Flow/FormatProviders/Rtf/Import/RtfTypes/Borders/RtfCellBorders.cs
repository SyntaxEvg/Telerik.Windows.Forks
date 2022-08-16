using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders
{
	class RtfCellBorders
	{
		public RtfCellBorders()
		{
			this.Top = new RtfBorder(false);
			this.Left = new RtfBorder(false);
			this.Bottom = new RtfBorder(false);
			this.Right = new RtfBorder(false);
			this.DiagonalLowerLeft = new RtfBorder(true);
			this.DiagonalUpperLeft = new RtfBorder(true);
		}

		public RtfBorder Top { get; set; }

		public RtfBorder Left { get; set; }

		public RtfBorder Bottom { get; set; }

		public RtfBorder Right { get; set; }

		public RtfBorder DiagonalLowerLeft { get; set; }

		public RtfBorder DiagonalUpperLeft { get; set; }

		public TableCellBorders CreateBorders()
		{
			return new TableCellBorders(this.Left.CreateBorder(), this.Top.CreateBorder(), this.Right.CreateBorder(), this.Bottom.CreateBorder(), null, null, this.DiagonalUpperLeft.CreateBorder(), this.DiagonalLowerLeft.CreateBorder());
		}
	}
}
