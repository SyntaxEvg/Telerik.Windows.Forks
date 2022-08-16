using System;
using Telerik.Windows.Documents.Core.PostScript.Encryption;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class EExec : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Operands.GetLast();
			interpreter.Reader.SkipWhiteSpaces();
			EExecEncryption eexecEncryption = new EExecEncryption(interpreter.Reader);
			interpreter.Reader.PushEncryption(eexecEncryption);
			eexecEncryption.Initialize();
		}
	}
}
