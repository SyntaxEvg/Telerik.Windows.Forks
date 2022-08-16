using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class String : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			interpreter.Operands.AddLast(new PostScriptString(lastAsInt));
		}
	}
}
