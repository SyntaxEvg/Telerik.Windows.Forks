using System;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders
{
	class RtfParagraphBorder
	{
		public RtfParagraphBorder()
		{
			this.Top = new RtfBorder(false);
			this.Left = new RtfBorder(false);
			this.Bottom = new RtfBorder(false);
			this.Right = new RtfBorder(false);
			this.Between = new RtfBorder(false);
			this.Box = new RtfBorder(false);
		}

		public bool HasValue { get; set; }

		public RtfBorder Top { get; set; }

		public RtfBorder Left { get; set; }

		public RtfBorder Bottom { get; set; }

		public RtfBorder Right { get; set; }

		public RtfBorder Box { get; set; }

		public RtfBorder Between { get; set; }

		public ParagraphBorders CreateBorders()
		{
			return new ParagraphBorders(this.Left.CreateBorder(), this.Top.CreateBorder(), this.Right.CreateBorder(), this.Bottom.CreateBorder(), this.Between.CreateBorder());
		}

		public RtfParagraphBorder CreateCopy()
		{
			RtfParagraphBorder rtfParagraphBorder = new RtfParagraphBorder();
			if (this.HasValue)
			{
				rtfParagraphBorder.HasValue = true;
				rtfParagraphBorder.Top = this.Top.CreateCopy();
				rtfParagraphBorder.Left = this.Left.CreateCopy();
				rtfParagraphBorder.Bottom = this.Bottom.CreateCopy();
				rtfParagraphBorder.Right = this.Right.CreateCopy();
				rtfParagraphBorder.Between = this.Between.CreateCopy();
				rtfParagraphBorder.Box = this.Box.CreateCopy();
			}
			return rtfParagraphBorder;
		}
	}
}
