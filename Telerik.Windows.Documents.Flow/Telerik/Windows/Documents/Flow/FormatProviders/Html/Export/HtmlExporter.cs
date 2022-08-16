using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	class HtmlExporter
	{
		public HtmlExporter()
		{
			this.contentManager = new HtmlContentManager();
		}

		public void Export(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			context.BeginExport();
			switch (context.Settings.DocumentExportLevel)
			{
			case DocumentExportLevel.Document:
				this.ExportDocument(writer, context);
				break;
			case DocumentExportLevel.Fragment:
				this.ExportDocumentFragment(writer, context);
				break;
			}
			context.EndExport();
		}

		void ExportDocument(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			writer.WriteDocumentStart();
			HtmlElement htmlElement = this.contentManager.CreateElement<HtmlElement>("html");
			htmlElement.Write(writer, context);
		}

		void ExportDocumentFragment(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			BodyElement bodyElement = this.contentManager.CreateElement<BodyElement>("body");
			bodyElement.Write(writer, context);
		}

		readonly HtmlContentManager contentManager;
	}
}
