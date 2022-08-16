using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class CurrentFile : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(null);
		}
	}
}
