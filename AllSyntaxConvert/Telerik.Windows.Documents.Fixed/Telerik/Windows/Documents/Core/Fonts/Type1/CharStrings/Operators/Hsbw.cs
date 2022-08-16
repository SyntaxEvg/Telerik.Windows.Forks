using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class Hsbw : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			int lastAsInt = buildChar.Operands.GetLastAsInt();
			int lastAsInt2 = buildChar.Operands.GetLastAsInt();
			buildChar.Operands.Clear();
			buildChar.Width = new int?(lastAsInt);
			buildChar.CurrentPoint = new Point((double)lastAsInt2, 0.0);
		}
	}
}
