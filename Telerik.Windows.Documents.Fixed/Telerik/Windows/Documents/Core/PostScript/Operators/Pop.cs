using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Pop : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.GetLast();
		}
	}
}
