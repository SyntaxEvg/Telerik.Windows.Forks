using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	abstract class StringConverterBase<T> : IStringConverter
	{
		public abstract bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out T result);

		public abstract bool ConvertBack(IHtmlExportContext context, T value, DocumentElementPropertiesBase properties, out string result);

		bool IStringConverter.Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result)
		{
			T t;
			bool result2 = this.Convert(context, property, properties, out t);
			result = t;
			return result2;
		}

		bool IStringConverter.ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result)
		{
			return this.ConvertBack(context, (T)((object)value), properties, out result);
		}
	}
}
