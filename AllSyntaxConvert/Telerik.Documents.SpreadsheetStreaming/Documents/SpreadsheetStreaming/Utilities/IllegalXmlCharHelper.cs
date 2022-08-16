using System;
using System.Text;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class IllegalXmlCharHelper
	{
		public static string RemoveInvalidCharacters(string text)
		{
			StringBuilder stringBuilder = null;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				bool flag = (c >= ' ' && c <= '\ufffd') || c == '\t' || c == '\n' || c == '\r';
				if (!flag && stringBuilder == null)
				{
					stringBuilder = new StringBuilder(text.Substring(0, i));
				}
				if (stringBuilder != null && flag)
				{
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder == null)
			{
				return text;
			}
			return stringBuilder.ToString();
		}
	}
}
