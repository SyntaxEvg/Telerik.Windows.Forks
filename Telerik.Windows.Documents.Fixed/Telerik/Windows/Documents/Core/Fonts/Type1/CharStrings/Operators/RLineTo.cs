using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class RLineTo : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			while (interpreter.Operands.Count / 2 > 0)
			{
				int firstAsInt = interpreter.Operands.GetFirstAsInt();
				int firstAsInt2 = interpreter.Operands.GetFirstAsInt();
				Operator.LineTo(interpreter, firstAsInt, firstAsInt2);
			}
			interpreter.Operands.Clear();
		}
	}
}
