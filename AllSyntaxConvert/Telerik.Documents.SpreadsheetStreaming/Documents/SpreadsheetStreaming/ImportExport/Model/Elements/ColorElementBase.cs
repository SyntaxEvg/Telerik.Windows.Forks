using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	abstract class ColorElementBase : DirectElementBase<SpreadThemableColor>
	{
		public ColorElementBase()
		{
			this.rgb = base.RegisterAttribute<ConvertedOpenXmlAttribute<UnsignedIntHex>>(new ConvertedOpenXmlAttribute<UnsignedIntHex>("rgb", Converters.UnsignedIntHexConverter, false));
			this.themeColorId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("theme", false));
			this.tint = base.RegisterAttribute<double>("tint", 0.0, false);
		}

		UnsignedIntHex Rgb
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

		int ThemeColorId
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

		double Tint
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

		protected override void InitializeAttributesOverride(SpreadThemableColor value)
		{
			if (value.IsFromTheme)
			{
				this.ThemeColorId = (int)value.ThemeColorType;
				if (value.ColorShadeType != null)
				{
					this.Tint = base.Theme.ColorScheme.GetTintAndShade(value.ThemeColorType, value.ColorShadeType.Value);
					return;
				}
				if (value.TintAndShade != null)
				{
					this.Tint = value.TintAndShade.Value;
					return;
				}
			}
			else
			{
				this.Rgb = new UnsignedIntHex(value.GetActualValue(base.Theme));
			}
		}

		protected override void CopyAttributesOverride(ref SpreadThemableColor value)
		{
			if (this.themeColorId.HasValue)
			{
				SpreadThemeColorType spreadThemeColorType = (SpreadThemeColorType)this.ThemeColorId;
				SpreadColor color = base.Theme.ColorScheme[spreadThemeColorType].Color;
				SpreadColorShadeType? colorShadeTypeForColorAndTint = ColorsHelper.GetColorShadeTypeForColorAndTint(color, this.Tint);
				if (colorShadeTypeForColorAndTint != null)
				{
					value = new SpreadThemableColor(spreadThemeColorType, colorShadeTypeForColorAndTint);
					return;
				}
				if (this.tint.HasValue)
				{
					value = new SpreadThemableColor(spreadThemeColorType, this.Tint);
					return;
				}
				value = new SpreadThemableColor(spreadThemeColorType);
				return;
			}
			else
			{
				if (this.rgb.HasValue)
				{
					value = new SpreadThemableColor(this.Rgb.Color);
					return;
				}
				value = new SpreadThemableColor(new SpreadColor(0, 0, 0));
				return;
			}
		}

		protected override void WriteChildElementsOverride(SpreadThemableColor value)
		{
		}

		readonly ConvertedOpenXmlAttribute<UnsignedIntHex> rgb;

		readonly IntOpenXmlAttribute themeColorId;

		readonly OpenXmlAttribute<double> tint;
	}
}
