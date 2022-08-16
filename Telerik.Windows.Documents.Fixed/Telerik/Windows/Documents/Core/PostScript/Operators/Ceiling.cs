using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Ceiling : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Ceiling(x);
		}
	}
}
