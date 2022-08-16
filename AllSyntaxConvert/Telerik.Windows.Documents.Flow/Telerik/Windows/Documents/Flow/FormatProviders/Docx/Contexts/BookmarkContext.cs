using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts
{
	class BookmarkContext : AnnotationContextBase<Bookmark>
	{
		public BookmarkContext(RadFlowDocument document)
			: base(document)
		{
			this.hangingBookmarkStarts = new List<BookmarkRangeStart>();
			this.hangingBookmarkEndIds = new List<int>();
		}

		public static Bookmark CreateBookmark(RadFlowDocument document, string name, int colFirst, int colLast)
		{
			Bookmark result;
			if (colFirst != colLast)
			{
				result = new Bookmark(document, name, new int?(colFirst), new int?(colLast));
			}
			else
			{
				result = new Bookmark(document, name);
			}
			return result;
		}

		public void AddHangingBookmarkStart(BookmarkRangeStart annotationStart)
		{
			this.hangingBookmarkStarts.Add(annotationStart);
		}

		public void AddHangingBookmarkEndId(int id)
		{
			this.hangingBookmarkEndIds.Add(id);
		}

		public List<BookmarkRangeStart> GetHangingBookmarkStarts()
		{
			return this.hangingBookmarkStarts;
		}

		public List<int> GetHangingBookmarkEndIds()
		{
			return this.hangingBookmarkEndIds;
		}

		public void ClearHangingBookmarkStarts()
		{
			this.hangingBookmarkStarts.Clear();
		}

		public void ClearHangingBookmarkEnds()
		{
			this.hangingBookmarkEndIds.Clear();
		}

		readonly List<BookmarkRangeStart> hangingBookmarkStarts;

		readonly List<int> hangingBookmarkEndIds;
	}
}
