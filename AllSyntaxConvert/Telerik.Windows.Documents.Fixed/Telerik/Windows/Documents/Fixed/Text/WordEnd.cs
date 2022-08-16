using System;

namespace Telerik.Windows.Documents.Fixed.Text
{
	static class WordEnd
	{
		public static bool IsWordEnd(char ch)
		{
			if (ch != '(')
			{
				switch (ch)
				{
				case '-':
				case '/':
					return true;
				case '.':
					break;
				default:
					if (ch == '\\')
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		public const char Dash = '-';

		public const char Slash = '/';

		public const char Backslash = '\\';

		public const char LeftParenthesis = '(';
	}
}
