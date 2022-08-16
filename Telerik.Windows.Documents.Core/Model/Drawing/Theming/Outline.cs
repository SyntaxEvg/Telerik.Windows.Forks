using System;

namespace Telerik.Windows.Documents.Model.Drawing.Theming
{
	public class Outline
	{
		public Fill Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				if (this.fill != value)
				{
					this.fill = value;
					this.OnShapeOutlineChanged();
				}
			}
		}

		public double? Width
		{
			get
			{
				return this.width;
			}
			set
			{
				double? num = this.width;
				double? num2 = value;
				if (num.GetValueOrDefault() != num2.GetValueOrDefault() || num != null != (num2 != null))
				{
					this.width = value;
					this.OnShapeOutlineChanged();
				}
			}
		}

		internal Outline()
		{
		}

		internal Outline(Fill fill, double? width)
		{
			this.Fill = fill;
			this.Width = width;
		}

		public Outline Clone()
		{
			Fill fill = null;
			if (this.Fill != null)
			{
				this.Fill.Clone();
			}
			return new Outline(fill, this.Width);
		}

		internal event EventHandler ShapeOutlineChanged;

		internal void OnShapeOutlineChanged()
		{
			if (this.ShapeOutlineChanged != null)
			{
				this.ShapeOutlineChanged(this, EventArgs.Empty);
			}
		}

		Fill fill;

		double? width;
	}
}
