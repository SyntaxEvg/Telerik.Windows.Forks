using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Eq : BinaryOperator
	{
		protected override object Execute(object x, object y)
		{
			return x.Equals(y);
		}
	}
}
