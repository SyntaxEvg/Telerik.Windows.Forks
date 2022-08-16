using System;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class HintOperator : Operator
	{
		static void ReadWidth(BuildChar interpreter)
		{
			if (interpreter.Width != null)
			{
				return;
			}
			if (interpreter.Operands.Count % 2 == 1)
			{
				interpreter.Width = new int?(interpreter.Operands.GetFirstAsInt());
				return;
			}
			interpreter.Width = new int?(0);
		}

		public override void Execute(BuildChar interpreter)
		{
			HintOperator.ReadWidth(interpreter);
			interpreter.Operands.Clear();
		}

		internal void Execute(BuildChar interpreter, out int count)
		{
			count = interpreter.Operands.Count / 2;
			this.Execute(interpreter);
		}
	}
}
