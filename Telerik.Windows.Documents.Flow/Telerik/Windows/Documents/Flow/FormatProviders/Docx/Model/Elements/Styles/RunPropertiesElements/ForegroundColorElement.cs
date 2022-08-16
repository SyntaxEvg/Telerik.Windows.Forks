using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class ForegroundColorElement : DocxElementBase
	{
		public ForegroundColorElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.colorAttributes = new ThemableColorAttributes(base.RegisterAttribute<ConvertedOpenXmlAttribute<AutoColor>>(new ConvertedOpenXmlAttribute<AutoColor>("val", OpenXmlNamespaces.WordprocessingMLNamespace, DocxConverters.AutoColorConverter, false)), base.RegisterAttribute<MappedOpenXmlAttribute<ThemeColorType>>(new MappedOpenXmlAttribute<ThemeColorType>("themeColor", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ThemeColorTypeMapper, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeTint", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), base.RegisterAttribute<ConvertedOpenXmlAttribute<double>>(new ConvertedOpenXmlAttribute<double>("themeShade", OpenXmlNamespaces.WordprocessingMLNamespace, Converters.HexToTintConverter, false)), Colors.Black);
		}

		public override string ElementName
		{
			get
			{
				return "color";
			}
		}

		public void FillAttributes(ThemableColor color, DocumentTheme theme)
		{
			this.colorAttributes.FillAttributes(color, theme);
		}

		public ThemableColor GetThemableColor()
		{
			return this.colorAttributes.GetThemableColor();
		}

		readonly ThemableColorAttributes colorAttributes;
	}
}
