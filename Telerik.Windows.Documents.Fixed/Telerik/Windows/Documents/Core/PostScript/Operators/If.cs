using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class If : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptArray lastAs = interpreter.Operands.GetLastAs<PostScriptArray>();
			bool lastAs2 = interpreter.Operands.GetLastAs<bool>();
			if (lastAs2)
			{
				interpreter.ExecuteProcedure(lastAs);
			}
		}
	}
}
