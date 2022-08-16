using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class BoundingRectangleFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.BoundingRectangleFit;
			}
		}

		internal override bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			return PageFit.TryGetScaleFactor(base.Page, viewportSize, out scaleFactor);
		}

		internal override bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageFit.TryGetHorizontalOffsetOnPage(out offset);
		}

		internal override bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageFit.TryGetVerticalOffsetOnPage(out offset);
		}

		internal override Destination CreateClonedInstance()
		{
			return new BoundingRectangleFit();
		}
	}
}
