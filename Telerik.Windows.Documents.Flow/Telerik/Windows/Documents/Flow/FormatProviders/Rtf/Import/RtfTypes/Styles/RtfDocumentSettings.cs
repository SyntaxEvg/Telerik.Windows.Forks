using System;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles
{
	class RtfDocumentSettings
	{
		public RtfDocumentSettings()
		{
			this.PageWidth = 816.0;
			this.PageHeight = 1056.0;
			this.LeftPageMargin = 120.0;
			this.RightPageMargin = 120.0;
			this.TopPageMargin = 96.0;
			this.BottomPageMargin = 96.0;
			this.VerticalAlignment = VerticalAlignment.Top;
		}

		public PageOrientation PageOrientation { get; set; }

		public double PageWidth { get; set; }

		public double PageHeight { get; set; }

		public double LeftPageMargin { get; set; }

		public double TopPageMargin { get; set; }

		public double RightPageMargin { get; set; }

		public double BottomPageMargin { get; set; }

		public VerticalAlignment VerticalAlignment { get; set; }

		const double DefaultPageWidth = 816.0;

		const double DefaultPageHeight = 1056.0;

		const double DefaultHorizontalMargin = 120.0;

		const double DefaultVerticalMargin = 96.0;

		const VerticalAlignment DefaultVerticalAlignment = VerticalAlignment.Top;
	}
}
