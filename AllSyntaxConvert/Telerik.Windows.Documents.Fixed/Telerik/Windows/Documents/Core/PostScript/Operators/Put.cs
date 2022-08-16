using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Put : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			object last2 = interpreter.Operands.GetLast();
			object last3 = interpreter.Operands.GetLast();
			if (last3 is PostScriptArray)
			{
				PostScriptArray postScriptArray = (PostScriptArray)last3;
				int index;
				Helper.UnboxInteger(last2, out index);
				postScriptArray[index] = last;
				return;
			}
			if (last3 is PostScriptDictionary)
			{
				PostScriptDictionary postScriptDictionary = (PostScriptDictionary)last3;
				postScriptDictionary[(string)last2] = last;
				return;
			}
			if (last3 is PostScriptString)
			{
				PostScriptString postScriptString = (PostScriptString)last3;
				int index2;
				Helper.UnboxInteger(last2, out index2);
				int num;
				Helper.UnboxInteger(last, out num);
				postScriptString[index2] = (char)num;
				return;
			}
			Guard.ThrowNotSupportedException();
		}
	}
}
