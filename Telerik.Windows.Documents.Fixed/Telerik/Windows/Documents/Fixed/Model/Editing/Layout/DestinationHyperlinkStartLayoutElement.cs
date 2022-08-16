using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Layout
{
	class DestinationHyperlinkStartLayoutElement : HyperlinkStartLayoutElement
	{
		public DestinationHyperlinkStartLayoutElement(Destination destination, FontBase font, double fontSize)
			: base(font, fontSize)
		{
			Guard.ThrowExceptionIfNull<Destination>(destination, "destination");
			this.destination = destination;
		}

		public Destination Destination
		{
			get
			{
				return this.destination;
			}
		}

		public override Annotation CreateAnnotation()
		{
			return new Link(this.destination);
		}

		public override StartMarkerLayoutElement Clone()
		{
			return new DestinationHyperlinkStartLayoutElement(this.destination, base.Font, base.FontSize);
		}

		readonly Destination destination;
	}
}
