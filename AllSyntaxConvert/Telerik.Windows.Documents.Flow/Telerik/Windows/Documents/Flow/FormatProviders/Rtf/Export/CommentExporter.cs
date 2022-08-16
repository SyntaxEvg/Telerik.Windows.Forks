using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class CommentExporter
	{
		public CommentExporter(RtfDocumentExporter exporter)
		{
			this.documentExporter = exporter;
			this.writer = exporter.Writer;
		}

		ExportContext Context
		{
			get
			{
				return this.documentExporter.Context;
			}
		}

		public void ExportComment(CommentRangeEnd commentRangeEnd)
		{
			using (this.writer.WriteGroup("atnid", true))
			{
				this.writer.WriteText(commentRangeEnd.Comment.Initials);
			}
			using (this.writer.WriteGroup("atnauthor", true))
			{
				this.writer.WriteText(commentRangeEnd.Comment.Author);
			}
			this.writer.WriteTag("chatn");
			this.ExportCommentDefinition(commentRangeEnd);
		}

		void ExportCommentDefinition(CommentRangeEnd commentRangeEnd)
		{
			using (this.writer.WriteGroup("annotation", true))
			{
				using (this.writer.WriteGroup("atnref", true))
				{
					this.writer.WriteText(this.Context.Comments[commentRangeEnd.Comment].ToString());
				}
				using (this.writer.WriteGroup("atndate", true))
				{
					this.writer.WriteText(RtfHelper.ConvertDateTimeToRtfInt(commentRangeEnd.Comment.Date).ToString());
				}
				foreach (BlockBase block in commentRangeEnd.Comment.Blocks)
				{
					this.documentExporter.ExportBlock(block);
				}
			}
		}

		readonly RtfDocumentExporter documentExporter;

		readonly RtfWriter writer;
	}
}
