using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	interface IStringConverter
	{
		bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result);

		bool ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result);
	}
}
