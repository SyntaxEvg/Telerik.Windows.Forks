using System;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	abstract class StartMarkerLayoutElement : MarkerLayoutElementBase
	{
		public StartMarkerLayoutElement(FontBase font, double fontSize)
			: base(font, fontSize)
		{
		}

		public double OffsetX { get; set; }

		public LineInfo Line { get; set; }

		public Block Block { get; set; }

		public abstract EndMarkerLayoutElement CreateEnd();

		public abstract StartMarkerLayoutElement Clone();

		internal override void Draw(DrawLayoutElementContext context)
		{
			Guard.ThrowExceptionIfNull<DrawLayoutElementContext>(context, "context");
			this.Block = context.Block;
			this.Line = context.Line;
			this.OffsetX = context.OffsetX;
		}
	}
}
