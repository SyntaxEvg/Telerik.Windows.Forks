using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class ThemableColorAttributes
	{
		public ThemableColorAttributes(OpenXmlAttributeBase<AutoColor> colorAttribute, OpenXmlAttributeBase<ThemeColorType> themeColorAttribute, OpenXmlAttributeBase<double> tintAttribute, OpenXmlAttributeBase<double> shadeAttribute, Color autoColorImportValue)
		{
			this.colorAttribute = colorAttribute;
			this.themeColorAttribute = themeColorAttribute;
			this.tintAttribute = tintAttribute;
			this.shadeAttribute = shadeAttribute;
			this.autoColorImportValue = autoColorImportValue;
		}

		public void FillAttributes(ThemableColor color, DocumentTheme theme)
		{
			if (color.IsFromTheme)
			{
				this.colorAttribute.Value = new AutoColor(color.GetActualValue(theme), color.IsAutomatic);
				this.themeColorAttribute.Value = color.ThemeColorType;
				if (color.TintAndShade != null && color.TintAndShade.Value != 0.0)
				{
					double value = color.TintAndShade.Value;
					if (value > 0.0)
					{
						this.tintAttribute.Value = value;
						return;
					}
					this.shadeAttribute.Value = -value;
					return;
				}
			}
			else
			{
				this.colorAttribute.Value = new AutoColor(color.LocalValue, color.IsAutomatic);
			}
		}

		public ThemableColor GetThemableColor()
		{
			ThemableColor result = null;
			if (this.themeColorAttribute.HasValue)
			{
				ThemeColorType value = this.themeColorAttribute.Value;
				double? num = null;
				if (this.tintAttribute.HasValue)
				{
					num = new double?(this.tintAttribute.Value);
				}
				else if (this.shadeAttribute.HasValue)
				{
					num = new double?(-this.shadeAttribute.Value);
				}
				if (num != null)
				{
					result = new ThemableColor(value, num.Value);
				}
				else
				{
					result = new ThemableColor(value, null);
				}
			}
			else if (this.colorAttribute.HasValue)
			{
				if (this.colorAttribute.Value.IsAutomatic)
				{
					result = new ThemableColor(this.autoColorImportValue, true);
				}
				else
				{
					result = new ThemableColor(this.colorAttribute.Value.Color, false);
				}
			}
			return result;
		}

		readonly OpenXmlAttributeBase<AutoColor> colorAttribute;

		readonly OpenXmlAttributeBase<ThemeColorType> themeColorAttribute;

		readonly OpenXmlAttributeBase<double> tintAttribute;

		readonly OpenXmlAttributeBase<double> shadeAttribute;

		readonly Color autoColorImportValue;
	}
}
