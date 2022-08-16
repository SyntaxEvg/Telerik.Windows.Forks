using System;
using Telerik.Windows.Documents.Core.PostScript.Encryption;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class RD : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			interpreter.Reader.Read();
			int lastAsInt = interpreter.Operands.GetLastAsInt();
			CharStringEncryption charStringEncryption = new CharStringEncryption(interpreter.Reader);
			interpreter.Reader.PushEncryption(charStringEncryption);
			charStringEncryption.Initialize();
			interpreter.Operands.AddLast(lastAsInt - charStringEncryption.RandomBytesCount);
			interpreter.ExecuteProcedure(interpreter.RD);
			interpreter.Reader.PopEncryption();
		}
	}
}
