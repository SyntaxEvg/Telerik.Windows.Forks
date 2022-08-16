using System;
using Telerik.Windows.Documents.Fixed.Model.Actions;
using Telerik.Windows.Documents.Fixed.Model.Collections;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Navigation
{
	public class BookmarkItem : FixedDocumentElementBase
	{
		public BookmarkItem()
		{
			this.children = new BookmarksCollection(this);
		}

		public bool IsExpanded { get; set; }

		public Destination Destination { get; set; }

		public Telerik.Windows.Documents.Fixed.Model.Actions.Action Action { get; set; }

		public BookmarkItemStyle TextStyle { get; set; }

		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "Title");
				this.title = value;
			}
		}

		internal BookmarksCollection Children
		{
			get
			{
				return this.children;
			}
		}

		internal override FixedDocumentElementType ElementType
		{
			get
			{
				return FixedDocumentElementType.BookmarkItem;
			}
		}

		readonly BookmarksCollection children;

		string title;
	}
}
