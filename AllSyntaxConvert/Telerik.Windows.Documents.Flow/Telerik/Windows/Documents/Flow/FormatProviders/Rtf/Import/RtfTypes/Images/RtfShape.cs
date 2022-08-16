using System;
using Telerik.Windows.Documents.Flow.Model.Shapes;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images
{
	class RtfShape : RtfImageBase
	{
		public RtfShape()
		{
			this.Wrapping = new ShapeWrapping();
		}

		public double Right { get; set; }

		public double Bottom { get; set; }

		public double Left { get; set; }

		public double Top { get; set; }

		public HorizontalRelativeFrom HorizontalRelativeFrom { get; set; }

		public bool IgnoreHorizontalRelativeFrom { get; set; }

		public VerticalRelativeFrom VerticalRelativeFrom { get; set; }

		public bool IgnoreVerticalRelativeFrom { get; set; }

		public int ZIndex { get; set; }

		public bool IsInHeader { get; set; }

		public bool IsBelowText { get; set; }

		public ShapeWrapping Wrapping { get; set; }
	}
}
