using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Fixed.Text;

namespace Telerik.Windows.Documents.Fixed.Search
{
	static class RegexTextDocumentSearch
	{
		static string CreateRegexWholeWord(string word)
		{
			string regex = WordSeparators.GetRegex();
			word = "(" + word + ")";
			return string.Concat(new string[]
			{
				regex, word, regex, "|^", word, regex, "|", regex, word, "$|^",
				word, "$"
			});
		}

		static int GetMatchGroupIndex(Match match)
		{
			for (int i = 1; i < match.Groups.Count; i++)
			{
				if (match.Groups[i].Success)
				{
					return i;
				}
			}
			return 0;
		}

		static IEnumerable<SearchResult> FindAll(TextPage page, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			string input = page.ToString(TextSearchOptions.TextSearchLinesSeparator);
			RegexOptions options = (matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
			if (wholeWordsOnly)
			{
				searchText = RegexTextDocumentSearch.CreateRegexWholeWord(searchText);
			}
			MatchCollection matchCollection = Regex.Matches(input, searchText, options);
			List<SearchResult> list = new List<SearchResult>();
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				int matchGroupIndex = RegexTextDocumentSearch.GetMatchGroupIndex(match);
				int num = match.Groups[matchGroupIndex].Index - TextSearch.GetFoundIndexOffset(match.Groups[matchGroupIndex].Index, page, TextSearchOptions.TextSearchLinesSeparator);
				TextPosition startPosition = new TextPosition(page, num);
				TextPosition endPosition = new TextPosition(page, num + match.Groups[matchGroupIndex].Length);
				list.Add(new SearchResult(startPosition, endPosition));
			}
			return list;
		}

		static SearchResult FindSingle(TextPage page, string searchText, int startIndex, bool matchCase, bool wholeWordsOnly)
		{
			startIndex += TextSearch.GetStartIndexOffset(startIndex, page, TextSearchOptions.TextSearchLinesSeparator);
			string input = page.ToString(TextSearchOptions.TextSearchLinesSeparator).Substring(startIndex);
			RegexOptions options = (matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
			if (wholeWordsOnly)
			{
				searchText = RegexTextDocumentSearch.CreateRegexWholeWord(searchText);
			}
			Match match = Regex.Match(input, searchText, options);
			if (match.Success)
			{
				int matchGroupIndex = RegexTextDocumentSearch.GetMatchGroupIndex(match);
				int num = match.Groups[matchGroupIndex].Index - TextSearch.GetFoundIndexOffset(startIndex + match.Groups[matchGroupIndex].Index, page, TextSearchOptions.TextSearchLinesSeparator);
				TextPosition endPosition = new TextPosition(page, startIndex + num + match.Groups[matchGroupIndex].Length);
				TextPosition startPosition = new TextPosition(page, startIndex + num);
				return new SearchResult(startPosition, endPosition);
			}
			return SearchResult.NotFound;
		}

		public static IEnumerable<SearchResult> FindAll(TextDocument document, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			List<SearchResult> list = new List<SearchResult>();
			for (int i = 0; i < document.PagesCount; i++)
			{
				list.AddRange(RegexTextDocumentSearch.FindAll(document.GetTextPage(i), searchText, matchCase, wholeWordsOnly));
			}
			return list;
		}

		public static SearchResult FindNext(TextDocument document, TextPosition startPosition, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			int num = document.IndexOf(startPosition.TextPage);
			int startIndex = startPosition.Index;
			for (int i = num; i < document.PagesCount; i++)
			{
				SearchResult searchResult = RegexTextDocumentSearch.FindSingle(document.GetTextPage(i), searchText, startIndex, matchCase, wholeWordsOnly);
				if (searchResult != SearchResult.NotFound)
				{
					return searchResult;
				}
				startIndex = 0;
			}
			return SearchResult.NotFound;
		}

		public static SearchResult FindPrev(TextDocument document, TextPosition startPosition, string searchText, bool matchCase, bool wholeWordsOnly)
		{
			int num = document.IndexOf(startPosition.TextPage);
			IEnumerable<SearchResult> source = RegexTextDocumentSearch.FindAll(document.GetTextPage(num), searchText, matchCase, wholeWordsOnly);
			SearchResult searchResult = source.LastOrDefault((SearchResult r) => r.Range.EndPosition <= startPosition);
			if (searchResult != null)
			{
				return searchResult;
			}
			for (int i = num - 1; i >= 0; i--)
			{
				source = RegexTextDocumentSearch.FindAll(document.GetTextPage(i), searchText, matchCase, wholeWordsOnly);
				if (source.Count<SearchResult>() > 0)
				{
					return source.Last<SearchResult>();
				}
			}
			return SearchResult.NotFound;
		}
	}
}
