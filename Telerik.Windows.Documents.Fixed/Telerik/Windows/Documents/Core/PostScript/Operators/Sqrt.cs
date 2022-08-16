using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Sqrt : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Sqrt(x);
		}
	}
}
