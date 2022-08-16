using System;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class ScatterSeries : PointSeriesBase, ISupportMarker, ISupportSmooth
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Scatter;
			}
		}

		public ScatterStyle ScatterStyle
		{
			get
			{
				bool flag = this.Marker != null && this.Marker.Fill != null && !(this.Marker.Fill is NoFill);
				bool flag2 = false;
				if (base.Outline != null && (base.Outline.Fill != null || base.Outline.Width != null))
				{
					flag2 = !(base.Outline.Fill is NoFill);
				}
				if (flag2 && !this.isSmooth && !flag)
				{
					return ScatterStyle.Line;
				}
				if (flag2 && !this.isSmooth && flag)
				{
					return ScatterStyle.LineMarker;
				}
				if (!flag2 && flag)
				{
					return ScatterStyle.Marker;
				}
				if (flag2 && this.isSmooth && !flag)
				{
					return ScatterStyle.Smooth;
				}
				if (flag2 && this.isSmooth && flag)
				{
					return ScatterStyle.SmoothMarker;
				}
				return ScatterStyle.None;
			}
		}

		public bool IsSmooth
		{
			get
			{
				return this.isSmooth;
			}
			set
			{
				if (this.isSmooth != value)
				{
					this.isSmooth = value;
					base.OnSeriesChanged();
				}
			}
		}

		public Marker Marker
		{
			get
			{
				return this.marker;
			}
			set
			{
				if (this.marker != null)
				{
					this.marker.Changed -= this.Marker_Changed;
				}
				this.marker = value;
				if (this.marker != null)
				{
					this.marker.Changed += this.Marker_Changed;
				}
				base.OnSeriesChanged();
			}
		}

		internal override void CloneProperties(SeriesBase series)
		{
			base.CloneProperties(series);
			ScatterSeries scatterSeries = (ScatterSeries)series;
			scatterSeries.isSmooth = this.isSmooth;
			if (this.marker != null)
			{
				scatterSeries.marker = this.marker.Clone();
			}
		}

		void Marker_Changed(object sender, EventArgs e)
		{
			base.OnSeriesChanged();
		}

		bool isSmooth;

		Marker marker;
	}
}
