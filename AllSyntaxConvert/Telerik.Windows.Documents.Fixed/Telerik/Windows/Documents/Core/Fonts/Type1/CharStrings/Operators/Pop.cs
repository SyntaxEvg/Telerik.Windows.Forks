using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class Pop : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			buildChar.Operands.AddLast(buildChar.PostScriptStack.GetLast());
		}
	}
}
