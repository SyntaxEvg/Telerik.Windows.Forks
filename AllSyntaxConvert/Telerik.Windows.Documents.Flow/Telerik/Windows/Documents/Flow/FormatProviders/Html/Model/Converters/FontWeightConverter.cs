using System;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class FontWeightConverter : StringConverterBase<FontWeight>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out FontWeight result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			return HtmlValueMappers.FontWeightValueMapper.TryGetToValue(property.Values[0].Value, out result);
		}

		public override bool ConvertBack(IHtmlExportContext context, FontWeight value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontWeight>(value, "value");
			return HtmlValueMappers.FontWeightValueMapper.TryGetFromValue(value, out result);
		}
	}
}
