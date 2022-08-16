using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class False : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(false);
		}
	}
}
