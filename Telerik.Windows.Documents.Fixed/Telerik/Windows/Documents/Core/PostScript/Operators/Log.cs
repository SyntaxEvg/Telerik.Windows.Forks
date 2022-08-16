using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Log : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Log10(x);
		}
	}
}
