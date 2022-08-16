using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class RMoveTo : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			Operator.ReadWidth(interpreter, 2);
			int firstAsInt = interpreter.Operands.GetFirstAsInt();
			int firstAsInt2 = interpreter.Operands.GetFirstAsInt();
			Operator.MoveTo(interpreter, firstAsInt, firstAsInt2);
			interpreter.Operands.Clear();
		}
	}
}
