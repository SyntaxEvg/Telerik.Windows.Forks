using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Ln : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Log(x);
		}
	}
}
