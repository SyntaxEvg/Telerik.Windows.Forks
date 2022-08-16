using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class CallGSubr : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			interpreter.ExecuteGlobalSubr(interpreter.Operands.GetLastAsInt());
		}
	}
}
