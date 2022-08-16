using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Search
{
	class TextSearch
	{
		public TextSearch(TextDocument document)
		{
			Guard.ThrowExceptionIfNull<TextDocument>(document, "document");
			this.document = document;
			this.startPosition = new TextPosition(this.document);
		}

		public SearchResult Find(string text, TextSearchOptions options)
		{
			Guard.ThrowExceptionIfNullOrEmpty(text, "text");
			Guard.ThrowExceptionIfNull<TextSearchOptions>(options, "options");
			this.InitFind(text, options, true);
			SearchResult searchResult;
			if (options.UseRegularExpression)
			{
				searchResult = RegexTextDocumentSearch.FindNext(this.document, this.startPosition, text, options.CaseSensitive, options.WholeWordsOnly);
			}
			else
			{
				searchResult = KMPTextSearch.FindNext(this.document, this.startPosition, text, options.CaseSensitive, options.WholeWordsOnly);
			}
			if (searchResult == SearchResult.NotFound)
			{
				this.startPosition.MoveToStartOfDocument();
			}
			else
			{
				this.startPosition.MoveToPosition(searchResult.Range.StartPosition);
				this.startPosition.MoveToNextPosition();
			}
			return searchResult;
		}

		public SearchResult FindPrevious(string text, TextSearchOptions options)
		{
			this.InitFind(text, options, false);
			SearchResult searchResult;
			if (options.UseRegularExpression)
			{
				searchResult = RegexTextDocumentSearch.FindPrev(this.document, this.startPosition, text, options.CaseSensitive, options.WholeWordsOnly);
			}
			else
			{
				searchResult = KMPTextSearch.FindPrev(this.document, this.startPosition, text, options.CaseSensitive, options.WholeWordsOnly);
			}
			if (searchResult == SearchResult.NotFound)
			{
				this.startPosition.MoveToEndOfDocument();
			}
			else
			{
				this.startPosition.MoveToPosition(searchResult.Range.EndPosition);
				this.startPosition.MoveToPreviousPosition();
			}
			return searchResult;
		}

		public IEnumerable<SearchResult> FindAll(string text, TextSearchOptions options)
		{
			if (options.UseRegularExpression)
			{
				return RegexTextDocumentSearch.FindAll(this.document, text, options.CaseSensitive, options.WholeWordsOnly);
			}
			return KMPTextSearch.FindAll(this.document, text, options.CaseSensitive, options.WholeWordsOnly);
		}

		internal static int GetStartIndexOffset(int startIndex, TextPage page, string separator)
		{
			int num = 0;
			foreach (Line line in page.Lines)
			{
				if (line.FirstIndex <= startIndex && startIndex < line.LastIndex)
				{
					break;
				}
				if (line.LastWord != null && line.LastWord.EndsWithSpace)
				{
					num += separator.Length;
				}
				else
				{
					num += separator.Length - 1;
				}
			}
			return num;
		}

		internal static int GetFoundIndexOffset(int foundIndex, TextPage page, string separator)
		{
			int num = 0;
			foreach (Line line in page.Lines)
			{
				if (num + line.LastIndex + separator.Length >= foundIndex)
				{
					break;
				}
				if (line.LastWord.EndsWithSpace)
				{
					num += separator.Length;
				}
				else
				{
					num += separator.Length - 1;
				}
			}
			return num;
		}

		void InitFind(string text, TextSearchOptions options, bool forward)
		{
			Guard.ThrowExceptionIfNull<string>(text, "text");
			Guard.ThrowExceptionIfNull<TextSearchOptions>(options, "options");
			if (this.options != options)
			{
				this.options = options;
			}
			if (this.searchText != text)
			{
				this.searchText = text;
				if (forward)
				{
					this.startPosition.MoveToStartOfDocument();
					return;
				}
				this.startPosition.MoveToEndOfDocument();
			}
		}

		readonly TextDocument document;

		readonly TextPosition startPosition;

		TextSearchOptions options;

		string searchText;
	}
}
