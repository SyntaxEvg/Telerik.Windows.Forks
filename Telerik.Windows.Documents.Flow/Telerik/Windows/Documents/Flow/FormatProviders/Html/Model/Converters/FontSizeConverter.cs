using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class FontSizeConverter : StringConverterBase<double>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out double result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			result = double.NaN;
			if (property.Values.Count > 1)
			{
				return false;
			}
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			IStyleProperty<double?> styleProperty = properties.GetStyleProperty(Run.FontSizePropertyDefinition) as IStyleProperty<double?>;
			if (htmlStylePropertyValue.UnitType != null && Unit.IsRelativeUnitType(htmlStylePropertyValue.UnitType.Value))
			{
				double basePixelSize;
				if (property.BaseProperty != null)
				{
					HtmlConverters.FontSizeConverter.Convert(context, property.BaseProperty, properties, out basePixelSize);
				}
				else
				{
					basePixelSize = (double)styleProperty.GetActualValueAsObject();
				}
				result = Unit.UnitToPixel(basePixelSize, htmlStylePropertyValue.ValueAsDouble, htmlStylePropertyValue.UnitType.Value);
			}
			else
			{
				result = Unit.UnitToDip(htmlStylePropertyValue.ValueAsDouble, (htmlStylePropertyValue.UnitType != null) ? htmlStylePropertyValue.UnitType.Value : UnitType.Dip);
			}
			return !double.IsNaN(result) && result > 0.1;
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
