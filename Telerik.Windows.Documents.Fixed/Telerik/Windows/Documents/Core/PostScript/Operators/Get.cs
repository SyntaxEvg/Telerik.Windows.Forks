using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;
using Telerik.Windows.Documents.Core.PostScript.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class Get : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			object last = interpreter.Operands.GetLast();
			object last2 = interpreter.Operands.GetLast();
			if (last2 is PostScriptArray)
			{
				PostScriptArray postScriptArray = (PostScriptArray)last2;
				int index;
				Helper.UnboxInteger(last, out index);
				interpreter.Operands.AddLast(postScriptArray[index]);
				return;
			}
			if (last2 is PostScriptDictionary)
			{
				PostScriptDictionary postScriptDictionary = (PostScriptDictionary)last2;
				interpreter.Operands.AddLast(postScriptDictionary[(string)last]);
				return;
			}
			if (last2 is PostScriptString)
			{
				PostScriptString postScriptString = (PostScriptString)last2;
				int index2;
				Helper.UnboxInteger(last, out index2);
				interpreter.Operands.AddLast(postScriptString[index2]);
				return;
			}
			Guard.ThrowNotSupportedException();
		}
	}
}
