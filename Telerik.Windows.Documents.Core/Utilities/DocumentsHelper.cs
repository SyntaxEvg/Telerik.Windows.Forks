using System;
using System.Linq;

namespace Telerik.Windows.Documents.Utilities
{
	static class DocumentsHelper
	{
		public static bool IsLineBreak(char ch)
		{
			return ch == '\n';
		}

		public static bool IsTab(char ch)
		{
			return ch == '\t';
		}

		public static bool IsWhiteSpace(char ch)
		{
			return ch == ' ';
		}

		public static string EscapeWhiteSpaces(string text)
		{
			if (char.IsWhiteSpace(text.Last<char>()))
			{
				return text + '\u200d';
			}
			return text;
		}

		public const char SpaceSymbol = ' ';

		public const char TabSymbol = '\t';

		public const char NewLine = '\n';

		public const char ZeroWidthSymbol = '\u200d';

		public const char LineHeightMeasureSymbol = 'X';
	}
}
