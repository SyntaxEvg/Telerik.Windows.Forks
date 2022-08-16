using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class SetCurrentPoint : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			int lastAsInt = buildChar.Operands.GetLastAsInt();
			int lastAsInt2 = buildChar.Operands.GetLastAsInt();
			buildChar.Operands.Clear();
			buildChar.CurrentPoint = new Point((double)lastAsInt2, (double)lastAsInt);
		}
	}
}
