using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class NP : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.ExecuteProcedure(interpreter.NP);
		}
	}
}
