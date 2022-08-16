using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Begin : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptDictionary lastAs = interpreter.Operands.GetLastAs<PostScriptDictionary>();
			interpreter.DictionaryStack.Push(lastAs);
		}
	}
}
