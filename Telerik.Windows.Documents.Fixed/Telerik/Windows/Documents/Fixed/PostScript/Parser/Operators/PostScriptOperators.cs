using System;

namespace Telerik.Windows.Documents.Fixed.PostScript.Parser.Operators
{
	static class PostScriptOperators
	{
		public static BeginBFChar BeginBFChar
		{
			get
			{
				return PostScriptOperators.beginBFChar;
			}
		}

		public static EndBFChar EndBFChar
		{
			get
			{
				return PostScriptOperators.endBFChar;
			}
		}

		static readonly BeginBFChar beginBFChar = new BeginBFChar();

		static readonly EndBFChar endBFChar = new EndBFChar();
	}
}
