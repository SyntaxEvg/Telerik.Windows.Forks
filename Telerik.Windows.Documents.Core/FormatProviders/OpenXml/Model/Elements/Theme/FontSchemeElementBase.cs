using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	abstract class FontSchemeElementBase : ThemeElementBase
	{
		public FontSchemeElementBase(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.latinFont = base.RegisterChildElement<LatinFontElement>("latin");
			this.eastAsianFont = base.RegisterChildElement<EastAsianFontElement>("ea");
			this.complexScriptFont = base.RegisterChildElement<ComplexScriptFontElement>("cs");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public LatinFontElement LatinFontElement
		{
			get
			{
				return this.latinFont.Element;
			}
			set
			{
				this.latinFont.Element = value;
			}
		}

		public EastAsianFontElement EastAsianFontElement
		{
			get
			{
				return this.eastAsianFont.Element;
			}
			set
			{
				this.eastAsianFont.Element = value;
			}
		}

		public ComplexScriptFontElement ComplexScriptFontElement
		{
			get
			{
				return this.complexScriptFont.Element;
			}
			set
			{
				this.complexScriptFont.Element = value;
			}
		}

		protected abstract ThemeFontType ThemeFontType { get; }

		public string GetLatinFontName(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			string fontName = this.LatinFontElement.GetFontName(context);
			base.ReleaseElement(this.latinFont);
			return fontName;
		}

		public string GetEastAsianFontName(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			string fontName = this.EastAsianFontElement.GetFontName(context);
			base.ReleaseElement(this.eastAsianFont);
			return fontName;
		}

		public string GetComplexScriptFontName(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			string fontName = this.ComplexScriptFontElement.GetFontName(context);
			base.ReleaseElement(this.complexScriptFont);
			return fontName;
		}

		protected override OpenXmlElementBase CreateElement(string elementName)
		{
			if (elementName == "font")
			{
				return null;
			}
			return base.CreateElement(elementName);
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			base.CreateElement(this.latinFont);
			base.CreateElement(this.eastAsianFont);
			base.CreateElement(this.complexScriptFont);
			this.LatinFontElement.SetFontName(context.Theme.FontScheme[this.ThemeFontType][FontLanguageType.Latin]);
			this.EastAsianFontElement.SetFontName(context.Theme.FontScheme[this.ThemeFontType][FontLanguageType.EastAsian]);
			this.ComplexScriptFontElement.SetFontName(context.Theme.FontScheme[this.ThemeFontType][FontLanguageType.ComplexScript]);
		}

		readonly OpenXmlChildElement<LatinFontElement> latinFont;

		readonly OpenXmlChildElement<EastAsianFontElement> eastAsianFont;

		readonly OpenXmlChildElement<ComplexScriptFontElement> complexScriptFont;
	}
}
