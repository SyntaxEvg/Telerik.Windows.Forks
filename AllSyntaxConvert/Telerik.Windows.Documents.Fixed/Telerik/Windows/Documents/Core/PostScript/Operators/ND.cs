using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class ND : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.ExecuteProcedure(interpreter.ND);
		}
	}
}
