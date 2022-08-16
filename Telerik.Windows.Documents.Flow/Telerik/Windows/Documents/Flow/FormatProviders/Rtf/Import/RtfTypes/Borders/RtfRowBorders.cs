using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders
{
	class RtfRowBorders
	{
		public RtfRowBorders()
		{
			this.Top = new RtfBorder(false);
			this.Left = new RtfBorder(false);
			this.Bottom = new RtfBorder(false);
			this.Right = new RtfBorder(false);
			this.InnerVertical = new RtfBorder(false);
			this.InnerHorizontal = new RtfBorder(false);
		}

		public RtfBorder Top { get; set; }

		public RtfBorder Left { get; set; }

		public RtfBorder Bottom { get; set; }

		public RtfBorder Right { get; set; }

		public RtfBorder InnerVertical { get; set; }

		public RtfBorder InnerHorizontal { get; set; }

		public TableBorders CreateBorders()
		{
			return new TableBorders(this.Left.CreateBorder(), this.Top.CreateBorder(), this.Right.CreateBorder(), this.Bottom.CreateBorder(), this.InnerHorizontal.CreateBorder(), this.InnerVertical.CreateBorder());
		}
	}
}
