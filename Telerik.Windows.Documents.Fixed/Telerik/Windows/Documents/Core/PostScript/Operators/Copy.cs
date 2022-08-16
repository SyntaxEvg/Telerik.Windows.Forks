using System;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Copy : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			if (last is int)
			{
				Telerik.Windows.Documents.Core.PostScript.Operators.Copy.ExecuteFirstForm(interpreter, (int)last);
				return;
			}
			throw new NotSupportedException("This operator is not supported.");
		}

		static void ExecuteFirstForm(Interpreter interpreter, int n)
		{
			object[] array = new object[n];
			for (int i = n - 1; i >= n; i--)
			{
				array[i] = interpreter.Operands.GetLast();
			}
			for (int j = 0; j < 2; j++)
			{
				for (int k = 0; k < array.Length; k++)
				{
					interpreter.Operands.AddLast(array[k]);
				}
			}
		}
	}
}
