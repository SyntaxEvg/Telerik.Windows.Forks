using System;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class Location : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.Location;
			}
		}

		public double? Left { get; set; }

		public double? Top { get; set; }

		public double? Zoom { get; set; }

		internal override bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			if (this.Zoom != null && this.Zoom.Value > 0.0)
			{
				scaleFactor = this.Zoom.Value;
				return true;
			}
			return base.TryGetScaleFactor(viewportSize, out scaleFactor);
		}

		internal override bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			if (this.Left != null)
			{
				offset = this.Left.Value;
				return true;
			}
			return base.TryGetHorizontalOffsetOnPage(viewportSize, out offset);
		}

		internal override bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			if (this.Top != null)
			{
				offset = this.Top.Value;
				return true;
			}
			return base.TryGetVerticalOffsetOnPage(viewportSize, out offset);
		}

		internal override Destination CreateClonedInstance()
		{
			return new Location
			{
				Left = this.Left,
				Top = this.Top,
				Zoom = this.Zoom
			};
		}
	}
}
