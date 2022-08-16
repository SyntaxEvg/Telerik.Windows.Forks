using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Mark : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(this);
		}
	}
}
