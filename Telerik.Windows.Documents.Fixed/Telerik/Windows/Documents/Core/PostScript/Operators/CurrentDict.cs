using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class CurrentDict : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(interpreter.CurrentDictionary);
		}
	}
}
