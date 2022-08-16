using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class BitShift : GenericBinaryOperator<int, int, int>
	{
		protected override int Execute(int x, int y)
		{
			if (y > 0)
			{
				return x << y;
			}
			return x >> -y;
		}
	}
}
