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

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class UnderlineElement : DocxElementBase
	{
		public UnderlineElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.colorAttributes = new ThemableColorAttributes(base.RegisterAttribute<ConvertedOpenXmlAttribute<AutoColor>>(new ConvertedOpenXmlAttribute<AutoColor>("color", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.AutoColorConverter, false)), base.RegisterAttribute<MappedOpenXmlAttribute<ThemeColorType>>(new MappedOpenXmlAttribute<ThemeColorType>("themeColor", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ThemeColorTypeMapper, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeTint", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeShade", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), Colors.Black);
			this.patternAttribute = new MappedOpenXmlAttribute<UnderlinePattern>("val", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.UnderlinePatternMapper, true);
			base.RegisterAttribute<MappedOpenXmlAttribute<UnderlinePattern>>(this.patternAttribute);
		}

		public override string ElementName
		{
			get
			{
				return "u";
			}
		}

		public void FillAttributes(CharacterProperties characterProperties, DocumentTheme theme)
		{
			if (characterProperties.UnderlineColor.HasLocalValue)
			{
				ThemableColor localValue = characterProperties.UnderlineColor.LocalValue;
				this.colorAttributes.FillAttributes(localValue, theme);
			}
			if (characterProperties.UnderlinePattern.HasLocalValue)
			{
				this.patternAttribute.Value = characterProperties.UnderlinePattern.LocalValue.Value;
			}
		}

		public void ReadAttributes(CharacterProperties characterProperties)
		{
			ThemableColor themableColor = this.colorAttributes.GetThemableColor();
			if (themableColor != null)
			{
				characterProperties.UnderlineColor.LocalValue = themableColor;
			}
			if (this.patternAttribute.HasValue)
			{
				characterProperties.UnderlinePattern.LocalValue = new UnderlinePattern?(this.patternAttribute.Value);
			}
		}

		readonly ThemableColorAttributes colorAttributes;

		readonly MappedOpenXmlAttribute<UnderlinePattern> patternAttribute;
	}
}
