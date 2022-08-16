using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class HMoveTo : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			Operator.ReadWidth(interpreter, 1);
			int firstAsInt = interpreter.Operands.GetFirstAsInt();
			Operator.MoveTo(interpreter, firstAsInt, 0);
			interpreter.Operands.Clear();
		}
	}
}
