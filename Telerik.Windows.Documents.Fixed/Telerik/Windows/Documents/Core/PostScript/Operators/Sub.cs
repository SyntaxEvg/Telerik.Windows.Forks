using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Sub : GenericBinaryOperator<double, double, double>
	{
		protected override double Execute(double x, double y)
		{
			return x - y;
		}
	}
}
