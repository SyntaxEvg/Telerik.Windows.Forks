using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class Div : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			double lastAsReal = buildChar.Operands.GetLastAsReal();
			double num = (double)buildChar.Operands.GetFirstAsInt();
			buildChar.Operands.AddLast(num / lastAsReal);
		}
	}
}
