using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class CallOtherSubr : Operator
	{
		public override void Execute(BuildChar interpreter)
		{
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			int lastAsInt2 = interpreter.Operands.GetLastAsInt();
			int num = lastAsInt;
			if (num != 0)
			{
				if (num != 3)
				{
					for (int i = 0; i < lastAsInt2; i++)
					{
						interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
					}
				}
				else
				{
					interpreter.PostScriptStack.AddLast(3);
				}
			}
			else
			{
				interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
				interpreter.PostScriptStack.AddLast(interpreter.Operands.GetLast());
			}
			interpreter.Operands.Clear();
		}
	}
}
