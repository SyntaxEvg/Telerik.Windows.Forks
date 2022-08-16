using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class End : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.DictionaryStack.Pop();
		}
	}
}
