using System;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public class Comment : BlockContainerBase
	{
		internal Comment(RadFlowDocument document)
			: base(document)
		{
			this.commentRangeStart = new CommentRangeStart(document, this);
			this.commentRangeEnd = new CommentRangeEnd(document, this);
		}

		public string Author
		{
			get
			{
				return this.author;
			}
			set
			{
				this.author = value;
			}
		}

		public string Initials
		{
			get
			{
				return this.initials;
			}
			set
			{
				this.initials = value;
			}
		}

		public DateTime Date
		{
			get
			{
				return this.date;
			}
			set
			{
				this.date = value;
			}
		}

		public CommentRangeStart CommentRangeStart
		{
			get
			{
				return this.commentRangeStart;
			}
		}

		public CommentRangeEnd CommentRangeEnd
		{
			get
			{
				return this.commentRangeEnd;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Comment;
			}
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Comment comment = cloneContext.Document.Comments.AddComment();
			comment.Blocks.AddClonedChildrenFrom(base.Blocks, cloneContext);
			comment.Author = this.Author;
			comment.Initials = this.Initials;
			comment.Date = this.Date;
			return comment;
		}

		readonly CommentRangeStart commentRangeStart;

		readonly CommentRangeEnd commentRangeEnd;

		string author;

		string initials;

		DateTime date;
	}
}
