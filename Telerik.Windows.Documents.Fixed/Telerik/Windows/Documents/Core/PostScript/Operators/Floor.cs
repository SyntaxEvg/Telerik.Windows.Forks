using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Floor : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Floor(x);
		}
	}
}
