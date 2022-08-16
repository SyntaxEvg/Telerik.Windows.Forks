using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class MappedConverter<T> : StringConverterBase<T>
	{
		public MappedConverter(ValueMapper<string, T> mapper)
		{
			Guard.ThrowExceptionIfNull<ValueMapper<string, T>>(mapper, "mapper");
			this.mapper = mapper;
		}

		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out T result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			return this.mapper.TryGetToValue(property.Values[0].Value.ToLowerInvariant(), out result);
		}

		public override bool ConvertBack(IHtmlExportContext context, T value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<T>(value, "value");
			return this.mapper.TryGetFromValue(value, out result);
		}

		readonly ValueMapper<string, T> mapper;
	}
}
