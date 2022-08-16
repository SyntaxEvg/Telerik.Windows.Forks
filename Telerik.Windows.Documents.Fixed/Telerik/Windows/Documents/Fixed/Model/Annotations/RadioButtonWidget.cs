using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class RadioButtonWidget : TwoStatesButtonWidget
	{
		internal RadioButtonWidget(FormField field)
			: base(field)
		{
			this.field = (RadioButtonField)field;
		}

		public RadioOption Option { get; internal set; }

		internal bool IsSelected
		{
			get
			{
				Guard.ThrowExceptionIfNull<RadioOption>(this.Option, "Option");
				return this.Option.Equals(this.field.Value, this.field.ShouldUpdateRadiosInUnison);
			}
		}

		bool IsRadioCircleTemplate
		{
			get
			{
				return base.TextProperties.Font.Name == "ZapfDingbats" && base.AppearanceCharacteristics.NormalCaption == "l";
			}
		}

		internal sealed override TwoStatesButtonWidget CreateClonedTwoStateButtonWidget(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedField = cloneContext.GetClonedField(base.Field);
			return new RadioButtonWidget(clonedField)
			{
				Option = cloneContext.GetClonedOption(this.Option)
			};
		}

		internal override GeometryBase CalculateBackgroundGeometry(Size size)
		{
			if (this.IsRadioCircleTemplate)
			{
				double num = ((base.Border != null) ? base.Border.Width : 0.0);
				return RadioButtonWidget.CreateCircleGeometry(size, num / 2.0);
			}
			return base.CalculateBackgroundGeometry(size);
		}

		internal override void CalculateBorder3dEffectGeometries(Size size, out PathGeometry pathAbove, out PathGeometry pathBelow)
		{
			if (this.IsRadioCircleTemplate)
			{
				double radiusOffset = ((base.Border != null) ? base.Border.Width : 0.0);
				Point circleCenter = RadioButtonWidget.GetCircleCenter(size);
				double circleRadius = RadioButtonWidget.GetCircleRadius(size, radiusOffset);
				double circleRadius2 = RadioButtonWidget.GetCircleRadius(size, base.BorderThicknessIncludingStyleThickness);
				Point point;
				Point point2;
				RadioButtonWidget.GetDiametralPoints(circleCenter, circleRadius, out point, out point2);
				Point point3;
				Point point4;
				RadioButtonWidget.GetDiametralPoints(circleCenter, circleRadius2, out point3, out point4);
				pathAbove = new PathGeometry();
				PathFigure pathFigure = pathAbove.Figures.AddPathFigure();
				pathFigure.IsClosed = true;
				pathFigure.StartPoint = point;
				pathFigure.Segments.AddArcSegment(point2, circleRadius, circleRadius);
				pathFigure.Segments.AddLineSegment(point4);
				pathFigure.Segments.AddArcSegment(point3, circleRadius2, circleRadius2).SweepDirection = SweepDirection.Counterclockwise;
				pathBelow = new PathGeometry();
				PathFigure pathFigure2 = pathBelow.Figures.AddPathFigure();
				pathFigure2.IsClosed = true;
				pathFigure2.StartPoint = point2;
				pathFigure2.Segments.AddArcSegment(point, circleRadius, circleRadius);
				pathFigure2.Segments.AddLineSegment(point3);
				pathFigure2.Segments.AddArcSegment(point4, circleRadius2, circleRadius2).SweepDirection = SweepDirection.Counterclockwise;
				return;
			}
			base.CalculateBorder3dEffectGeometries(size, out pathAbove, out pathBelow);
		}

		static void GetDiametralPoints(Point center, double radius, out Point leftPoint, out Point rightPoint)
		{
			double num = RadioButtonWidget.diagonalDirectionCoordinateValue * radius;
			leftPoint = new Point(center.X - num, center.Y + num);
			rightPoint = new Point(center.X + num, center.Y - num);
		}

		internal override GeometryBase CalculateContentClippingGeometry(Size size)
		{
			if (this.IsRadioCircleTemplate)
			{
				double borderThicknessIncludingStyleThickness = base.BorderThicknessIncludingStyleThickness;
				return RadioButtonWidget.CreateCircleGeometry(size, borderThicknessIncludingStyleThickness / 2.0);
			}
			return base.CalculateContentClippingGeometry(size);
		}

		internal override double ScaleBlockSize(Size blockSize)
		{
			if (this.IsRadioCircleTemplate)
			{
				Rect buttonContentBox = this.ButtonContentBox;
				double circleRadius = RadioButtonWidget.GetCircleRadius(new Size(buttonContentBox.Width, buttonContentBox.Height), 0.0);
				return 1.3 * circleRadius / blockSize.Width;
			}
			return base.ScaleBlockSize(blockSize);
		}

		static PathGeometry CreateCircleGeometry(Size boundingBox, double radiusOffset)
		{
			Point circleCenter = RadioButtonWidget.GetCircleCenter(boundingBox);
			double circleRadius = RadioButtonWidget.GetCircleRadius(boundingBox, radiusOffset);
			Point point = new Point(circleCenter.X - circleRadius, circleCenter.Y);
			Point point2 = new Point(circleCenter.X + circleRadius, circleCenter.Y);
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.IsClosed = true;
			pathFigure.StartPoint = point;
			pathFigure.Segments.AddArcSegment(point2, circleRadius, circleRadius);
			pathFigure.Segments.AddArcSegment(point, circleRadius, circleRadius);
			return pathGeometry;
		}

		static Point GetCircleCenter(Size boundingBox)
		{
			return new Point(boundingBox.Width / 2.0, boundingBox.Height / 2.0);
		}

		static double GetCircleRadius(Size boundingBox, double radiusOffset)
		{
			return Math.Min(boundingBox.Width, boundingBox.Height) / 2.0 - radiusOffset;
		}

		static readonly double diagonalDirectionCoordinateValue = 1.0 / Math.Sqrt(2.0);

		readonly RadioButtonField field;
	}
}
