using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	abstract class BinaryOperator : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			object last2 = interpreter.Operands.GetLast();
			object obj = this.Execute(last2, last);
			interpreter.Operands.AddLast(obj);
		}

		protected abstract object Execute(object x, object y);
	}
}
