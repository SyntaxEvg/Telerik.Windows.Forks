using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.Text;

namespace Telerik.Windows.Documents.Fixed.Search
{
	static class KMPTextSearch
	{
		static int[] CalculatePrefixArray(string input)
		{
			int[] array = new int[input.Length];
			array[0] = 0;
			int num = 0;
			for (int i = 1; i < input.Length; i++)
			{
				while (num > 0 && input[num] != input[i])
				{
					num = array[num - 1];
				}
				if (input[num] == input[i])
				{
					num++;
				}
				array[i] = num;
			}
			return array;
		}

		static IEnumerable<SearchResult> Find(TextPage page, string searchText, int startIndex, bool matchCase, bool wholeWordsOnly, bool singleMatch)
		{
			string text = page.ToString(TextSearchOptions.TextSearchLinesSeparator);
			startIndex += TextSearch.GetStartIndexOffset(startIndex, page, TextSearchOptions.TextSearchLinesSeparator);
			List<SearchResult> list = new List<SearchResult>();
			if (startIndex < 0 || startIndex >= text.Length)
			{
				return list;
			}
			if (!matchCase)
			{
				text = text.ToLower();
				searchText = searchText.ToLower();
			}
			int[] array = KMPTextSearch.CalculatePrefixArray(searchText);
			int num = 0;
			for (int i = startIndex; i < text.Length; i++)
			{
				while (num > 0 && searchText[num] != text[i])
				{
					num = array[num - 1];
				}
				if (searchText[num] == text[i])
				{
					num++;
				}
				if (num == searchText.Length)
				{
					bool flag = true;
					if (wholeWordsOnly)
					{
						if (i < text.Length - 1 && !WordSeparators.IsWordSeparator(text[i + 1]))
						{
							flag = false;
						}
						if (i - searchText.Length >= 0 && !WordSeparators.IsWordSeparator(text[i - searchText.Length]))
						{
							flag = false;
						}
					}
					if (flag)
					{
						int num2 = i - searchText.Length + 1;
						num2 -= TextSearch.GetFoundIndexOffset(num2, page, TextSearchOptions.TextSearchLinesSeparator);
						TextPosition startPosition = new TextPosition(page, num2);
						TextPosition endPosition = new TextPosition(page, num2 + searchText.Length);
						list.Add(new SearchResult(startPosition, endPosition));
						if (singleMatch)
						{
							return list;
						}
					}
					num = array[num - 1];
				}
			}
			return list;
		}

		public static IEnumerable<SearchResult> FindAll(TextDocument document, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			List<SearchResult> list = new List<SearchResult>();
			for (int i = 0; i < document.PagesCount; i++)
			{
				list.AddRange(KMPTextSearch.Find(document.GetTextPage(i), searchText, 0, matchCase, wholeWordsOnly, false));
			}
			return list;
		}

		public static SearchResult FindNext(TextDocument document, TextPosition startPosition, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			int num = document.IndexOf(startPosition.TextPage);
			int startIndex = startPosition.Index;
			for (int i = num; i < document.PagesCount; i++)
			{
				IEnumerable<SearchResult> source = KMPTextSearch.Find(document.GetTextPage(i), searchText, startIndex, matchCase, wholeWordsOnly, true);
				if (source.Count<SearchResult>() > 0)
				{
					return source.First<SearchResult>();
				}
				startIndex = 0;
			}
			return SearchResult.NotFound;
		}

		public static SearchResult FindPrev(TextDocument document, TextPosition startPosition, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			int num = document.IndexOf(startPosition.TextPage);
			IEnumerable<SearchResult> source = KMPTextSearch.Find(document.GetTextPage(num), searchText, 0, matchCase, wholeWordsOnly, false);
			SearchResult searchResult = source.LastOrDefault((SearchResult r) => r.Range.EndPosition <= startPosition);
			if (searchResult != null)
			{
				return searchResult;
			}
			for (int i = num - 1; i >= 0; i--)
			{
				source = KMPTextSearch.Find(document.GetTextPage(i), searchText, 0, matchCase, false, false);
				if (source.Count<SearchResult>() > 0)
				{
					return source.Last<SearchResult>();
				}
			}
			return SearchResult.NotFound;
		}
	}
}
