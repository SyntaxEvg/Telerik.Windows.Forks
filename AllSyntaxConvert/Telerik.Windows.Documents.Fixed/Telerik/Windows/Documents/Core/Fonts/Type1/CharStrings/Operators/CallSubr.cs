using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class CallSubr : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			interpreter.ExecuteSubr(interpreter.Operands.GetLastAsInt());
		}
	}
}
