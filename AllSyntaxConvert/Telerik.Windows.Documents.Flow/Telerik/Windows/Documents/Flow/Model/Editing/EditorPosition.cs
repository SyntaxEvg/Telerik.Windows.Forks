using System;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Editing
{
	class EditorPosition
	{
		public EditorPosition(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
		}

		public Paragraph CurrentParagraph { get; set; }

		public Table CurrentTable { get; set; }

		public int InlineIndex { get; set; }

		public bool IsAtTableEnd
		{
			get
			{
				return this.CurrentTable != null;
			}
		}

		public bool IsInitialized
		{
			get
			{
				return this.CurrentBlock != null;
			}
		}

		public BlockBase CurrentBlock
		{
			get
			{
				if (this.IsAtTableEnd)
				{
					return this.CurrentTable;
				}
				return this.CurrentParagraph;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public bool IsAtParagraphEnd()
		{
			return this.CurrentParagraph != null && this.CurrentParagraph.Inlines.Count == this.InlineIndex;
		}

		public bool IsInTableCell()
		{
			return this.CurrentParagraph != null && this.CurrentParagraph.EnumerateParentsOfType<TableCell>().FirstOrDefault<TableCell>() != null;
		}

		public void MoveToParagraphStart(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			Guard.ThrowExceptionIfFalse(paragraph.Document == this.document, "document");
			this.MoveToParagraphInternal(paragraph, 0);
		}

		public void MoveToInlineEnd(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			Guard.ThrowExceptionIfFalse(inline.Document == this.document, "document");
			this.MoveToParagraphInternal(inline.Paragraph, inline.Paragraph.Inlines.IndexOf(inline) + 1);
		}

		public void MoveToInlineStart(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			Guard.ThrowExceptionIfFalse(inline.Document == this.document, "document");
			this.MoveToParagraphInternal(inline.Paragraph, inline.Paragraph.Inlines.IndexOf(inline));
		}

		public void MoveAfterTable(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			Guard.ThrowExceptionIfFalse(table.Document == this.document, "document");
			this.CurrentTable = table;
			this.CurrentParagraph = null;
			this.InlineIndex = 0;
		}

		void MoveToParagraphInternal(Paragraph paragraph, int index)
		{
			this.CurrentParagraph = paragraph;
			this.InlineIndex = index;
			this.CurrentTable = null;
		}

		readonly RadFlowDocument document;
	}
}
