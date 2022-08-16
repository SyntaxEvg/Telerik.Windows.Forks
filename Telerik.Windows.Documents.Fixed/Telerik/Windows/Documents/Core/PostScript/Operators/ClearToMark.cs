using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class ClearToMark : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last;
			do
			{
				last = interpreter.Operands.GetLast();
			}
			while (last == null || !(last is Mark));
		}
	}
}
