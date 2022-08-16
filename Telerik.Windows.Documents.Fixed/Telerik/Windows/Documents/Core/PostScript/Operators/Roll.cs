using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Roll : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			int lastAsInt2 = interpreter.Operands.GetLastAsInt();
			object[] array = new object[lastAsInt2];
			for (int i = 0; i < lastAsInt2; i++)
			{
				array[i] = interpreter.Operands.GetLast();
			}
			object[] array2 = Telerik.Windows.Documents.Core.PostScript.Operators.Roll.Rotate(array, (lastAsInt % lastAsInt2 + lastAsInt2) % lastAsInt2);
			foreach (object obj in array2)
			{
				interpreter.Operands.AddLast(obj);
			}
		}

		static object[] Rotate(object[] a, int j)
		{
			int num = a.Length;
			List<object> list = new List<object>(num);
			int num2 = (j - 1) % num;
			for (int i = num2; i >= 0; i--)
			{
				list.Add(a[i]);
			}
			int num3 = j % num;
			for (int k = num - 1; k >= num3; k--)
			{
				list.Add(a[k]);
			}
			return list.ToArray();
		}
	}
}
