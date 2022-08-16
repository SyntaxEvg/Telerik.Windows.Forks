using System;
using System.Linq;
using CsQuery.StringScanner.Implementation;

namespace CsQuery.StringScanner
{
	static class CharacterData
	{
		static CharacterData()
		{
			CharacterData.characterFlags = new uint[65536];
			CharacterData.setBit(" \t\n\f\r\u00a0À", 1U);
			CharacterData.setBit("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 2U);
			CharacterData.setBit("0123456789", 4U);
			CharacterData.setBit("0123456789.-+", 8U);
			CharacterData.setBit("abcdefghijklmnopqrstuvwxyz", 16U);
			CharacterData.setBit("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 32U);
			CharacterData.setBit("\"'", 256U);
			CharacterData.setBit("!+-*/%<>^=~", 64U);
			CharacterData.setBit("()[]{}<>`\u00b4“”«»", 128U);
			CharacterData.setBit("\\", 512U);
			CharacterData.setBit(", |", 1024U);
			CharacterData.setBit(" \t\n\f\r", 1048576U);
			CharacterData.setBit("0123456789abcdefABCDEF", 8388608U);
			CharacterData.SetHtmlTagNameStart(65536U);
			CharacterData.SetHtmlTagNameExceptStart(131072U);
			CharacterData.SetHtmlTagNameStart(4096U);
			CharacterData.SetHtmlTagSelectorExceptStart(8192U);
			CharacterData.SetHtmlAttributeName(262144U);
			CharacterData.setBit(" \t\n\f\r/>", 16384U);
			CharacterData.setBit("<>/", 32768U);
			CharacterData.setBit("<>&", 2097152U);
			CharacterData.setBit(" \t\n\f\r>", 4194304U);
			CharacterData.SetAlphaISO10646(2048U);
			CharacterData.SetSelectorTerminator(524288U);
		}

		public static ICharacterInfo CreateCharacterInfo()
		{
			return new CharacterInfo();
		}

		public static ICharacterInfo CreateCharacterInfo(char character)
		{
			return new CharacterInfo(character);
		}

		public static IStringInfo CreateStringInfo()
		{
			return new StringInfo();
		}

		public static IStringInfo CreateStringInfo(string text)
		{
			return new StringInfo(text);
		}

		public static bool IsType(char character, CharacterType type)
		{
			return (CharacterData.characterFlags[(int)character] & (uint)type) > 0U;
		}

		public static CharacterType GetType(char character)
		{
			return (CharacterType)CharacterData.characterFlags[(int)character];
		}

		public static char Closer(char character)
		{
			char c = CharacterData.CloserImpl(character);
			if (c == '\0')
			{
				throw new InvalidOperationException("The character '" + character + "' is not a known opening bound.");
			}
			return c;
		}

		public static char MatchingBound(char character)
		{
			if (character <= ']')
			{
				if (character == ')')
				{
					return '(';
				}
				if (character == '>')
				{
					return '<';
				}
				if (character == ']')
				{
					return '[';
				}
			}
			else if (character <= '\u00b4')
			{
				if (character == '}')
				{
					return '{';
				}
				if (character == '\u00b4')
				{
					return '`';
				}
			}
			else
			{
				if (character == '»')
				{
					return '«';
				}
				if (character == '”')
				{
					return '“';
				}
			}
			char c = CharacterData.CloserImpl(character);
			if (c == '\0')
			{
				throw new InvalidOperationException("The character '" + character + "' is not a bound.");
			}
			return c;
		}

		static void SetAlphaISO10646(uint hsb)
		{
			CharacterData.setBit("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", hsb);
			CharacterData.setBit('-', hsb);
			CharacterData.setBit('_', hsb);
			CharacterData.SetRange(hsb, 161, ushort.MaxValue);
		}

		static void SetSelectorTerminator(uint hsb)
		{
			CharacterData.setBit(" \t\n\f\r\u00a0À", hsb);
			CharacterData.setBit(",:[>~+.#", hsb);
		}

		static void SetHtmlAttributeName(uint hsb)
		{
			CharacterData.SetAlphaISO10646(hsb);
			CharacterData.setBit("0123456789.-+", hsb);
			CharacterData.setBit("_:.-", hsb);
		}

		static void SetHtmlTagSelectorStart(uint hsb)
		{
			CharacterData.setBit("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", hsb);
			CharacterData.setBit("_", hsb);
			CharacterData.SetRange(hsb, 192, 214);
			CharacterData.SetRange(hsb, 216, 246);
			CharacterData.SetRange(hsb, 248, 767);
			CharacterData.SetRange(hsb, 880, 893);
			CharacterData.SetRange(hsb, 895, 8191);
			CharacterData.SetRange(hsb, 8204, 8205);
			CharacterData.SetRange(hsb, 8304, 8591);
			CharacterData.SetRange(hsb, 11264, 12271);
			CharacterData.SetRange(hsb, 12289, 55295);
			CharacterData.SetRange(hsb, 63744, 64975);
			CharacterData.SetRange(hsb, 65008, 65533);
		}

		static void SetHtmlTagSelectorExceptStart(uint hsb)
		{
			CharacterData.SetHtmlTagSelectorStart(hsb);
			CharacterData.setBit("0123456789", hsb);
			CharacterData.setBit("-", hsb);
			CharacterData.setBit('·', hsb);
			CharacterData.SetRange(hsb, 768, 879);
			CharacterData.SetRange(hsb, 8255, 8256);
		}

		static void SetHtmlTagNameStart(uint hsb)
		{
			CharacterData.SetHtmlTagSelectorStart(hsb);
			CharacterData.setBit(":", hsb);
		}

		static void SetHtmlTagNameExceptStart(uint hsb)
		{
			CharacterData.SetHtmlTagSelectorExceptStart(hsb);
			CharacterData.setBit(":", hsb);
			CharacterData.setBit(".", hsb);
		}

		static void SetRange(uint flag, ushort start, ushort end)
		{
			for (int i = (int)start; i <= (int)end; i++)
			{
				CharacterData.setBit((char)i, flag);
			}
		}

		static void setBit(string forCharacters, uint bit)
		{
			for (int i = 0; i < forCharacters.Length; i++)
			{
				CharacterData.setBit(forCharacters[i], bit);
			}
		}

		static void setBit(char character, uint bit)
		{
			CharacterData.characterFlags[(int)character] |= bit;
		}

		static char CloserImpl(char character)
		{
			if (character <= '[')
			{
				if (character <= '(')
				{
					if (character == '"')
					{
						return '"';
					}
					switch (character)
					{
					case '\'':
						return '\'';
					case '(':
						return ')';
					}
				}
				else
				{
					if (character == '<')
					{
						return '>';
					}
					if (character == '[')
					{
						return ']';
					}
				}
			}
			else if (character <= '{')
			{
				if (character == '`')
				{
					return '\u00b4';
				}
				if (character == '{')
				{
					return '}';
				}
			}
			else
			{
				if (character == '«')
				{
					return '»';
				}
				if (character == '»')
				{
					return '«';
				}
				if (character == '“')
				{
					return '”';
				}
			}
			return '\0';
		}

		const string charsHtmlSpace = " \t\n\f\r";

		const string charsWhitespace = " \t\n\f\r\u00a0À";

		const string charsNumeric = "0123456789";

		const string charsHex = "0123456789abcdefABCDEF";

		const string charsNumericExtended = "0123456789.-+";

		const string charsLower = "abcdefghijklmnopqrstuvwxyz";

		const string charsUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		const string charsAlpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		const string charsQuote = "\"'";

		const string charsOperator = "!+-*/%<>^=~";

		const string charsEnclosing = "()[]{}<>`\u00b4“”«»";

		const string charsEscape = "\\";

		const string charsSeparators = ", |";

		const string charsHtmlTagAny = "<>/";

		const string charsHtmlMustBeEncoded = "<>&";

		static uint[] characterFlags;

		public static readonly char[] charsHtmlSpaceArray = " \t\n\f\r".ToArray<char>();
	}
}
