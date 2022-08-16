using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Array : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			PostScriptArray obj = new PostScriptArray(lastAsInt);
			interpreter.Operands.AddLast(obj);
		}
	}
}
