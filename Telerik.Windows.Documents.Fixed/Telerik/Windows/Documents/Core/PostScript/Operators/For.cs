using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class For : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptArray lastAs = interpreter.Operands.GetLastAs<PostScriptArray>();
			double lastAsReal = interpreter.Operands.GetLastAsReal();
			double lastAsReal2 = interpreter.Operands.GetLastAsReal();
			double lastAsReal3 = interpreter.Operands.GetLastAsReal();
			double num = lastAsReal3;
			while ((lastAsReal > 0.0) ? (num < lastAsReal) : (num > lastAsReal))
			{
				interpreter.Operands.AddLast(num);
				interpreter.ExecuteProcedure(lastAs);
				num += lastAsReal2;
			}
		}
	}
}
