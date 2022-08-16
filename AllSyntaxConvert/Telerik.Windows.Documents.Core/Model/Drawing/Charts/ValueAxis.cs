using System;

namespace Telerik.Windows.Documents.Model.Drawing.Charts
{
	public class ValueAxis : Axis
	{
		public override AxisType AxisType
		{
			get
			{
				return AxisType.Value;
			}
		}
	}
}
