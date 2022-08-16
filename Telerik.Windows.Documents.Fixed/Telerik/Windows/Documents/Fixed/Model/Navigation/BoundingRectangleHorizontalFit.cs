using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class BoundingRectangleHorizontalFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.BoundingRectangleHorizontalFit;
			}
		}

		public double? Top { get; set; }

		internal override bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			return PageHorizontalFit.TryGetScaleFactor(base.Page, viewportSize, out scaleFactor);
		}

		internal override bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageHorizontalFit.TryGetHorizontalOffsetOnPage(out offset);
		}

		internal override bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageHorizontalFit.TryGetVerticalOffsetOnPage(this.Top, out offset);
		}

		internal override Destination CreateClonedInstance()
		{
			return new BoundingRectangleHorizontalFit
			{
				Top = this.Top
			};
		}
	}
}
