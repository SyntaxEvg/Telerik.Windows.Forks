using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class SpacingAfterConverter : StringConverterBase<double>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out double result)
		{
			return HtmlConverters.DoubleConverter.Convert(context, property, properties, out result);
		}

		public override bool ConvertBack(IHtmlExportContext context, double value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<double>(value, "value");
			ParagraphProperties paragraphProperties = properties as ParagraphProperties;
			if (paragraphProperties != null)
			{
				bool? actualValue = paragraphProperties.AutomaticSpacingAfter.GetActualValue();
				if (actualValue != null && actualValue.Value)
				{
					result = null;
					return false;
				}
			}
			return HtmlConverters.DoubleConverter.ConvertBack(context, value, properties, out result);
		}
	}
}
