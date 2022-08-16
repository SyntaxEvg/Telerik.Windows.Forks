using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class LineSeries : CategorySeriesBase, ISupportMarker, ISupportSmooth
	{
		public override SeriesType SeriesType
		{
			get
			{
				return SeriesType.Line;
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
			LineSeries lineSeries = (LineSeries)series;
			lineSeries.isSmooth = this.isSmooth;
			if (this.marker != null)
			{
				lineSeries.marker = this.marker.Clone();
			}
		}

		void Marker_Changed(object sender, EventArgs e)
		{
			base.OnSeriesChanged();
		}

		Marker marker;

		bool isSmooth;
	}
}
