using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class SystemDict : Operator
	{
		public override void Execute(Interpreter interpteret)
		{
			interpteret.Operands.AddLast(interpteret.SystemDict);
		}
	}
}
