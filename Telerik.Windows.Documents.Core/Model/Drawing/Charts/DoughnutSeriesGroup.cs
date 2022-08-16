using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class DoughnutSeriesGroup : PieSeriesGroup
	{
		public int HoleSizePercent
		{
			get
			{
				return this.holeSizePercent;
			}
			set
			{
				if (this.holeSizePercent != value)
				{
					if (value < 0 || 90 < value)
					{
						throw new ArgumentException("The HoleSizePercent must be between 0 and 90.");
					}
					this.holeSizePercent = value;
					this.OnSeriesGroupChanged();
				}
			}
		}

		int holeSizePercent;
	}
}
