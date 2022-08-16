using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class BookmarkRangeEnd : AnnotationRangeEndBase
	{
		internal BookmarkRangeEnd(RadFlowDocument document, Bookmark bookmark)
			: base(document)
		{
			this.bookmark = bookmark;
		}

		public Bookmark Bookmark
		{
			get
			{
				return this.bookmark;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.BookmarkRangeEnd;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			BookmarkRangeStart bookmarkRangeStart = cloneContext.BookmarkContext.PopHangingAnnotationStart();
			if (bookmarkRangeStart != null)
			{
				return bookmarkRangeStart.Bookmark.BookmarkRangeEnd;
			}
			return null;
		}

		readonly Bookmark bookmark;
	}
}
