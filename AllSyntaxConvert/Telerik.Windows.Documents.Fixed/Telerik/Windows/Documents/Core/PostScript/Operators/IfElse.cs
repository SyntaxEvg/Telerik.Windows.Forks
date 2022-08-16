using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class IfElse : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptArray lastAs = interpreter.Operands.GetLastAs<PostScriptArray>();
			PostScriptArray lastAs2 = interpreter.Operands.GetLastAs<PostScriptArray>();
			bool lastAs3 = interpreter.Operands.GetLastAs<bool>();
			if (lastAs3)
			{
				interpreter.ExecuteProcedure(lastAs2);
				return;
			}
			interpreter.ExecuteProcedure(lastAs);
		}
	}
}
