using System;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class RectangleFit : Destination
	{
		internal override DestinationType DestinationType
		{
			get
			{
				return DestinationType.RectangleFit;
			}
		}

		public double? Left { get; set; }

		public double? Bottom { get; set; }

		public double? Top { get; set; }

		public double? Right { get; set; }

		bool IsValid
		{
			get
			{
				return this.Left != null && this.Bottom != null && this.Top != null && this.Right != null;
			}
		}

		Rect RectangleToFit
		{
			get
			{
				Guard.ThrowExceptionIfFalse(this.IsValid, "IsValid");
				double x = System.Math.Min(this.Left.Value, this.Right.Value);
				double y = System.Math.Min(this.Top.Value, this.Bottom.Value);
				double width = Math.Abs(this.Left.Value - this.Right.Value);
				double height = Math.Abs(this.Top.Value - this.Bottom.Value);
				return new Rect(x, y, width, height);
			}
		}

		internal override bool TryGetScaleFactor(Size viewportSize, out double scaleFactor)
		{
			if (this.IsValid)
			{
				Rect rectangleToFit = this.RectangleToFit;
				Size scale = Destination.GetScale(new Size(rectangleToFit.Width, rectangleToFit.Height), viewportSize);
				scaleFactor = System.Math.Min(scale.Width, scale.Height);
				return true;
			}
			return base.TryGetScaleFactor(viewportSize, out scaleFactor);
		}

		internal override bool TryGetHorizontalOffsetOnPage(Size viewportSize, out double offset)
		{
			if (this.IsValid)
			{
				offset = this.RectangleToFit.Left;
				return true;
			}
			return base.TryGetHorizontalOffsetOnPage(viewportSize, out offset);
		}

		internal override bool TryGetVerticalOffsetOnPage(Size viewportSize, out double offset)
		{
			if (this.IsValid)
			{
				offset = this.RectangleToFit.Top;
				return true;
			}
			return base.TryGetVerticalOffsetOnPage(viewportSize, out offset);
		}

		internal override Destination CreateClonedInstance()
		{
			return new RectangleFit
			{
				Left = this.Left,
				Top = this.Top,
				Right = this.Right,
				Bottom = this.Bottom
			};
		}
	}
}
