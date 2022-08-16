using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class PreferredWidthConverter : StringConverterBase<TableWidthUnit>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out TableWidthUnit result)
		{
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			result = null;
			if (htmlStylePropertyValue.UnitType != null)
			{
				double value;
				if (htmlStylePropertyValue.UnitType.Value == UnitType.Percent)
				{
					result = new TableWidthUnit(TableWidthUnitType.Percent, htmlStylePropertyValue.ValueAsDouble);
				}
				else if (CssValueTypesHelper.TryConvertNonRelativeLengthValue(htmlStylePropertyValue, out value))
				{
					result = new TableWidthUnit(TableWidthUnitType.Fixed, value);
				}
			}
			else if (htmlStylePropertyValue.Value.ToLowerInvariant() == "auto")
			{
				result = new TableWidthUnit(TableWidthUnitType.Auto);
			}
			return result != null;
		}

		public override bool ConvertBack(IHtmlExportContext context, TableWidthUnit value, DocumentElementPropertiesBase properties, out string result)
		{
			switch (value.Type)
			{
			case TableWidthUnitType.Fixed:
				result = CssValueTypesHelper.ConvertToPixels(context, value.Value);
				goto IL_49;
			case TableWidthUnitType.Percent:
				result = CssValueTypesHelper.ConvertToPercents(context, value.Value);
				goto IL_49;
			}
			result = string.Empty;
			IL_49:
			return !string.IsNullOrEmpty(result);
		}
	}
}
