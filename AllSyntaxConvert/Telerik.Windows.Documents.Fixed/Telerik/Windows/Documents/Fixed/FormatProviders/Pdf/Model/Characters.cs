using System;
using Telerik.Windows.Documents.Fixed.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model
{
	static class Characters
	{
		public static bool IsHexChar(byte b)
		{
			return (48 <= b && b <= 57) || (65 <= b && b <= 70) || (97 <= b && b <= 102);
		}

		public static bool IsOctalChar(byte b)
		{
			return 48 <= b && b <= 55;
		}

		public static bool IsHexStringOrDictionaryStart(byte b)
		{
			return b == 60;
		}

		public static bool IsHexStringOrDictionaryEnd(byte b)
		{
			return b == 62;
		}

		public static bool IsValidNameCharacter(byte b)
		{
			return !Characters.IsDelimiter(b) && !Characters.IsWhiteSpace(b);
		}

		public static bool IsDelimiter(byte b)
		{
			if (b <= 41)
			{
				if (b != 10 && b != 13)
				{
					switch (b)
					{
					case 37:
					case 40:
					case 41:
						break;
					case 38:
					case 39:
						return false;
					default:
						return false;
					}
				}
			}
			else if (b <= 62)
			{
				if (b != 47)
				{
					switch (b)
					{
					case 60:
					case 62:
						break;
					case 61:
						return false;
					default:
						return false;
					}
				}
			}
			else
			{
				switch (b)
				{
				case 91:
				case 93:
					break;
				case 92:
					return false;
				default:
					switch (b)
					{
					case 123:
					case 125:
						break;
					case 124:
						return false;
					default:
						return false;
					}
					break;
				}
			}
			return true;
		}

		public static bool IsLiteralStringStart(byte b)
		{
			return b == 40;
		}

		public static bool IsLiteralStringEnd(byte b)
		{
			return b == 41;
		}

		public static bool IsArrayStart(byte b)
		{
			return b == 91;
		}

		public static bool IsArrayEnd(byte b)
		{
			return b == 93;
		}

		public static bool IsNameStart(byte b)
		{
			return b == 47;
		}

		public static bool IsNumberCharacter(byte b)
		{
			return char.IsDigit((char)b) || PdfHelper.CultureInfo.IsDecimalSeparator((char)b);
		}

		public static bool IsCharacter(byte b)
		{
			return (97 <= b && b <= 122) || (65 <= b && b <= 90);
		}

		public static bool IsKeywordCharacter(byte b)
		{
			return Characters.IsCharacter(b) || b == 42;
		}

		public static bool IsKeywordStartCharacter(byte b)
		{
			return Characters.IsCharacter(b) || b == 39 || b == 34;
		}

		public static bool IsWhiteSpace(byte b)
		{
			return char.IsWhiteSpace((char)b);
		}

		public static bool IsCommentStart(byte b)
		{
			return b == 37;
		}

		public static bool IsLineFeed(byte b)
		{
			return b == 10;
		}

		public static bool IsCarriageReturn(byte b)
		{
			return b == 13;
		}

		public static bool IsSign(byte b)
		{
			return 43 == b || 45 == b;
		}

		public const char HexStringAndDictionaryStart = '<';

		public const char HexStringAndDictionaryEnd = '>';

		public const char StringEscapeCharacter = '\\';

		public const char LiteralStringStart = '(';

		public const char LiteralStringEnd = ')';

		public const char LineFeed = '\n';

		public const char CarriageReturn = '\r';

		public const char ArrayStart = '[';

		public const char ArrayEnd = ']';

		public const char ProcedureStart = '{';

		public const char ProcedureEnd = '}';

		public const char NameStart = '/';

		public const char Comment = '%';

		public const char NumberCharacter = '#';

		public const byte Zero = 48;

		public const byte Seven = 55;
	}
}
