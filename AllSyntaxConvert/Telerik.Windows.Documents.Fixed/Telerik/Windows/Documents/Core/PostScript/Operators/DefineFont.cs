using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Dictionaries;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.PostScript.Operators
{
	class DefineFont : Operator
	{
		public override void Execute(Interpreter interpreter)
		{
			PostScriptDictionary lastAs = interpreter.Operands.GetLastAs<PostScriptDictionary>();
			object last = interpreter.Operands.GetLast();
			string text = last as string;
			if (text == null)
			{
				PostScriptString postScriptString = last as PostScriptString;
				if (postScriptString == null)
				{
					throw new InvalidOperationException(string.Format("Cannot convert key from type: {0}", last.GetType()));
				}
				text = postScriptString.Value;
			}
			Type1Font type1Font = new Type1Font();
			type1Font.Load(lastAs);
			interpreter.Fonts[text] = type1Font;
			interpreter.Operands.AddLast(type1Font);
		}
	}
}
