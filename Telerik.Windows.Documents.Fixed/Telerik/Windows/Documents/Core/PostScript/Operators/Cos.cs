using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Cos : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Cos(x);
		}
	}
}
