using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class FontDirectory : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.AddLast(interpreter.SystemDict["FontDirectory"]);
		}
	}
}
