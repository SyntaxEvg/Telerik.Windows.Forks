using System;
using HtmlParserSharp.Common;

namespace HtmlParserSharp.Core
{
	sealed class Portability
	{
		[Local]
		public static string NewLocalNameFromBuffer(char[] buf, int offset, int length)
		{
			return string.Intern(new string(buf, offset, length));
		}

		public static bool LocalEqualsBuffer([Local] string local, char[] buf, int offset, int length)
		{
			if (local.Length != length)
			{
				return false;
			}
			for (int i = 0; i < length; i++)
			{
				if (local[i] != buf[offset + i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool LowerCaseLiteralIsPrefixOfIgnoreAsciiCaseString(string lowerCaseLiteral, string str)
		{
			if (str == null)
			{
				return false;
			}
			if (lowerCaseLiteral.Length > str.Length)
			{
				return false;
			}
			for (int i = 0; i < lowerCaseLiteral.Length; i++)
			{
				char c = lowerCaseLiteral[i];
				char c2 = str[i];
				if (c2 >= 'A' && c2 <= 'Z')
				{
					c2 += ' ';
				}
				if (c != c2)
				{
					return false;
				}
			}
			return true;
		}

		public static bool LowerCaseLiteralEqualsIgnoreAsciiCaseString(string lowerCaseLiteral, string str)
		{
			if (str == null)
			{
				return false;
			}
			if (lowerCaseLiteral.Length != str.Length)
			{
				return false;
			}
			for (int i = 0; i < lowerCaseLiteral.Length; i++)
			{
				char c = lowerCaseLiteral[i];
				char c2 = str[i];
				if (c2 >= 'A' && c2 <= 'Z')
				{
					c2 += ' ';
				}
				if (c != c2)
				{
					return false;
				}
			}
			return true;
		}
	}
}
