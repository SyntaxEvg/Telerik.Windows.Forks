using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Exp : GenericBinaryOperator<double, double, double>
	{
		protected override double Execute(double x, double y)
		{
			return Math.Pow(x, y);
		}
	}
}
