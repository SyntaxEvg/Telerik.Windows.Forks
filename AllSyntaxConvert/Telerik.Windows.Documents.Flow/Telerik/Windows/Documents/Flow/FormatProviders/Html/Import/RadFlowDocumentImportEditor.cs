using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class RadFlowDocumentImportEditor
	{
		public RadFlowDocumentImportEditor(RadFlowDocument document, HtmlImportSettings htmlImportSettings)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			this.document = document;
			this.importSettings = htmlImportSettings;
			this.blockContainersStack = new Stack<BlockContainerBase>();
			this.tablesStack = new Stack<Table>();
			this.tableRowStack = new Stack<TableRow>();
			this.tableRowTypesStack = new Stack<Dictionary<RowType, LinkedList<TableRow>>>();
			this.PushBlockContainer(this.document.Sections.AddSection());
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public Paragraph CurrentParagraph
		{
			get
			{
				return this.currentParagraph;
			}
		}

		BlockContainerBase CurrentBlockContainer
		{
			get
			{
				if (this.blockContainersStack.Count > 0)
				{
					return this.blockContainersStack.Peek();
				}
				return null;
			}
		}

		Table CurrentTable
		{
			get
			{
				if (this.tablesStack.Count > 0)
				{
					return this.tablesStack.Peek();
				}
				return null;
			}
		}

		TableRow CurrentTableRow
		{
			get
			{
				if (this.tableRowStack.Count > 0)
				{
					return this.tableRowStack.Peek();
				}
				return null;
			}
		}

		Dictionary<RowType, LinkedList<TableRow>> CurrentTableRows
		{
			get
			{
				if (this.tableRowTypesStack.Count > 0)
				{
					return this.tableRowTypesStack.Peek();
				}
				return null;
			}
		}

		public void BeginHyperlink()
		{
			this.hasCurrentHyperlink = true;
		}

		public void EndHyperlink()
		{
			this.hasCurrentHyperlink = false;
		}

		public void BeginParagraph(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.EndParagraph();
			this.currentParagraph = paragraph;
			this.CurrentBlockContainer.Blocks.Add(paragraph);
		}

		public void EndParagraph()
		{
			if (this.currentParagraph != null)
			{
				Run run = this.currentParagraph.Inlines.LastOrDefault<InlineBase>() as Run;
				if (run != null)
				{
					this.FinishRun(run);
				}
				if (this.currentParagraph.Inlines.Count == 0 && this.currentParagraph.ListId == DocumentDefaultStyleSettings.ListId)
				{
					((BlockContainerBase)this.currentParagraph.Parent).Blocks.Remove(this.currentParagraph);
				}
				RadFlowDocumentImportEditor.ClearNormalStyleId(this.currentParagraph);
				this.currentParagraph = null;
			}
		}

		public void PushTable(Table table)
		{
			this.tablesStack.Push(table);
			this.tableRowTypesStack.Push(RadFlowDocumentImportEditor.CreateTableRowsDictionary());
			this.CurrentBlockContainer.Blocks.Add(table);
		}

		public void PopTable()
		{
			if (this.CurrentTable != null)
			{
				Table table = this.tablesStack.Pop();
				Dictionary<RowType, LinkedList<TableRow>> dictionary = this.tableRowTypesStack.Pop();
				RadFlowDocumentImportEditor.InsertTableRows(table, dictionary[RowType.Header]);
				RadFlowDocumentImportEditor.InsertTableRows(table, dictionary[RowType.Body]);
				RadFlowDocumentImportEditor.InsertTableRows(table, dictionary[RowType.Footer]);
				RadFlowDocumentImportEditor.ClearTableNormalStyleId(table);
			}
		}

		public void PushTableRow(TableRow row, RowType rowType = RowType.Body)
		{
			Guard.ThrowExceptionIfNull<TableRow>(row, "row");
			if (this.CurrentTable != null)
			{
				this.CurrentTableRows[rowType].AddLast(row);
				this.tableRowStack.Push(row);
			}
		}

		public void PopTableRow()
		{
			if (this.CurrentTableRow != null)
			{
				this.tableRowStack.Pop();
			}
		}

		public void InsertTableCell(TableCell cell, CellType cellType)
		{
			Guard.ThrowExceptionIfNull<TableCell>(cell, "cell");
			Guard.ThrowExceptionIfNull<CellType>(cellType, "cellType");
			if (this.CurrentTableRow != null)
			{
				this.CurrentTableRow.Cells.Add(cell);
				this.PushBlockContainer(cell);
			}
		}

		public void InsertRun(Run run)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			this.AssureParagraphExists();
			run.Text = HtmlTextProcessor.RemoveMultipleWhiteSpaces(run.Text);
			if (!string.IsNullOrEmpty(run.Text))
			{
				this.PrepareRunToInsert(run);
				if (!string.IsNullOrEmpty(run.Text))
				{
					this.currentParagraph.Inlines.Add(run);
				}
			}
		}

		public void InsertInline(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			this.AssureParagraphExists();
			Run run = this.currentParagraph.Inlines.LastOrDefault<InlineBase>() as Run;
			if (run != null && inline.Type != DocumentElementType.Run && inline.Type != DocumentElementType.FieldCharacter)
			{
				this.FinishRun(run);
			}
			this.currentParagraph.Inlines.Add(inline);
		}

		public void PushBlockContainer(BlockContainerBase blockContainer)
		{
			this.blockContainersStack.Push(blockContainer);
			this.EndParagraph();
		}

		public void PopBlockContainer()
		{
			this.EndParagraph();
			this.blockContainersStack.Pop();
		}

		public bool HasCurrentParagraph()
		{
			return this.currentParagraph != null;
		}

		public void FinishDocument()
		{
			this.EndParagraph();
			this.PopTable();
		}

		static void ClearNormalStyleId(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			if (paragraph.StyleId == StyleNamesConverter.PrefixedNormalStyleId)
			{
				paragraph.StyleId = null;
			}
		}

		static void ClearTableNormalStyleId(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			if (table.StyleId == StyleNamesConverter.PrefixedNormalTableStyleId)
			{
				table.StyleId = null;
			}
		}

		static Dictionary<RowType, LinkedList<TableRow>> CreateTableRowsDictionary()
		{
			Dictionary<RowType, LinkedList<TableRow>> dictionary = new Dictionary<RowType, LinkedList<TableRow>>();
			dictionary[RowType.Header] = new LinkedList<TableRow>();
			dictionary[RowType.Body] = new LinkedList<TableRow>();
			dictionary[RowType.Footer] = new LinkedList<TableRow>();
			return dictionary;
		}

		static void InsertTableRows(Table table, LinkedList<TableRow> tableRows)
		{
			foreach (TableRow tableRow in tableRows)
			{
				if (tableRow.Cells.Count > 0)
				{
					table.Rows.Add(tableRow);
				}
			}
		}

		void PrepareRunToInsert(Run run)
		{
			Run run2 = this.currentParagraph.Inlines.LastOrDefault<InlineBase>() as Run;
			string lastText = ((run2 != null) ? run2.Text : string.Empty);
			run.Text = HtmlTextProcessor.StripText(run.Text, lastText);
			if (this.hasCurrentHyperlink && string.IsNullOrEmpty(run.StyleId))
			{
				run.StyleId = "Hyperlink";
			}
		}

		void AssureParagraphExists()
		{
			if (!this.HasCurrentParagraph())
			{
				this.BeginParagraph(new Paragraph(this.Document));
			}
		}

		void FinishRun(Run run)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			run.Text = HtmlTextProcessor.TrimEnd(run.Text);
			if (this.importSettings.ReplaceNonBreakingSpaces)
			{
				run.Text = HtmlTextProcessor.Normalize(run.Text);
			}
			if (string.IsNullOrEmpty(run.Text))
			{
				this.currentParagraph.Inlines.Remove(run);
			}
		}

		readonly RadFlowDocument document;

		readonly HtmlImportSettings importSettings;

		readonly Stack<BlockContainerBase> blockContainersStack;

		readonly Stack<Table> tablesStack;

		readonly Stack<Dictionary<RowType, LinkedList<TableRow>>> tableRowTypesStack;

		readonly Stack<TableRow> tableRowStack;

		Paragraph currentParagraph;

		bool hasCurrentHyperlink;
	}
}
