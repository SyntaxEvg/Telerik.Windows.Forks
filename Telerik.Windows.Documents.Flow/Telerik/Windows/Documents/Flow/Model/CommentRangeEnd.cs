using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class CommentRangeEnd : AnnotationRangeEndBase
	{
		internal CommentRangeEnd(RadFlowDocument document, Comment comment)
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
				return DocumentElementType.CommentRangeEnd;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			CommentRangeStart commentRangeStart = cloneContext.CommentContext.PopHangingAnnotationStart();
			if (commentRangeStart != null)
			{
				return commentRangeStart.Comment.CommentRangeEnd;
			}
			return null;
		}

		readonly Comment comment;
	}
}
