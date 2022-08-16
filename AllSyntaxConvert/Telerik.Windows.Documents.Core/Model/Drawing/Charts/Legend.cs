using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class Legend
	{
		public LegendPosition Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (this.position != value)
				{
					this.position = value;
					this.OnLegendChanged();
				}
			}
		}

		public Legend Clone()
		{
			return new Legend
			{
				Position = this.Position
			};
		}

		internal event EventHandler LegendChanged;

		internal void OnLegendChanged()
		{
			if (this.LegendChanged != null)
			{
				this.LegendChanged(this, EventArgs.Empty);
			}
		}

		LegendPosition position;
	}
}
