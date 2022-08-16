using System;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public abstract class Axis
	{
		protected Axis()
		{
			this.isVisible = true;
			this.outline = new Outline();
			this.outline.ShapeOutlineChanged += this.ShapeOutline_ShapeOutlineChanged;
			this.majorGridlines = new ChartLine();
			this.majorGridlines.ChartLineChanged += this.MajorGridlines_ChartLineChanged;
			this.numberFormat = string.Empty;
		}

		public abstract AxisType AxisType { get; }

		public double? Min
		{
			get
			{
				return this.min;
			}
			set
			{
				double? num = this.min;
				double? num2 = value;
				if (num.GetValueOrDefault() != num2.GetValueOrDefault() || num != null != (num2 != null))
				{
					this.min = value;
					this.OnAxisChanged();
				}
			}
		}

		public double? Max
		{
			get
			{
				return this.max;
			}
			set
			{
				double? num = this.max;
				double? num2 = value;
				if (num.GetValueOrDefault() != num2.GetValueOrDefault() || num != null != (num2 != null))
				{
					this.max = value;
					this.OnAxisChanged();
				}
			}
		}

		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (this.isVisible != value)
				{
					this.isVisible = value;
					this.OnAxisChanged();
				}
			}
		}

		public string NumberFormat
		{
			get
			{
				return this.numberFormat;
			}
			set
			{
				if (this.numberFormat != value)
				{
					Guard.ThrowExceptionIfNull<string>(value, "value");
					this.numberFormat = value;
					this.OnAxisChanged();
				}
			}
		}

		public Outline Outline
		{
			get
			{
				return this.outline;
			}
		}

		public ChartLine MajorGridlines
		{
			get
			{
				return this.majorGridlines;
			}
		}

		public Axis Clone()
		{
			Axis axis = (Axis)Activator.CreateInstance(base.GetType());
			axis.Min = this.Min;
			axis.Max = this.Max;
			axis.IsVisible = this.IsVisible;
			axis.NumberFormat = this.NumberFormat;
			axis.Outline.Fill = this.Outline.Fill.Clone();
			axis.Outline.Width = this.Outline.Width;
			axis.MajorGridlines.Outline.Fill = this.MajorGridlines.Outline.Fill.Clone();
			axis.MajorGridlines.Outline.Width = this.MajorGridlines.Outline.Width;
			return axis;
		}

		void ShapeOutline_ShapeOutlineChanged(object sender, EventArgs e)
		{
			this.OnAxisChanged();
		}

		void MajorGridlines_ChartLineChanged(object sender, EventArgs e)
		{
			this.OnAxisChanged();
		}

		internal event EventHandler AxisChanged;

		internal void OnAxisChanged()
		{
			if (this.AxisChanged != null)
			{
				this.AxisChanged(this, EventArgs.Empty);
			}
		}

		double? min;

		double? max;

		bool isVisible;

		string numberFormat;

		readonly Outline outline;

		readonly ChartLine majorGridlines;
	}
}
