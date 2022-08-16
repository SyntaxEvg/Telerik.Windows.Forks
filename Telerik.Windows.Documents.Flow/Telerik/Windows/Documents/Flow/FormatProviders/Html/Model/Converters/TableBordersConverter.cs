using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class TableBordersConverter : IStringConverter
	{
		public bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out object result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			result = null;
			TableBorders tableBorders = new TableBorders();
			IStyleProperty styleProperty = properties.GetStyleProperty(Table.TableBordersPropertyDefinition);
			tableBorders = styleProperty.GetActualValueAsObject() as TableBorders;
			Border border;
			if (!HtmlConverters.BorderConverter.Convert(context, property, properties, out border))
			{
				return false;
			}
			if (property.Name == HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor.Name)
			{
				tableBorders = new TableBorders(tableBorders, border, null, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor.Name)
			{
				tableBorders = new TableBorders(tableBorders, null, border, null, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor.Name)
			{
				tableBorders = new TableBorders(tableBorders, null, null, border, null, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor.Name)
			{
				tableBorders = new TableBorders(tableBorders, null, null, null, border, null, null);
			}
			else if (property.Name == HtmlStylePropertyDescriptors.BorderPropertyDescriptor.Name)
			{
				tableBorders = new TableBorders(border, border, border, border);
			}
			result = tableBorders;
			return true;
		}

		public bool ConvertBack(IHtmlExportContext context, object value, HtmlStylePropertyDescriptor descriptor, DocumentElementPropertiesBase properties, out string result)
		{
			TableBorders tableBorders = value as TableBorders;
			result = null;
			bool flag = tableBorders.Left.Equals(tableBorders.Top) && tableBorders.Left.Equals(tableBorders.Right) && tableBorders.Left.Equals(tableBorders.Bottom);
			if (flag)
			{
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, tableBorders.Left, properties, out result);
				}
			}
			else
			{
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, tableBorders.Left, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, tableBorders.Top, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, tableBorders.Right, properties, out result);
				}
				if (descriptor.Name == HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor.Name)
				{
					return HtmlConverters.BorderConverter.ConvertBack(context, tableBorders.Bottom, properties, out result);
				}
			}
			return false;
		}
	}
}
