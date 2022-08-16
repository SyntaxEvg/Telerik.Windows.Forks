using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	abstract class HyperlinkStartLayoutElement : StartMarkerLayoutElement<HyperlinkStartLayoutElement>
	{
		public HyperlinkStartLayoutElement(FontBase font, double fontSize)
			: base(font, fontSize)
		{
		}

		public override EndMarkerLayoutElement CreateEnd()
		{
			return new HyperlinkEndLayoutElement(this, base.Font, base.FontSize);
		}

		public abstract Annotation CreateAnnotation();
	}
}
