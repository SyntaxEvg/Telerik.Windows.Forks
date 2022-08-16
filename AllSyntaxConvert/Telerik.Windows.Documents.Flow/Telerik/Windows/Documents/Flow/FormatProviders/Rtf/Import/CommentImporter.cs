using System;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import
{
	class CommentImporter
	{
		public CommentImporter(RtfImportContext context)
		{
			this.context = context;
		}

		public Comment Comment { get; set; }

		public string RefID { get; set; }

		public string Author { get; set; }

		public string Initials { get; set; }

		public DateTime Date { get; set; }

		public void ImportCommentId(RtfGroup group)
		{
			this.Initials = RtfHelper.GetGroupText(group, true);
		}

		public void ImportAuthor(RtfGroup group)
		{
			this.Author = RtfHelper.GetGroupText(group, true);
		}

		public void ImportDate(RtfGroup group)
		{
			int numeric;
			if (int.TryParse(RtfHelper.GetGroupText(group, true), out numeric))
			{
				this.Date = RtfHelper.ConvertRtfIntToDateTime(numeric);
			}
		}

		public void ImportCommentDefinition(RtfGroup group)
		{
			RtfGroup rtfGroup = group.Elements.FirstOrDefault(delegate(RtfElement e)
			{
				RtfGroup rtfGroup2 = e as RtfGroup;
				return rtfGroup2 != null && rtfGroup2.Destination == "atnref";
			}) as RtfGroup;
			if (rtfGroup != null)
			{
				this.RefID = RtfHelper.GetGroupText(rtfGroup, true);
			}
			else
			{
				this.RefID = "singleRef" + AnnotationIdGenerator.GetNext().ToString();
			}
			if (!this.context.Comments.ContainsKey(this.RefID))
			{
				this.Comment = this.context.Document.Comments.AddComment();
				this.context.Comments[this.RefID] = this.Comment;
				this.context.AddInline(this.Comment.CommentRangeStart);
				this.context.AddInline(this.Comment.CommentRangeEnd);
			}
			this.Comment = this.context.Comments[this.RefID];
			RtfDocumentImporter rtfDocumentImporter = new RtfDocumentImporter(new RtfImportContext(this.context, this.Comment)
			{
				CurrentCommentHandler = this
			});
			rtfDocumentImporter.ImportRoot(group);
			this.Comment.Author = this.Author;
			this.Comment.Initials = this.Initials;
			this.Comment.Date = this.Date;
		}

		const string SingleCommentReferenceString = "singleRef";

		readonly RtfImportContext context;
	}
}
