using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class And : GenericBinaryOperator<int, int, int>
	{
		protected override int Execute(int x, int y)
		{
			return x & y;
		}
	}
}
