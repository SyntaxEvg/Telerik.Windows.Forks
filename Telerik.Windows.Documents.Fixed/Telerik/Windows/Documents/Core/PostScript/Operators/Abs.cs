using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Abs : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Abs(x);
		}
	}
}
