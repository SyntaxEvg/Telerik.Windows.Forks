using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Round : UnaryOperator<double, double>
	{
		protected override double Execute(double x)
		{
			return Math.Round(x);
		}
	}
}
