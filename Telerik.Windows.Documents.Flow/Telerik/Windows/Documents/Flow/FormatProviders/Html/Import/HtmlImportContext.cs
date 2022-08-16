using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class HtmlImportContext : IHtmlImportContext
	{
		public HtmlImportContext(HtmlImportSettings settings)
		{
			Guard.ThrowExceptionIfNull<HtmlImportSettings>(settings, "settings");
			this.settings = settings;
			RadFlowDocument document = HtmlImportDocumentFactory.CreateDocumentForImport();
			this.editor = new RadFlowDocumentImportEditor(document, this.settings);
			this.listImportContext = new ListImportContext();
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.editor.Document;
			}
		}

		public HtmlImportSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public HtmlStyleRepository HtmlStyleRepository { get; set; }

		public StyleNamesInfo StyleNamesInfo { get; set; }

		public Paragraph CurrentParagraph
		{
			get
			{
				return this.editor.CurrentParagraph;
			}
		}

		public ListImportContext ListImportContext
		{
			get
			{
				return this.listImportContext;
			}
		}

		public bool ShouldImportNormalWeb { get; set; }

		public void BeginParagraph(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.editor.BeginParagraph(paragraph);
		}

		public void EndParagraph()
		{
			this.editor.EndParagraph();
		}

		public void BeginHyperlink()
		{
			this.editor.BeginHyperlink();
		}

		public void EndHyperlink()
		{
			this.editor.EndHyperlink();
		}

		public void PushTable(Table table)
		{
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			this.editor.PushTable(table);
		}

		public void PopTable()
		{
			this.editor.PopTable();
		}

		public void PushTableRow(TableRow row, RowType rowType)
		{
			Guard.ThrowExceptionIfNull<TableRow>(row, "row");
			this.editor.PushTableRow(row, rowType);
		}

		public void PopTableRow()
		{
			this.editor.PopTableRow();
		}

		public void InsertTableCell(TableCell cell, CellType cellType)
		{
			Guard.ThrowExceptionIfNull<TableCell>(cell, "cell");
			this.editor.InsertTableCell(cell, cellType);
		}

		public void InsertRun(Run run)
		{
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			this.editor.InsertRun(run);
		}

		public void InsertInline(InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			this.editor.InsertInline(inline);
		}

		public void PopBlockContainer()
		{
			this.editor.PopBlockContainer();
		}

		public void BeginImport()
		{
		}

		public void EndImport()
		{
			StylesImporter.Import(this);
			this.editor.FinishDocument();
		}

		public bool HasCurrentParagraph()
		{
			return this.editor.HasCurrentParagraph();
		}

		readonly HtmlImportSettings settings;

		readonly RadFlowDocumentImportEditor editor;

		readonly ListImportContext listImportContext;
	}
}
