using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Sin : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Sin(x);
		}
	}
}
