using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Dict : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			PostScriptDictionary obj = new PostScriptDictionary(lastAsInt);
			interpreter.Operands.AddLast(obj);
		}
	}
}
