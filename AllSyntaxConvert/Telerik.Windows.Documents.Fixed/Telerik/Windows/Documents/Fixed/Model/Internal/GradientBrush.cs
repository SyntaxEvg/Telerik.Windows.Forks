using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	abstract class GradientBrush : PatternBrush, IGradient, IPatternColor
	{
		public GradientBrush(Color startColor, Color endColor, Point startPoint, Point endPoint, bool extendBefore, bool extendAfter, Color? background, Matrix transform, Rect? boundingBox)
			: base(transform)
		{
			this.startColor = startColor;
			this.endColor = endColor;
			this.startPoint = startPoint;
			this.endPoint = endPoint;
			this.extendBefore = extendBefore;
			this.extendAfter = extendAfter;
			this.background = background;
			this.boundingBox = boundingBox;
		}

		public Rect? BoundingBox
		{
			get
			{
				return this.boundingBox;
			}
		}

		public Point EndPoint
		{
			get
			{
				return this.endPoint;
			}
		}

		public Point StartPoint
		{
			get
			{
				return this.startPoint;
			}
		}

		public GradientStop[] GradientStops { get; set; }

		public Color? Background
		{
			get
			{
				return this.background;
			}
		}

		public bool ExtendAfter
		{
			get
			{
				return this.extendAfter;
			}
		}

		public bool ExtendBefore
		{
			get
			{
				return this.extendBefore;
			}
		}

		public Color EndColor
		{
			get
			{
				return this.endColor;
			}
		}

		public Color StartColor
		{
			get
			{
				return this.startColor;
			}
		}

		Color IGradient.StartColor
		{
			get
			{
				return new Color(base.AlphaConstant, this.StartColor.R, this.StartColor.G, this.StartColor.B);
			}
		}

		Color IGradient.EndColor
		{
			get
			{
				return new Color(base.AlphaConstant, this.EndColor.R, this.EndColor.G, this.EndColor.B);
			}
		}

		IEnumerable<IGradientStop> IGradient.GradientStops
		{
			get
			{
				if (this.GradientStops != null)
				{
					foreach (GradientStop stop in this.GradientStops)
					{
						byte alphaConstant = base.AlphaConstant;
						GradientStop gradientStop = stop;
						byte r = gradientStop.Color.R;
						GradientStop gradientStop2 = stop;
						byte g = gradientStop2.Color.G;
						GradientStop gradientStop3 = stop;
						Color color = new Color(alphaConstant, r, g, gradientStop3.Color.B);
						GradientStop gradientStop4 = stop;
						GradientStop stopWithAlpha = new GradientStop(color, gradientStop4.Offset);
						yield return stopWithAlpha;
					}
				}
				yield break;
			}
		}

		public override Brush Clone()
		{
			GradientBrush gradientBrush = this.CloneOverride();
			gradientBrush.GradientStops = (GradientStop[])this.GradientStops.Clone();
			return gradientBrush;
		}

		protected abstract GradientBrush CloneOverride();

		readonly Point startPoint;

		readonly Point endPoint;

		readonly Color startColor;

		readonly Color endColor;

		readonly bool extendBefore;

		readonly bool extendAfter;

		readonly Color? background;

		readonly Rect? boundingBox;
	}
}
