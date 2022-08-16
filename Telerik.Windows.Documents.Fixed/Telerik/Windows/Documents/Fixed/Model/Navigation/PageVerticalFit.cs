using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class PageVerticalFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.PageVerticalFit;
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

		internal static bool TryGetHorizontalOffsetOnPage(double? left, out double offset)
		{
			if (left != null)
			{
				offset = left.Value;
				return true;
			}
			offset = 0.0;
			return false;
		}

		internal static bool TryGetVerticalOffsetOnPage(out double offset)
		{
			offset = 0.0;
			return true;
		}

		internal static bool TryGetScaleFactor(RadFixedPage page, Size viewportSize, out double scaleFactor)
		{
			scaleFactor = Destination.GetScale(page.Size.Height, viewportSize.Height);
			return true;
		}

		internal override Destination CreateClonedInstance()
		{
			return new PageVerticalFit
			{
				Left = this.Left
			};
		}
	}
}
