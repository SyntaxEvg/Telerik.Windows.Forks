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
	class PaddingConverter : StringConverterBase<Padding>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out Padding result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			result = null;
			IStyleProperty styleProperty = properties.GetStyleProperty(TableCell.PaddingPropertyDefinition) ?? properties.GetStyleProperty(Table.TableCellPaddingPropertyDefinition);
			Padding padding = styleProperty.GetActualValueAsObject() as Padding;
			if (property.Name == HtmlStylePropertyDescriptors.PaddingPropertyDescriptor.Name)
			{
				double left;
				double right;
				double top;
				double bottom;
				PaddingConverter.TryGetPaddingValues(property.Values, out left, out right, out top, out bottom);
				result = new Padding(left, top, right, bottom);
				return true;
			}
			HtmlStylePropertyValue val = property.Values[0];
			double num;
			if (CssValueTypesHelper.TryConvertNonRelativeLengthValue(val, out num))
			{
				if (padding == null)
				{
					padding = new Padding(0.0);
				}
				if (property.Name == HtmlStylePropertyDescriptors.PaddingLeftPropertyDescriptor.Name)
				{
					result = PaddingConverter.SetLeft(padding, num);
				}
				else if (property.Name == HtmlStylePropertyDescriptors.PaddingRightPropertyDescriptor.Name)
				{
					result = PaddingConverter.SetRight(padding, num);
				}
				else if (property.Name == HtmlStylePropertyDescriptors.PaddingTopPropertyDescriptor.Name)
				{
					result = PaddingConverter.SetTop(padding, num);
				}
				else if (property.Name == HtmlStylePropertyDescriptors.PaddingBottomPropertyDescriptor.Name)
				{
					result = PaddingConverter.SetBottom(padding, num);
				}
				else
				{
					result = new Padding(num);
				}
				return true;
			}
			return false;
		}

		public override bool ConvertBack(IHtmlExportContext context, Padding value, DocumentElementPropertiesBase properties, out string result)
		{
			result = null;
			return false;
		}

		static void TryGetPaddingValues(HtmlStylePropertyValues values, out double left, out double right, out double top, out double bottom)
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
			switch (array.Length)
			{
			case 1:
				left = (right = (top = (bottom = array[0])));
				return;
			case 2:
				top = (bottom = array[0]);
				left = (right = array[1]);
				return;
			case 3:
				top = array[0];
				left = (right = array[1]);
				bottom = array[2];
				return;
			case 4:
				top = array[0];
				right = array[1];
				bottom = array[2];
				left = array[3];
				return;
			default:
				left = (right = (top = (bottom = 0.0)));
				return;
			}
		}

		static Padding SetLeft(Padding padding, double left)
		{
			Guard.ThrowExceptionIfNull<Padding>(padding, "padding");
			return new Padding(left, padding.Top, padding.Right, padding.Bottom);
		}

		static Padding SetRight(Padding padding, double right)
		{
			Guard.ThrowExceptionIfNull<Padding>(padding, "padding");
			return new Padding(padding.Left, padding.Top, right, padding.Bottom);
		}

		static Padding SetTop(Padding padding, double top)
		{
			Guard.ThrowExceptionIfNull<Padding>(padding, "padding");
			return new Padding(padding.Left, top, padding.Right, padding.Bottom);
		}

		static Padding SetBottom(Padding padding, double bottom)
		{
			Guard.ThrowExceptionIfNull<Padding>(padding, "padding");
			return new Padding(padding.Left, padding.Top, padding.Right, bottom);
		}
	}
}
