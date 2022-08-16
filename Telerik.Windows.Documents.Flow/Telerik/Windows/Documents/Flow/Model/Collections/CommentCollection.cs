using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public sealed class CommentCollection : DocumentElementCollection<Comment, RadFlowDocument>
	{
		internal CommentCollection(RadFlowDocument document)
			: base(document)
		{
		}

		public Comment AddComment()
		{
			Comment comment = new Comment(base.Owner.Document);
			base.Add(comment);
			return comment;
		}

		protected override void OnAfterRemove(Comment comment)
		{
			Guard.ThrowExceptionIfNull<Comment>(comment, "comment");
			CommentRangeStart commentRangeStart = comment.CommentRangeStart;
			if (commentRangeStart.Paragraph != null)
			{
				commentRangeStart.Paragraph.Inlines.Remove(commentRangeStart);
			}
			CommentRangeEnd commentRangeEnd = comment.CommentRangeEnd;
			if (commentRangeEnd.Paragraph != null)
			{
				commentRangeEnd.Paragraph.Inlines.Remove(commentRangeEnd);
			}
		}
	}
}
