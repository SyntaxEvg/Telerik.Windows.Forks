using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class CloseFile : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.GetLast();
			interpreter.Reader.PopEncryption();
		}
	}
}
