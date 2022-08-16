using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class AxisGroup
	{
		public AxisGroup(Axis categoryAxis, Axis valueAxis)
		{
			this.CategoryAxis = categoryAxis;
			this.ValueAxis = valueAxis;
		}

		public AxisGroup()
		{
			this.CategoryAxis = new CategoryAxis();
			this.ValueAxis = new ValueAxis();
		}

		public Axis CategoryAxis
		{
			get
			{
				return this.categoryAxis;
			}
			set
			{
				if (this.categoryAxis != value)
				{
					if (value == null)
					{
						throw new ArgumentNullException("value", "The axis group cannot have axis with value null.");
					}
					if (this.categoryAxis != null)
					{
						this.categoryAxis.AxisChanged -= this.Axis_AxisChanged;
					}
					this.categoryAxis = value;
					this.categoryAxis.AxisChanged += this.Axis_AxisChanged;
					this.OnCollectionChanged();
				}
			}
		}

		public Axis ValueAxis
		{
			get
			{
				return this.valueAxis;
			}
			set
			{
				if (this.valueAxis != value)
				{
					if (value == null)
					{
						throw new ArgumentNullException("value", "The axis group cannot have axis with value null.");
					}
					if (this.valueAxis != null)
					{
						this.valueAxis.AxisChanged -= this.Axis_AxisChanged;
					}
					this.valueAxis = value;
					this.valueAxis.AxisChanged += this.Axis_AxisChanged;
					this.OnCollectionChanged();
				}
			}
		}

		void Axis_AxisChanged(object sender, EventArgs e)
		{
			this.OnAxesChanged();
		}

		internal event EventHandler CollectionChanged;

		void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, EventArgs.Empty);
			}
		}

		internal event EventHandler AxesChanged;

		void OnAxesChanged()
		{
			if (this.AxesChanged != null)
			{
				this.AxesChanged(this, EventArgs.Empty);
			}
		}

		public AxisGroup Clone()
		{
			return new AxisGroup(this.CategoryAxis, this.ValueAxis);
		}

		Axis categoryAxis;

		Axis valueAxis;
	}
}
