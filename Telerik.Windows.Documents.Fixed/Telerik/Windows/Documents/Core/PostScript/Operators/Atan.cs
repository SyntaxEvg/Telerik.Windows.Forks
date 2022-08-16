using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Atan : GenericBinaryOperator<double, double, double>
	{
		protected override double Execute(double x, double y)
		{
			return Math.Atan2(x, y);
		}
	}
}
