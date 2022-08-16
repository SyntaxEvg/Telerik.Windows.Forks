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
	class LineSpacingTypeConverter : IStringConverter
	{
		public bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			result = HeightType.Auto;
			if (htmlStylePropertyValue.UnitType != null)
			{
				UnitType value = htmlStylePropertyValue.UnitType.Value;
				switch (value)
				{
				case UnitType.Dip:
				case UnitType.Point:
					break;
				default:
					if (value != UnitType.Em)
					{
						return true;
					}
					break;
				}
				result = HeightType.Exact;
			}
			return true;
		}

		public bool ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<object>(value, "value");
			HeightType heightType = (HeightType)value;
			IStyleProperty<double?> styleProperty = properties.GetStyleProperty(Paragraph.LineSpacingPropertyDefinition) as IStyleProperty<double?>;
			double? actualValue = styleProperty.GetActualValue();
			if (heightType == HeightType.Auto)
			{
				result = CssValueTypesHelper.ConvertToPercents(context, actualValue.Value * 100.0);
			}
			else
			{
				result = CssValueTypesHelper.ConvertToPixels(context, actualValue.Value);
			}
			return true;
		}
	}
}
