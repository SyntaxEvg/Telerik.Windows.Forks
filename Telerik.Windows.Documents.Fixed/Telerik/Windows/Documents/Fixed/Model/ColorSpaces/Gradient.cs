using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public abstract class Gradient : PatternColor, IGradient, IPatternColor
	{
		public Gradient(Point startPoint, Point endPoint)
			: this(startPoint, endPoint, SimplePosition.Default)
		{
		}

		public Gradient(Point startPoint, Point endPoint, IPosition position)
		{
			this.StartPoint = startPoint;
			this.EndPoint = endPoint;
			this.Position = position;
			this.gradientStops = new GradientStopCollection();
			this.ExtendBefore = FixedDocumentDefaults.Extend;
			this.ExtendAfter = FixedDocumentDefaults.Extend;
		}

		public Point StartPoint { get; set; }

		public Point EndPoint { get; set; }

		public bool ExtendBefore { get; set; }

		public bool ExtendAfter { get; set; }

		public ColorBase Background { get; set; }

		public GradientStopCollection GradientStops
		{
			get
			{
				return this.gradientStops;
			}
		}

		public override IPosition Position { get; set; }

		internal override PatternType PatternType
		{
			get
			{
				return PatternType.Gradient;
			}
		}

		internal abstract GradientType GradientType { get; }

		IEnumerable<IGradientStop> IGradient.GradientStops
		{
			get
			{
				foreach (IGradientStop stop in this.GradientStops)
				{
					yield return stop;
				}
				yield break;
			}
		}

		Rect? IGradient.BoundingBox
		{
			get
			{
				return null;
			}
		}

		Color IGradient.StartColor
		{
			get
			{
				GradientStop gradientStop = this.GradientStops[0];
				return gradientStop.Color.ToColor();
			}
		}

		Color IGradient.EndColor
		{
			get
			{
				GradientStop gradientStop = this.GradientStops[this.GradientStops.Count - 1];
				return gradientStop.Color.ToColor();
			}
		}

		Color? IGradient.Background
		{
			get
			{
				Color? result = null;
				if (this.Background != null)
				{
					result = new Color?(this.Background.ToColor());
				}
				return result;
			}
		}

		public override bool Equals(ColorBase other)
		{
			Gradient gradient = other as Gradient;
			if (gradient == null)
			{
				return false;
			}
			if (this.GradientStops.Count != gradient.GradientStops.Count)
			{
				return false;
			}
			for (int i = 0; i < this.GradientStops.Count; i++)
			{
				if (!this.GradientStops[i].Equals(other))
				{
					return false;
				}
			}
			return this.Background == gradient.Background && this.EndPoint == gradient.EndPoint && this.ExtendAfter == gradient.ExtendAfter && this.ExtendBefore == gradient.ExtendBefore && this.StartPoint == gradient.StartPoint;
		}

		readonly GradientStopCollection gradientStops;
	}
}
