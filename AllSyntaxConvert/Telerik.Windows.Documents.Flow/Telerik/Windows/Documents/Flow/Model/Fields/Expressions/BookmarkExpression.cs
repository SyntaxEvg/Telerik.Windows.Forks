using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Fields.Expressions
{
	class BookmarkExpression : Expression
	{
		public BookmarkExpression(string bookmark, RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNullOrEmpty(bookmark, "bookmark");
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.bookmarkName = bookmark;
			this.document = document;
		}

		public string BookmarkName
		{
			get
			{
				return this.bookmarkName;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public override ExpressionResult GetResult()
		{
			Bookmark bookmarkByName = this.document.GetBookmarkByName(this.BookmarkName);
			if (bookmarkByName == null)
			{
				return ErrorExpressions.GetExpressionResult(ExpressionErrorType.UndefinedBookmark);
			}
			string bookmarkContent = BookmarkExpression.GetBookmarkContent(bookmarkByName);
			Expression expression = ExpressionParser.Parse(bookmarkContent, this.Document);
			return expression.GetResult();
		}

		static string GetBookmarkContent(Bookmark bmk)
		{
			return InlineRangeEditor.GetTextInRange(bmk.BookmarkRangeStart, bmk.BookmarkRangeEnd, true, false);
		}

		readonly RadFlowDocument document;

		readonly string bookmarkName;
	}
}
