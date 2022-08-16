using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class TableBordersColorConverter : IStringConverter
	{
		public bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			result = null;
			TableBorders borders = new TableBorders();
			IStyleProperty styleProperty = properties.GetStyleProperty(Table.TableBordersPropertyDefinition);
			borders = styleProperty.GetActualValueAsObject() as TableBorders;
			ThemableColor color = new ThemableColor(Colors.Black);
			HtmlStylePropertyValue htmlStylePropertyValue = property.Values[0];
			if (htmlStylePropertyValue != null)
			{
				HtmlStyleProperty property2 = new HtmlStyleProperty("color", htmlStylePropertyValue.Value);
				if (HtmlConverters.ThemableColorConverter.Convert(context, property2, properties, out color))
				{
					result = TableBordersColorConverter.GetTableBorders(borders, color);
					return true;
				}
			}
			return false;
		}

		public bool ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result)
		{
			result = null;
			return false;
		}

		static TableBorders GetTableBorders(TableBorders borders, ThemableColor color)
		{
			Border border = borders.Left;
			Border leftBorder = new Border(border.Thickness, border.Style, color ?? border.Color, border.Shadow, border.Frame, border.Spacing);
			border = borders.Top;
			Border topBorder = new Border(border.Thickness, border.Style, color ?? border.Color, border.Shadow, border.Frame, border.Spacing);
			border = borders.Right;
			Border rightBorder = new Border(border.Thickness, border.Style, color ?? border.Color, border.Shadow, border.Frame, border.Spacing);
			border = borders.Bottom;
			Border bottomBorder = new Border(border.Thickness, border.Style, color ?? border.Color, border.Shadow, border.Frame, border.Spacing);
			return new TableBorders(leftBorder, topBorder, rightBorder, bottomBorder);
		}
	}
}
