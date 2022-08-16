using System;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	static class Characters
	{
		public static bool IsWhiteSpace(int b)
		{
			char c = (char)b;
			return char.IsWhiteSpace(c);
		}

		public static bool IsOctalChar(int b)
		{
			char c = (char)b;
			return '0' <= c && c <= '7';
		}

		public static bool IsHexChar(int b)
		{
			char c = (char)b;
			return ('0' <= c && c <= '9') || ('A' <= c && c <= 'F') || ('a' <= c && c <= 'f');
		}

		public static bool IsDelimiter(int b)
		{
			char c = (char)b;
			if (char.IsWhiteSpace(c))
			{
				return true;
			}
			char c2 = c;
			if (c2 <= '/')
			{
				if (c2 != '\0')
				{
					switch (c2)
					{
					case '%':
					case '(':
					case ')':
						break;
					case '&':
					case '\'':
						return false;
					default:
						if (c2 != '/')
						{
							return false;
						}
						break;
					}
				}
			}
			else
			{
				switch (c2)
				{
				case '<':
				case '>':
					break;
				case '=':
					return false;
				default:
					switch (c2)
					{
					case '[':
					case ']':
						break;
					case '\\':
						return false;
					default:
						switch (c2)
						{
						case '{':
						case '}':
							break;
						case '|':
							return false;
						default:
							return false;
						}
						break;
					}
					break;
				}
			}
			return true;
		}

		public static bool IsLetter(int b)
		{
			char c = (char)b;
			return char.IsLetter(c);
		}

		public static bool IsValidNumberChar(IPostScriptReader reader)
		{
			char c = (char)reader.Peek(0);
			if (c == '+' || c == '-')
			{
				char ch = (char)reader.Peek(1);
				return Characters.IsDigitOrDecimalPoint(ch);
			}
			return Characters.IsDigitOrDecimalPoint(c);
		}

		public static bool IsValidHexCharacter(int ch)
		{
			return 80 <= ch && ch <= 128;
		}

		static bool IsDigitOrDecimalPoint(char ch)
		{
			return char.IsDigit(ch) || ch == '.';
		}

		public const char NotDef = '\0';

		public const char Null = '\0';

		public const byte SingleByteSpace = 32;

		public const char Ap = '\'';

		public const char Qu = '"';

		public const char Space = ' ';

		public const char Tab = '\t';

		public const char LF = '\n';

		public const char FF = '\f';

		public const char CR = '\r';

		public const char DecimalPoint = '.';

		public const char LiteralStringStart = '(';

		public const char LiteralStringEnd = ')';

		public const char HexadecimalStringStart = '<';

		public const char HexadecimalStringEnd = '>';

		public const char Comment = '%';

		public const char Name = '/';

		public const char ArrayStart = '[';

		public const char ArrayEnd = ']';

		public const char ExecuteableArrayStart = '{';

		public const char ExecuteableArrayEnd = '}';

		public const char StringEscapeCharacter = '\\';

		public const char Minus = '-';

		public const char Plus = '+';

		public const char VerticalBar = '|';

		public static readonly char[] FontFamilyDelimiters = new char[] { ',', '-' };
	}
}
