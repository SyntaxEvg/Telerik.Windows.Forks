using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class LineSpacingConverter : StringConverterBase<double>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out double result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			result = double.NaN;
			if (htmlStylePropertyValue.UnitType != null)
			{
				if (CssValueTypesHelper.IsPercentValue(htmlStylePropertyValue))
				{
					result = htmlStylePropertyValue.ValueAsDouble / 100.0;
				}
				else if (CssValueTypesHelper.IsLengthValue(htmlStylePropertyValue) && !Unit.IsRelativeUnitType(htmlStylePropertyValue.UnitType.Value))
				{
					result = Unit.UnitToDip(htmlStylePropertyValue.ValueAsDouble, htmlStylePropertyValue.UnitType.Value);
				}
			}
			else if (CssValueTypesHelper.IsNumberValue(htmlStylePropertyValue))
			{
				result = htmlStylePropertyValue.ValueAsDouble;
			}
			return !double.IsNaN(result);
		}

		public override bool ConvertBack(IHtmlExportContext context, double value, DocumentElementPropertiesBase properties, out string result)
		{
			result = null;
			return false;
		}
	}
}
