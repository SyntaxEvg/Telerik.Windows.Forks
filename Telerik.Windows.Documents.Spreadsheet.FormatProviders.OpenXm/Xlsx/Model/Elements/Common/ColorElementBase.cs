using System;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	abstract class ColorElementBase : XlsxElementBase
	{
		public ColorElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.indexed = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("indexed", false));
			this.rgb = base.RegisterAttribute<ConvertedOpenXmlAttribute<UnsignedIntHex>>(new ConvertedOpenXmlAttribute<UnsignedIntHex>("rgb", Converters.UnsignedIntHexConverter, false));
			this.themeColorId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("theme", false));
			this.tint = base.RegisterAttribute<double>("tint", 0.0, false);
		}

		public int Indexed
		{
			get
			{
				return this.indexed.Value;
			}
			set
			{
				this.indexed.Value = value;
			}
		}

		public UnsignedIntHex Rgb
		{
			get
			{
				return this.rgb.Value;
			}
			set
			{
				this.rgb.Value = value;
			}
		}

		public int ThemeColorId
		{
			get
			{
				return this.themeColorId.Value;
			}
			set
			{
				this.themeColorId.Value = value;
			}
		}

		public double Tint
		{
			get
			{
				return this.tint.Value;
			}
			set
			{
				this.tint.Value = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, ThemableColor color)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ThemableColor>(color, "border");
			if (color.IsFromTheme)
			{
				this.ThemeColorId = (int)color.ThemeColorType;
				if (color.ColorShadeType != null)
				{
					this.Tint = context.Theme.ColorScheme.GetTintAndShade(color.ThemeColorType, color.ColorShadeType.Value);
					return;
				}
				if (color.TintAndShade != null)
				{
					this.Tint = color.TintAndShade.Value;
					return;
				}
			}
			else
			{
				this.Rgb = new UnsignedIntHex(color.GetActualValue(context.Theme));
			}
		}

		public ThemableColor CreateThemableColor(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			if (this.themeColorId.HasValue)
			{
				ThemeColorType themeColorTypeByIndex = ThemeColorTypes.GetThemeColorTypeByIndex(this.ThemeColorId);
				Color color = context.Theme.ColorScheme[themeColorTypeByIndex].Color;
				ColorShadeType? colorShadeTypeForColorAndTint = ColorsHelper.GetColorShadeTypeForColorAndTint(color, this.Tint);
				if (colorShadeTypeForColorAndTint != null)
				{
					return new ThemableColor(themeColorTypeByIndex, colorShadeTypeForColorAndTint);
				}
				if (this.tint.HasValue)
				{
					return new ThemableColor(themeColorTypeByIndex, this.Tint);
				}
				return new ThemableColor(themeColorTypeByIndex, null);
			}
			else
			{
				if (this.rgb.HasValue)
				{
					return new ThemableColor(this.Rgb.Color);
				}
				if (this.indexed.HasValue)
				{
					ResourceIndexedTable<Color> indexedColorTable = context.StyleSheet.IndexedColorTable;
					int num = indexedColorTable.Count<Color>();
					int num2 = this.Indexed;
					if (num2 < 0 || num2 >= num)
					{
						num2 = ((num2 % 2 == 0) ? (num - 2) : (num - 1));
					}
					return new ThemableColor(context.StyleSheet.IndexedColorTable[num2]);
				}
				return new ThemableColor(Colors.Black);
			}
		}

		readonly IntOpenXmlAttribute indexed;

		readonly ConvertedOpenXmlAttribute<UnsignedIntHex> rgb;

		readonly IntOpenXmlAttribute themeColorId;

		readonly OpenXmlAttribute<double> tint;
	}
}
