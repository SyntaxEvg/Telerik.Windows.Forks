using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class RowHeightConverter : StringConverterBase<TableRowHeight>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out TableRowHeight result)
		{
			result = null;
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			double value = 0.0;
			if (CssValueTypesHelper.TryConvertNonRelativeLengthValue(htmlStylePropertyValue, out value))
			{
				result = new TableRowHeight(HeightType.AtLeast, value);
			}
			else if (htmlStylePropertyValue.UnitType == null && htmlStylePropertyValue.Value.ToLowerInvariant() == "auto")
			{
				result = new TableRowHeight(HeightType.Auto);
			}
			return result != null;
		}

		public override bool ConvertBack(IHtmlExportContext context, TableRowHeight value, DocumentElementPropertiesBase properties, out string result)
		{
			result = null;
			switch (value.Type)
			{
			case HeightType.AtLeast:
			case HeightType.Exact:
				result = CssValueTypesHelper.ConvertToPixels(context, value.Value);
				break;
			}
			return !string.IsNullOrEmpty(result);
		}
	}
}
