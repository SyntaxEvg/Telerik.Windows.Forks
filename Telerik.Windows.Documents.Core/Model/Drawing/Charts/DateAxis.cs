using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class DateAxis : Axis
	{
		public override AxisType AxisType
		{
			get
			{
				return AxisType.Date;
			}
		}
	}
}
