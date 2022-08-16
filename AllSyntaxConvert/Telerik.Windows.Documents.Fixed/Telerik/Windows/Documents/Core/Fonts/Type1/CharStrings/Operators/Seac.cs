using System;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.CharStrings.Operators
{
	class Seac : Operator
	{
		public override void Execute(BuildChar buildChar)
		{
			int lastAsInt = buildChar.Operands.GetLastAsInt();
			int lastAsInt2 = buildChar.Operands.GetLastAsInt();
			int lastAsInt3 = buildChar.Operands.GetLastAsInt();
			int lastAsInt4 = buildChar.Operands.GetLastAsInt();
			buildChar.Operands.GetLastAsInt();
			string accentedChar = PredefinedEncodings.StandardEncoding[lastAsInt];
			string baseChar = PredefinedEncodings.StandardEncoding[lastAsInt2];
			buildChar.CombineChars(accentedChar, baseChar, lastAsInt4, lastAsInt3);
			buildChar.Operands.Clear();
		}
	}
}
