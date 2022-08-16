using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Cloning
{
	class CloneContext
	{
		public CloneContext(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
			this.fieldContext = new CloneFieldContext();
			this.bookmarkContext = new AnnotationCloneContext<BookmarkRangeStart, BookmarkRangeEnd>();
			this.commentContext = new AnnotationCloneContext<CommentRangeStart, CommentRangeEnd>();
			this.permissionContext = new AnnotationCloneContext<PermissionRangeStart, PermissionRangeEnd>();
			this.renamedStyles = new Dictionary<string, string>();
			this.reinitializedLists = new Dictionary<int, int>();
			this.oldListsToStyles = new Dictionary<int, List<string>>();
		}

		public Dictionary<string, string> RenamedStyles
		{
			get
			{
				return this.renamedStyles;
			}
		}

		public Dictionary<int, int> ReinitializedLists
		{
			get
			{
				return this.reinitializedLists;
			}
		}

		public Dictionary<int, List<string>> OldListsToStyles
		{
			get
			{
				return this.oldListsToStyles;
			}
		}

		public CloneFieldContext FieldContext
		{
			get
			{
				return this.fieldContext;
			}
		}

		public AnnotationCloneContext<BookmarkRangeStart, BookmarkRangeEnd> BookmarkContext
		{
			get
			{
				return this.bookmarkContext;
			}
		}

		public AnnotationCloneContext<CommentRangeStart, CommentRangeEnd> CommentContext
		{
			get
			{
				return this.commentContext;
			}
		}

		public AnnotationCloneContext<PermissionRangeStart, PermissionRangeEnd> PermissionContext
		{
			get
			{
				return this.permissionContext;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public Section CurrentSection { get; set; }

		public MergeOptions MergeOptions { get; set; }

		readonly RadFlowDocument document;

		readonly CloneFieldContext fieldContext;

		readonly AnnotationCloneContext<BookmarkRangeStart, BookmarkRangeEnd> bookmarkContext;

		readonly AnnotationCloneContext<CommentRangeStart, CommentRangeEnd> commentContext;

		readonly AnnotationCloneContext<PermissionRangeStart, PermissionRangeEnd> permissionContext;

		readonly Dictionary<string, string> renamedStyles;

		readonly Dictionary<int, int> reinitializedLists;

		readonly Dictionary<int, List<string>> oldListsToStyles;
	}
}
