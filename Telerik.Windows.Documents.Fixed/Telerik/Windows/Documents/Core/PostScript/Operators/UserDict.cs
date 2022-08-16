using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class UserDict : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			Guard.ThrowExceptionIfNull<Interpreter>(interpreter, "interpreter");
			interpreter.Operands.AddLast(interpreter.UserDict);
		}
	}
}
