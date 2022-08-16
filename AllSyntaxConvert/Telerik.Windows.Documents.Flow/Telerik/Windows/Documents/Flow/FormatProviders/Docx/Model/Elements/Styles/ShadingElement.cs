using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles
{
	class ShadingElement : DocxElementBase
	{
		public ShadingElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.backgroundColorAttributes = new ThemableColorAttributes(base.RegisterAttribute<ConvertedOpenXmlAttribute<AutoColor>>(new ConvertedOpenXmlAttribute<AutoColor>("fill", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.AutoColorConverter, false)), base.RegisterAttribute<MappedOpenXmlAttribute<ThemeColorType>>(new MappedOpenXmlAttribute<ThemeColorType>("themeFill", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ThemeColorTypeMapper, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeFillTint", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeFillShade", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), Colors.Transparent);
			this.patternColorAttributes = new ThemableColorAttributes(base.RegisterAttribute<ConvertedOpenXmlAttribute<AutoColor>>(new ConvertedOpenXmlAttribute<AutoColor>("color", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.AutoColorConverter, false)), base.RegisterAttribute<MappedOpenXmlAttribute<ThemeColorType>>(new MappedOpenXmlAttribute<ThemeColorType>("themeColor", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ThemeColorTypeMapper, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeTint", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeShade", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), Colors.Transparent);
			this.shadingPatternAttribute = new MappedOpenXmlAttribute<ShadingPattern>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ShadingPatternMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<ShadingPattern>>(this.shadingPatternAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "shd";
			}
		}

		public void FillAttributes(IPropertiesWithShading properties)
		{
			DocumentTheme theme = properties.Document.Theme;
			if (properties.BackgroundColor.HasLocalValue)
			{
				ThemableColor localValue = properties.BackgroundColor.LocalValue;
				this.backgroundColorAttributes.FillAttributes(localValue, theme);
			}
			if (properties.ShadingPatternColor.HasLocalValue)
			{
				ThemableColor localValue2 = properties.ShadingPatternColor.LocalValue;
				this.patternColorAttributes.FillAttributes(localValue2, theme);
			}
			ShadingPattern value = TypeMappers.ShadingPatternMapper.DefaultToValue.Value;
			if (properties.ShadingPattern.HasLocalValue)
			{
				value = properties.ShadingPattern.LocalValue.Value;
			}
			this.shadingPatternAttribute.Value = value;
		}

		public void ReadAttributes(IPropertiesWithShading properties)
		{
			ThemableColor themableColor = this.backgroundColorAttributes.GetThemableColor();
			if (themableColor != null)
			{
				properties.BackgroundColor.LocalValue = themableColor;
			}
			ThemableColor themableColor2 = this.patternColorAttributes.GetThemableColor();
			if (themableColor2 != null)
			{
				properties.ShadingPatternColor.LocalValue = themableColor2;
			}
			properties.ShadingPattern.LocalValue = new ShadingPattern?(this.shadingPatternAttribute.Value);
		}

		readonly ThemableColorAttributes backgroundColorAttributes;

		readonly ThemableColorAttributes patternColorAttributes;

		readonly MappedOpenXmlAttribute<ShadingPattern> shadingPatternAttribute;
	}
}
