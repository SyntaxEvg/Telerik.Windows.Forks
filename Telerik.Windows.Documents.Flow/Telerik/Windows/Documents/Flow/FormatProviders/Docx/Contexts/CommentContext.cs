using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts
{
	class CommentContext : AnnotationContextBase<Comment>
	{
		public CommentContext(RadFlowDocument document)
			: base(document)
		{
			this.importedCommentIds = new Queue<int>();
			this.importedIdToCommentPairs = new Dictionary<int, Comment>();
		}

		public bool HasCommentsToExport
		{
			get
			{
				return (from c in base.Document.Comments
					where c.CommentRangeStart.Paragraph != null || c.CommentRangeEnd.Paragraph != null
					select c).Any<Comment>();
			}
		}

		public static Comment CreateComment(RadFlowDocument document)
		{
			return document.Comments.AddComment();
		}

		public void AddImportedComment(int id, Comment comment)
		{
			this.importedIdToCommentPairs.Add(id, comment);
		}

		public Comment GetImportedCommentById(int id)
		{
			Comment result;
			this.importedIdToCommentPairs.TryGetValue(id, out result);
			return result;
		}

		public Comment PopImportedComment()
		{
			int key = this.importedCommentIds.Dequeue();
			Comment result = this.importedIdToCommentPairs[key];
			this.importedIdToCommentPairs.Remove(key);
			return result;
		}

		public void AddIdToCommentsPartImport(int id)
		{
			this.importedCommentIds.Enqueue(id);
		}

		readonly Queue<int> importedCommentIds;

		readonly Dictionary<int, Comment> importedIdToCommentPairs;
	}
}
