using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class BookmarkRangeStart : AnnotationRangeStartBase
	{
		internal BookmarkRangeStart(RadFlowDocument document, Bookmark bookmark)
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
				return DocumentElementType.BookmarkRangeStart;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			BookmarkRangeStart bookmarkRangeStart = new Bookmark(cloneContext.Document, this.Bookmark.Name, this.Bookmark.FromColumn, this.Bookmark.ToColumn).BookmarkRangeStart;
			cloneContext.BookmarkContext.AddHangingAnnotationStart(bookmarkRangeStart);
			return bookmarkRangeStart;
		}

		readonly Bookmark bookmark;
	}
}
