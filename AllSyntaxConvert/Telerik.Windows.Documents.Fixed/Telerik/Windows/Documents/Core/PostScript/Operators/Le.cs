using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Le : GenericBinaryOperator<double, double, bool>
	{
		protected override bool Execute(double x, double y)
		{
			return x <= y;
		}
	}
}
