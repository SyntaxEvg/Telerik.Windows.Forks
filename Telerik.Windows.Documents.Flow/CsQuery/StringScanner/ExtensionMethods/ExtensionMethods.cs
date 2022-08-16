using System;

namespace CsQuery.StringScanner.ExtensionMethods
{
	static class ExtensionMethods
	{
		public static string SubstringBetween(this string text, int startIndex, int endIndex)
		{
			if (endIndex > text.Length || endIndex < 0)
			{
				return "";
			}
			return text.Substring(startIndex, endIndex - startIndex);
		}

		public static string SubstringBetween(this char[] text, int startIndex, int endIndex)
		{
			string text2 = "";
			for (int i = startIndex; i < endIndex; i++)
			{
				text2 += text[i];
			}
			return text2;
		}
	}
}
