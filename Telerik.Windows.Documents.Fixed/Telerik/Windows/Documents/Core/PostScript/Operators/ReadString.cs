using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class ReadString : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptString lastAs = interpreter.Operands.GetLastAs<PostScriptString>();
			interpreter.Operands.GetLast();
			int num = 0;
			while (!interpreter.Reader.EndOfFile && num < lastAs.Capacity)
			{
				lastAs[num++] = (char)interpreter.Reader.Read();
			}
			interpreter.Operands.AddLast(lastAs);
			interpreter.Operands.AddLast(true);
		}
	}
}
