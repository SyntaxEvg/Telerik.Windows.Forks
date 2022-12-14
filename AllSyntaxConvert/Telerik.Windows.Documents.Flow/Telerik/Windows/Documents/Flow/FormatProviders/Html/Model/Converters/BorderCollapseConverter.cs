using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class BorderCollapseConverter : StringConverterBase<double>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out double result)
		{
			throw new InvalidOperationException("This converter is for export only");
		}

		public override bool ConvertBack(IHtmlExportContext context, double value, DocumentElementPropertiesBase properties, out string result)
		{
			result = ((value <= 0.0) ? "collapse" : "separate");
			return true;
		}
	}
}
