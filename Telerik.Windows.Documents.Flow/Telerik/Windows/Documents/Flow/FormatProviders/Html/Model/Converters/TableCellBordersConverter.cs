using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.BorderEvaluation;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class TableCellBordersConverter : IStringConverter
	{
		public bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			result = null;
			TableCellBorders tableCellBorders = new TableCellBorders();
			IStyleProperty styleProperty = properties.GetStyleProperty(TableCell.BordersPropertyDefinition);
			tableCellBorders = styleProperty.GetActualValueAsObject() as TableCellBorders;
			Border border;
			if (!HtmlConverters.BorderConverter.Convert(context, property, properties, out border))
			{
				return false;
			}
			if (property.Name == HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor.Name)
			{
				tableCellBorders = new TableCellBorders(tableCellBorders, border, null, null, null, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor.Name)
			{
				tableCellBorders = new TableCellBorders(tableCellBorders, null, border, null, null, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor.Name)
			{
				tableCellBorders = new TableCellBorders(tableCellBorders, null, null, border, null, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor.Name)
			{
				tableCellBorders = new TableCellBorders(tableCellBorders, null, null, null, border, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderPropertyDescriptor.Name)
			{
				tableCellBorders = new TableCellBorders(border);
			}
			result = tableCellBorders;
			return true;
		}

		public bool ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result)
		{
			TableCellBorders tableCellBorders = value as TableCellBorders;
			result = null;
			Border border = tableCellBorders.Left;
			Border border2 = tableCellBorders.Right;
			Border border3 = tableCellBorders.Top;
			Border border4 = tableCellBorders.Bottom;
			TableCell tableCell = properties.OwnerDocumentElement as TableCell;
			if (tableCell != null)
			{
				Table table = tableCell.Row.Table;
				if (table.HasCellSpacing)
				{
					if (border.Style == BorderStyle.Inherit)
					{
						border = table.Borders.InsideVertical;
					}
					if (border2.Style == BorderStyle.Inherit)
					{
						border2 = table.Borders.InsideVertical;
					}
					if (border3.Style == BorderStyle.Inherit)
					{
						border3 = table.Borders.InsideHorizontal;
					}
					if (border4.Style == BorderStyle.Inherit)
					{
						border4 = table.Borders.InsideHorizontal;
					}
				}
				else
				{
					TableBorderGrid borderGrid = context.GetBorderGrid(table);
					border3 = borderGrid.GetTopHorizontalBorder(tableCell.GridRowIndex, tableCell.GridColumnIndex).Border;
					border4 = borderGrid.GetTopHorizontalBorder(tableCell.GridRowIndex + tableCell.RowSpan, tableCell.GridColumnIndex).Border;
					border = borderGrid.GetLeftVerticalBorder(tableCell.GridRowIndex, tableCell.GridColumnIndex).Border;
					border2 = borderGrid.GetLeftVerticalBorder(tableCell.GridRowIndex, tableCell.GridColumnIndex + tableCell.ColumnSpan).Border;
				}
			}
			bool flag = border.Equals(border3) && border.Equals(border2) && border.Equals(border4);
			if (flag)
			{
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, border, properties, out result);
				}
			}
			else
			{
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, border, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, border3, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, border2, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, border4, properties, out result);
				}
			}
			return false;
		}
	}
}
