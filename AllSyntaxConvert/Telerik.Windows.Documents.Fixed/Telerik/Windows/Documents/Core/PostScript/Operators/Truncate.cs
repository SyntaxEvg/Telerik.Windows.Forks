using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Truncate : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Telerik.Windows.Documents.Core.PostScript.Operators.Truncate.TruncateValue(x);
		}

		static double TruncateValue(double x)
		{
			return ((x < 0.0) ? (1.0) : 1.0) * Math.Floor(Math.Abs(x));
		}
	}
}
