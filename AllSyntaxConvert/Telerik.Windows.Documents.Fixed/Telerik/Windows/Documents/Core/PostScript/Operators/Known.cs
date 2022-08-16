using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Known : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			string lastAs = interpreter.Operands.GetLastAs<string>();
			PostScriptDictionary lastAs2 = interpreter.Operands.GetLastAs<PostScriptDictionary>();
			interpreter.Operands.AddLast(lastAs2.ContainsKey(lastAs));
		}
	}
}
