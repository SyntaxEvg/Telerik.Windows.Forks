using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	interface IHtmlImportContext
	{
		RadFlowDocument Document { get; }

		HtmlStyleRepository HtmlStyleRepository { get; set; }

		HtmlImportSettings Settings { get; }

		StyleNamesInfo StyleNamesInfo { get; set; }

		ListImportContext ListImportContext { get; }

		Paragraph CurrentParagraph { get; }

		bool ShouldImportNormalWeb { get; set; }

		void BeginParagraph(Paragraph paragraph);

		void EndParagraph();

		void BeginHyperlink();

		void EndHyperlink();

		void PushTable(Table table);

		void PopTable();

		void PushTableRow(TableRow row, RowType rowType);

		void PopTableRow();

		void InsertTableCell(TableCell cell, CellType cellType);

		void InsertRun(Run run);

		void InsertInline(InlineBase inline);

		void PopBlockContainer();

		void BeginImport();

		void EndImport();

		bool HasCurrentParagraph();
	}
}
