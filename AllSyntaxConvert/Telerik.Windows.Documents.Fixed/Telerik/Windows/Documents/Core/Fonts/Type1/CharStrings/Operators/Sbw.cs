using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class Sbw : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			buildChar.Operands.GetLastAsInt();
			int lastAsInt = buildChar.Operands.GetLastAsInt();
			int lastAsInt2 = buildChar.Operands.GetLastAsInt();
			int lastAsInt3 = buildChar.Operands.GetLastAsInt();
			buildChar.Operands.Clear();
			buildChar.Width = new int?(lastAsInt);
			buildChar.CurrentPoint = new Point((double)lastAsInt3, (double)lastAsInt2);
		}
	}
}
