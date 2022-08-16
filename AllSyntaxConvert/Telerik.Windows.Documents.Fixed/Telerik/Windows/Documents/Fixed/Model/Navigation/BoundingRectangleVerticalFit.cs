using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class BoundingRectangleVerticalFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.BoundingRectangleVerticalFit;
			}
		}

		public double? Left { get; set; }

		internal override bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			return PageVerticalFit.TryGetScaleFactor(base.Page, viewportSize, out scaleFactor);
		}

		internal override bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageVerticalFit.TryGetHorizontalOffsetOnPage(this.Left, out offset);
		}

		internal override bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			return PageVerticalFit.TryGetVerticalOffsetOnPage(out offset);
		}

		internal override Destination CreateClonedInstance()
		{
			return new BoundingRectangleVerticalFit
			{
				Left = this.Left
			};
		}
	}
}
