using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class CommentRangeStart : AnnotationRangeStartBase
	{
		internal CommentRangeStart(RadFlowDocument document, Comment comment)
			: base(document)
		{
			this.comment = comment;
		}

		public Comment Comment
		{
			get
			{
				return this.comment;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.CommentRangeStart;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			CommentRangeStart commentRangeStart = ((Comment)this.Comment.CloneCore(cloneContext)).CommentRangeStart;
			cloneContext.CommentContext.AddHangingAnnotationStart(commentRangeStart);
			return commentRangeStart;
		}

		readonly Comment comment;
	}
}
