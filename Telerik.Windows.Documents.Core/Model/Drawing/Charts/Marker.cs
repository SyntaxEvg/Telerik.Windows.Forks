using System;
using Telerik.Windows.Documents.Model.Drawing.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class Marker
	{
		public Marker()
		{
			this.size = Marker.DefaultSize;
			this.Outline = new Outline();
			this.Outline.ShapeOutlineChanged += this.Outline_ShapeOutlineChanged;
		}

		public MarkerStyle Symbol
		{
			get
			{
				return this.symbol;
			}
			set
			{
				if (this.symbol != value)
				{
					this.symbol = value;
					this.OnChanged();
				}
			}
		}

		public byte Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (this.size != value)
				{
					Guard.ThrowExceptionIfLessThan<int>(2, (int)value, "value");
					Guard.ThrowExceptionIfGreaterThan<int>(72, (int)value, "value");
					this.size = value;
					this.OnChanged();
				}
			}
		}

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
					this.OnChanged();
				}
			}
		}

		public Outline Outline { get; set; }

		public Marker Clone()
		{
			return new Marker
			{
				Symbol = this.Symbol,
				Size = this.Size,
				Fill = this.Fill.Clone(),
				Outline = this.Outline.Clone()
			};
		}

		internal event EventHandler Changed;

		void OnChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		void Outline_ShapeOutlineChanged(object sender, EventArgs e)
		{
			this.OnChanged();
		}

		internal static readonly byte DefaultSize = 5;

		byte size;

		MarkerStyle symbol;

		Fill fill;
	}
}
