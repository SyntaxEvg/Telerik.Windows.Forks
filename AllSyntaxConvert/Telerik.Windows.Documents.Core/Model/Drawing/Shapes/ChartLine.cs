using System;
using Telerik.Windows.Documents.Model.Drawing.Theming;

namespace Telerik.Windows.Documents.Model.Drawing.Shapes
{
	public class ChartLine
	{
		public Outline Outline { get; set; }

		internal ChartLine()
		{
			this.Outline = new Outline();
			this.Outline.ShapeOutlineChanged += this.Outline_ShapeOutlineChanged;
		}

		public ChartLine Clone()
		{
			return new ChartLine
			{
				Outline = 
				{
					Fill = this.Outline.Fill,
					Width = this.Outline.Width
				}
			};
		}

		void Outline_ShapeOutlineChanged(object sender, EventArgs e)
		{
			this.OnChartLineChanged();
		}

		internal event EventHandler ChartLineChanged;

		internal void OnChartLineChanged()
		{
			if (this.ChartLineChanged != null)
			{
				this.ChartLineChanged(this, EventArgs.Empty);
			}
		}
	}
}
