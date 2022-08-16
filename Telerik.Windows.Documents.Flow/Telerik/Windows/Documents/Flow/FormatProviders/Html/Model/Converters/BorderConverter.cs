using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	class BorderConverter : StringConverterBase<Border>
	{
		public override bool Convert(IHtmlImportContext context, HtmlStyleProperty property, DocumentElementPropertiesBase properties, out Border result)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			int num = 0;
			result = null;
			if (property.Values.Count == 0)
			{
				return false;
			}
			double thickness = 1.0;
			bool flag = false;
			for (int i = 0; i < property.Values.Count; i++)
			{
				HtmlStylePropertyValue borderWidth = property.Values[i];
				if (BorderConverter.ParseThickness(borderWidth, out thickness))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				num++;
			}
			else
			{
				thickness = 1.0;
			}
			BorderStyle style = BorderStyle.Single;
			bool flag2 = false;
			for (int j = 0; j < property.Values.Count; j++)
			{
				HtmlStylePropertyValue borderStyle = property.Values[j];
				if (BorderConverter.ParseStyle(borderStyle, out style))
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				num++;
			}
			else
			{
				style = BorderStyle.Single;
			}
			ThemableColor defaultBorderColor = BorderConverter.DefaultBorderColor;
			if (num == property.Values.Count - 1)
			{
				bool flag3 = false;
				for (int k = 0; k < property.Values.Count; k++)
				{
					HtmlStylePropertyValue borderColor = property.Values[k];
					if (BorderConverter.ParseColor(borderColor, context, properties, out defaultBorderColor))
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					result = null;
					return false;
				}
			}
			result = new Border(thickness, style, defaultBorderColor);
			return true;
		}

		public override bool ConvertBack(IHtmlExportContext context, Border value, DocumentElementPropertiesBase properties, out string result)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<DocumentElementPropertiesBase>(properties, "properties");
			Guard.ThrowExceptionIfNull<Border>(value, "property");
			result = null;
			if (value.Style == BorderStyle.Inherit)
			{
				return false;
			}
			double value2 = Math.Max(value.Thickness, context.Settings.BordersMinimalThickness);
			string text = CssValueTypesHelper.ConvertToPixels(context, value2);
			string value3;
			if (!HtmlValueMappers.BorderStyleValueMapper.TryGetFromValue(value.Style, out value3))
			{
				value3 = HtmlValueMappers.BorderStyleValueMapper.DefaultFromValue.Value;
			}
			string text2;
			if (!HtmlConverters.ThemableColorConverter.ConvertBack(context, value.Color, properties, out text2))
			{
				return false;
			}
			result = string.Concat(new string[] { text, " ", value3, " ", text2 });
			return true;
		}

		static bool ParseThickness(HtmlStylePropertyValue borderWidth, out double thickness)
		{
			return CssValueTypesHelper.TryConvertNonRelativeLengthValue(borderWidth, out thickness);
		}

		static bool ParseColor(HtmlStylePropertyValue borderColor, IHtmlImportContext context, DocumentElementPropertiesBase properties, out ThemableColor color)
		{
			HtmlStyleProperty property = new HtmlStyleProperty("color", borderColor.Value);
			return HtmlConverters.ThemableColorConverter.Convert(context, property, properties, out color);
		}

		static bool ParseStyle(HtmlStylePropertyValue borderStyle, out BorderStyle style)
		{
			return HtmlValueMappers.BorderStyleValueMapper.TryGetToValue(borderStyle.Value, out style);
		}

		static readonly ThemableColor DefaultBorderColor = new ThemableColor(Colors.Black);
	}
}
