using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class True : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(true);
		}
	}
}
