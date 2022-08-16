using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Export
{
	static class StyleExporter
	{
		public static string Export(IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			CssStyleWriter cssStyleWriter = new CssStyleWriter();
			foreach (Selector selector in context.HtmlStyleRepository.GetStyles())
			{
				selector.Write(cssStyleWriter, context);
			}
			return cssStyleWriter.GetData();
		}
	}
}
