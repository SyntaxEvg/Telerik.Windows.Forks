using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Navigation;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	class BookmarksCollection : DocumentElementCollection<BookmarkItem, IFixedDocumentElement>, IFixedDocumentElement
	{
		internal BookmarksCollection(IFixedDocumentElement parent)
			: base(parent)
		{
		}

		IFixedDocumentElement IFixedDocumentElement.Parent
		{
			get
			{
				return base.Owner;
			}
		}

		internal IEnumerable<BookmarkItem> IterateBookmarksInDepthFirstSearch()
		{
			foreach (BookmarkItem bookmark in this)
			{
				yield return bookmark;
				foreach (BookmarkItem childBookmark in bookmark.Children.IterateBookmarksInDepthFirstSearch())
				{
					yield return childBookmark;
				}
			}
			yield break;
		}

		internal IEnumerable<BookmarkItem> IterateBookmarksInBreadthFirstSearch()
		{
			Queue<BookmarkItem> bookmarksQueue = new Queue<BookmarkItem>(this);
			while (bookmarksQueue.Count > 0)
			{
				BookmarkItem bookmark = bookmarksQueue.Dequeue();
				foreach (BookmarkItem item in bookmark.Children)
				{
					bookmarksQueue.Enqueue(item);
				}
				yield return bookmark;
			}
			yield break;
		}

		protected override void VerifyDocumentElementOnInsert(BookmarkItem item)
		{
			Guard.ThrowExceptionIfNull<BookmarkItem>(item, "item");
			if (item.Parent != null)
			{
				throw new InvalidOperationException("The element has already been associated with a collection.");
			}
		}
	}
}
