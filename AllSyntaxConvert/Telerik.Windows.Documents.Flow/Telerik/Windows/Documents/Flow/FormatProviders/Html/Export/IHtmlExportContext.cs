using System;
using System.Globalization;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	interface IHtmlExportContext
	{
		CultureInfo Culture { get; }

		RadFlowDocument Document { get; }

		HtmlExportSettings Settings { get; }

		HtmlStyleRepository HtmlStyleRepository { get; }

		ImageRepository ImageRepository { get; }

		void BeginExport();

		void EndExport();

		void ClearParagraphClasses(ClassNamesCollection classNamesCollection);

		void BeginExportTable(Table table);

		void EndExportTable(Table table);

		TableBorderGrid GetBorderGrid(Table table);
	}
}
