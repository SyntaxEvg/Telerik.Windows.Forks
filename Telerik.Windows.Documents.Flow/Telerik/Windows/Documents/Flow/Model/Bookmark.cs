using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class Bookmark
	{
		public Bookmark(RadFlowDocument document, string name)
			: this(document, name, null, null)
		{
		}

		public Bookmark(RadFlowDocument document, string name, int? fromColumn, int? toColumn)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfContainsWhitespace(name, "name");
			this.document = document;
			this.bookmarkRangeStart = new BookmarkRangeStart(document, this);
			this.bookmarkRangeEnd = new BookmarkRangeEnd(document, this);
			this.name = name;
			this.fromColumn = fromColumn;
			this.toColumn = toColumn;
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public BookmarkRangeStart BookmarkRangeStart
		{
			get
			{
				return this.bookmarkRangeStart;
			}
		}

		public BookmarkRangeEnd BookmarkRangeEnd
		{
			get
			{
				return this.bookmarkRangeEnd;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int? FromColumn
		{
			get
			{
				return this.fromColumn;
			}
		}

		public int? ToColumn
		{
			get
			{
				return this.toColumn;
			}
		}

		internal static bool IsValidBookmarkName(string bookmarkName, bool allowHidden)
		{
			if (string.IsNullOrEmpty(bookmarkName))
			{
				return false;
			}
			if (allowHidden && bookmarkName.StartsWith(Bookmark.HiddenPrefix))
			{
				bookmarkName = bookmarkName.Substring(Bookmark.HiddenPrefix.Length);
			}
			if (!char.IsLetter(bookmarkName[0]))
			{
				return false;
			}
			foreach (char c in bookmarkName)
			{
				if (!char.IsLetterOrDigit(c))
				{
					return false;
				}
			}
			return true;
		}

		internal static readonly string HiddenPrefix = "_";

		readonly RadFlowDocument document;

		readonly BookmarkRangeStart bookmarkRangeStart;

		readonly BookmarkRangeEnd bookmarkRangeEnd;

		readonly string name;

		readonly int? fromColumn;

		readonly int? toColumn;
	}
}
