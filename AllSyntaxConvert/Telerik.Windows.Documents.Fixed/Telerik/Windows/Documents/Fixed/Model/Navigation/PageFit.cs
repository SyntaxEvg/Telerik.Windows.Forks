using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class PageFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.PageFit;
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

		internal static bool TryGetScaleFactor(RadFixedPage page, Size viewportSize, out double scaleFactor)
		{
			Size scale = Destination.GetScale(page.Size, viewportSize);
			scaleFactor = System.Math.Min(scale.Width, scale.Height);
			return true;
		}

		internal static bool TryGetHorizontalOffsetOnPage(out double offset)
		{
			offset = 0.0;
			return true;
		}

		internal static bool TryGetVerticalOffsetOnPage(out double offset)
		{
			offset = 0.0;
			return true;
		}

		internal override Destination CreateClonedInstance()
		{
			return new PageFit();
		}
	}
}
