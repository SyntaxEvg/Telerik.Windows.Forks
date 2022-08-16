using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class DoubleConverter : StringConverterBase<double>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out double result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			result = htmlStylePropertyValue.ValueAsDouble;
			if (htmlStylePropertyValue.UnitType != null && !Unit.IsRelativeUnitType(htmlStylePropertyValue.UnitType.Value))
			{
				result = Unit.UnitToDip(result, htmlStylePropertyValue.UnitType.Value);
			}
			return !double.IsNaN(result);
		}

		public override bool ConvertBack(IHtmlExportContext context, double value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<double>(value, "value");
			result = CssValueTypesHelper.ConvertToPixels(context, value);
			return true;
		}
	}
}
