using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class PaddingShortHandConverter : StringConverterBase<Padding>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out Padding result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			double left;
			double right;
			double top;
			double bottom;
			PaddingShortHandConverter.GetPaddingValues(property.Values, out left, out right, out top, out bottom);
			result = new Padding(left, top, right, bottom);
			return true;
		}

		public override bool ConvertBack(IHtmlExportContext context, Padding value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Padding>(value, "value");
			bool result2 = false;
			Padding padding = value;
			IStyleProperty styleProperty = properties.GetStyleProperty(TableCell.PaddingPropertyDefinition);
			if (styleProperty != null && !styleProperty.HasLocalValue)
			{
				TableCell tableCell = properties.OwnerDocumentElement as TableCell;
				if (tableCell != null)
				{
					if (tableCell.Row.Table.Properties.TableCellPadding.HasLocalValue)
					{
						padding = tableCell.Row.Table.TableCellPadding;
					}
					result2 = true;
				}
			}
			else
			{
				result2 = true;
			}
			result = string.Format("{0} {1} {2} {3}", new object[]
			{
				CssValueTypesHelper.ConvertToPixels(context, padding.Top),
				CssValueTypesHelper.ConvertToPixels(context, padding.Right),
				CssValueTypesHelper.ConvertToPixels(context, padding.Bottom),
				CssValueTypesHelper.ConvertToPixels(context, padding.Left)
			});
			return result2;
		}

		static void GetPaddingValues(HtmlStylePropertyValues values, out double left, out double right, out double top, out double bottom)
		{
			Guard.ThrowExceptionIfNull<HtmlStylePropertyValues>(values, "values");
			double[] pixelValues = PaddingShortHandConverter.GetPixelValues(values);
			switch (pixelValues.Length)
			{
			case 1:
				left = (right = (top = (bottom = pixelValues[0])));
				return;
			case 2:
				top = (bottom = pixelValues[0]);
				left = (right = pixelValues[1]);
				return;
			case 3:
				top = pixelValues[0];
				left = (right = pixelValues[1]);
				bottom = pixelValues[2];
				return;
			case 4:
				top = pixelValues[0];
				right = pixelValues[1];
				bottom = pixelValues[2];
				left = pixelValues[3];
				return;
			default:
				left = (right = (top = (bottom = 0.0)));
				return;
			}
		}

		static double[] GetPixelValues(HtmlStylePropertyValues values)
		{
			Guard.ThrowExceptionIfNull<HtmlStylePropertyValues>(values, "values");
			double[] array = new double[values.Count];
			for (int i = 0; i < values.Count; i++)
			{
				double num;
				if (CssValueTypesHelper.TryConvertNonRelativeLengthValue(values[i], out num))
				{
					array[i] = num;
				}
				else
				{
					array[i] = 0.0;
				}
			}
			return array;
		}
	}
}
