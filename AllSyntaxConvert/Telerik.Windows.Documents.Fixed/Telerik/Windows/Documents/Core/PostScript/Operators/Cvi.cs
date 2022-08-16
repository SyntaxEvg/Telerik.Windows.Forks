using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Cvi : UnaryOperator<double, int>
	{
		protected override int Execute(double x)
		{
			return Convert.ToInt32(x);
		}
	}
}
