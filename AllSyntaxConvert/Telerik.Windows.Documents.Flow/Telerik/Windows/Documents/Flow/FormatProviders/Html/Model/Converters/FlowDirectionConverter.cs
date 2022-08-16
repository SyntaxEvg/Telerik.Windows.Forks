using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class FlowDirectionConverter : StringConverterBase<FlowDirection>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out FlowDirection result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			return HtmlValueMappers.FlowDirectionValueMapper.TryGetToValue(property.Values[0].Value.ToLowerInvariant(), out result);
		}

		public override bool ConvertBack(IHtmlExportContext context, FlowDirection value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FlowDirection>(value, "value");
			return HtmlValueMappers.FlowDirectionValueMapper.TryGetFromValue(value, out result);
		}
	}
}
