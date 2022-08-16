using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	abstract class GenericBinaryOperator<T1, T2, TResult> : Operator where T1 : struct where T2 : struct
	{
		public override void Execute(Interpreter interpreter)
		{
			T2 lastUnboxedAs = interpreter.Operands.GetLastUnboxedAs<T2>();
			T1 lastUnboxedAs2 = interpreter.Operands.GetLastUnboxedAs<T1>();
			TResult tresult = this.Execute(lastUnboxedAs2, lastUnboxedAs);
			interpreter.Operands.AddLast(tresult);
		}

		protected abstract TResult Execute(T1 x, T2 y);
	}
}
