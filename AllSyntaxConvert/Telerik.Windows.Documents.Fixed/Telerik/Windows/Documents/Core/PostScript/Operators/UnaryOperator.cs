using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	abstract class UnaryOperator<T, TResult> : Operator where T : struct
	{
		public override void Execute(Interpreter interpreter)
		{
			T lastUnboxedAs = interpreter.Operands.GetLastUnboxedAs<T>();
			TResult tresult = this.Execute(lastUnboxedAs);
			interpreter.Operands.AddLast(tresult);
		}

		protected abstract TResult Execute(T x);
	}
}
