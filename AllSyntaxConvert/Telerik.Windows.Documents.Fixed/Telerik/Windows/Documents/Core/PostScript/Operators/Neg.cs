using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Neg : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return -x;
		}
	}
}
