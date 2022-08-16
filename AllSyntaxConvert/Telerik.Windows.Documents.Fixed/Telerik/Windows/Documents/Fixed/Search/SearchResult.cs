using System;
using Telerik.Windows.Documents.Fixed.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Search
{
	public class SearchResult
	{
		internal SearchResult(TextPosition startPosition, TextPosition endPosition)
		{
			Guard.ThrowExceptionIfNull<TextPosition>(startPosition, "startPosition");
			Guard.ThrowExceptionIfNull<TextPosition>(endPosition, "endPosition");
			this.range = new TextRange(startPosition, endPosition);
			this.value = TextDocument.GetText(startPosition, endPosition, " ");
		}

		SearchResult()
		{
			this.range = TextRange.Empty;
		}

		public static SearchResult NotFound
		{
			get
			{
				return SearchResult.notFound;
			}
		}

		public TextRange Range
		{
			get
			{
				return this.range;
			}
		}

		public string Result
		{
			get
			{
				return this.value;
			}
		}

		public override string ToString()
		{
			return this.Result;
		}

		static readonly SearchResult notFound = new SearchResult();

		readonly TextRange range;

		readonly string value;
	}
}
