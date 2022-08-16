using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Def : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			string lastAs = interpreter.Operands.GetLastAs<string>();
			interpreter.CurrentDictionary[lastAs] = last;
			if (lastAs == "RD" || lastAs == "-|")
			{
				interpreter.RD = (PostScriptArray)last;
				return;
			}
			if (lastAs == "ND" || lastAs == "|-")
			{
				interpreter.ND = (PostScriptArray)last;
				return;
			}
			if (lastAs == "NP" || lastAs == "|")
			{
				interpreter.NP = (PostScriptArray)last;
			}
		}
	}
}
