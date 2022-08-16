using System;
using System.Globalization;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.RunPropertiesElements
{
	class FontElement : DocxElementBase
	{
		public FontElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.asciiAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("ascii", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.highAnsiAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("hAnsi", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.complexScriptAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("cs", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.asciiThemeAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("asciiTheme", OpenXmlNamespaces.WordprocessingMLNamespace, false));
			this.highAnsiThemeAttribute = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("hAnsiTheme", OpenXmlNamespaces.WordprocessingMLNamespace, false));
		}

		public override string ElementName
		{
			get
			{
				return "rFonts";
			}
		}

		public void FillAttributes(ThemableFontFamily fontFamily, DocumentTheme theme)
		{
			FontFamily actualValue = fontFamily.GetActualValue(theme);
			this.asciiAttribute.Value = actualValue.Source;
			this.highAnsiAttribute.Value = actualValue.Source;
			this.complexScriptAttribute.Value = actualValue.Source;
			if (fontFamily.IsFromTheme)
			{
				string value = fontFamily.ThemeFontType.ToString().ToLower(CultureInfo.InvariantCulture) + "HAnsi";
				this.asciiThemeAttribute.Value = value;
				this.highAnsiThemeAttribute.Value = value;
			}
		}

		public ThemableFontFamily GetFontFamily()
		{
			ThemableFontFamily result = null;
			if (this.asciiThemeAttribute.HasValue)
			{
				if (this.asciiThemeAttribute.Value.StartsWith("minor", StringComparison.OrdinalIgnoreCase))
				{
					result = new ThemableFontFamily(ThemeFontType.Minor);
				}
				else
				{
					result = new ThemableFontFamily(ThemeFontType.Major);
				}
			}
			else if (this.asciiAttribute.HasValue)
			{
				result = new ThemableFontFamily(this.asciiAttribute.Value);
			}
			return result;
		}

		readonly OpenXmlAttribute<string> asciiAttribute;

		readonly OpenXmlAttribute<string> highAnsiAttribute;

		readonly OpenXmlAttribute<string> complexScriptAttribute;

		readonly OpenXmlAttribute<string> asciiThemeAttribute;

		readonly OpenXmlAttribute<string> highAnsiThemeAttribute;
	}
}
