using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Fixed.Search
{
	static class WordSeparators
	{
		static WordSeparators()
		{
			WordSeparators.AddDefaultWordSeparators();
		}

		static void AddDefaultWordSeparators()
		{
			foreach (char separator in WordSeparators.defaultSeparators)
			{
				WordSeparators.Add(separator);
			}
		}

		internal static bool IsWordSeparator(char symbol)
		{
			return WordSeparators.wordSeparators.Contains(symbol);
		}

		internal static void Add(char separator)
		{
			WordSeparators.wordSeparators.Add(separator);
		}

		internal static void Clear()
		{
			WordSeparators.wordSeparators.Clear();
		}

		internal static string GetRegex()
		{
			if (WordSeparators.wordSeparators.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			foreach (char value in WordSeparators.wordSeparators)
			{
				stringBuilder.Append(value);
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		static readonly char[] defaultSeparators = new char[] { '.', ',', '!', '?', ' ', ':', ';' };

		static readonly List<char> wordSeparators = new List<char>();
	}
}
