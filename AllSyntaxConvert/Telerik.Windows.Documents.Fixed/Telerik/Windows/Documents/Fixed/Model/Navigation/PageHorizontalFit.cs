using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class PageHorizontalFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.PageHorizontalFit;
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

		internal static bool TryGetScaleFactor(RadFixedPage page, Size viewportSize, out double scaleFactor)
		{
			scaleFactor = Destination.GetScale(page.Size.Width, viewportSize.Width);
			return true;
		}

		internal static bool TryGetHorizontalOffsetOnPage(out double offset)
		{
			offset = 0.0;
			return true;
		}

		internal static bool TryGetVerticalOffsetOnPage(double? top, out double offset)
		{
			if (top != null)
			{
				offset = top.Value;
				return true;
			}
			offset = 0.0;
			return false;
		}

		internal override Destination CreateClonedInstance()
		{
			return new PageHorizontalFit
			{
				Top = this.Top
			};
		}
	}
}
