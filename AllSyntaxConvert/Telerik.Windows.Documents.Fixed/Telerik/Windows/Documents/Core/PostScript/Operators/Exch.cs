using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Exch : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			object last2 = interpreter.Operands.GetLast();
			interpreter.Operands.AddLast(last);
			interpreter.Operands.AddLast(last2);
		}
	}
}
