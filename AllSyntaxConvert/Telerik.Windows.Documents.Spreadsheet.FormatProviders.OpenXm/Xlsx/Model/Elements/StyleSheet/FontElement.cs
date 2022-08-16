using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class FontElement : StyleSheetElementBase
	{
		public FontElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.bold = base.RegisterChildElement<BoldElement>("b");
			this.italic = base.RegisterChildElement<ItalicElement>("i");
			this.underline = base.RegisterChildElement<UnderlineElement>("u");
			this.vertAlign = base.RegisterChildElement<VerticalAlignmentElement>("vertAlign");
			this.fontSize = base.RegisterChildElement<FontSizeElement>("sz");
			this.color = base.RegisterChildElement<ColorElement>("color");
			this.name = base.RegisterChildElement<FontNameElement>("name");
			this.family = base.RegisterChildElement<FontFamilyElement>("family");
			this.scheme = base.RegisterChildElement<SchemeElement>("scheme");
		}

		public override string ElementName
		{
			get
			{
				return "font";
			}
		}

		public FontInfo? FontInfo
		{
			get
			{
				return this.fontInfo;
			}
		}

		public FontNameElement NameElement
		{
			get
			{
				return this.name.Element;
			}
			set
			{
				this.name.Element = value;
			}
		}

		public FontFamilyElement FamilyElement
		{
			get
			{
				return this.family.Element;
			}
			set
			{
				this.family.Element = value;
			}
		}

		public BoldElement BoldElement
		{
			get
			{
				return this.bold.Element;
			}
			set
			{
				this.bold.Element = value;
			}
		}

		public ItalicElement ItalicElement
		{
			get
			{
				return this.italic.Element;
			}
			set
			{
				this.italic.Element = value;
			}
		}

		public ColorElement ColorElement
		{
			get
			{
				return this.color.Element;
			}
			set
			{
				this.color.Element = value;
			}
		}

		public FontSizeElement FontSizeElement
		{
			get
			{
				return this.fontSize.Element;
			}
			set
			{
				this.fontSize.Element = value;
			}
		}

		public UnderlineElement UnderlineElement
		{
			get
			{
				return this.underline.Element;
			}
			set
			{
				this.underline.Element = value;
			}
		}

		public VerticalAlignmentElement VertAlignElement
		{
			get
			{
				return this.vertAlign.Element;
			}
			set
			{
				this.vertAlign.Element = value;
			}
		}

		public SchemeElement SchemeElement
		{
			get
			{
				return this.scheme.Element;
			}
			set
			{
				this.scheme.Element = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, FontInfo font)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<FontInfo>(font, "font");
			if (font.Bold != null)
			{
				base.CreateElement(this.bold);
				this.BoldElement.Value = font.Bold.Value;
			}
			if (font.Italic != null)
			{
				base.CreateElement(this.italic);
				this.ItalicElement.Value = font.Italic.Value;
			}
			if (font.ForeColor != null)
			{
				base.CreateElement(this.color);
				this.ColorElement.CopyPropertiesFrom(context, font.ForeColor);
			}
			if (font.FontSize != null)
			{
				base.CreateElement(this.fontSize);
				this.FontSizeElement.Value = UnitHelper.DipToUnit(font.FontSize.Value, UnitType.Point);
			}
			if (font.FontFamily != null)
			{
				base.CreateElement(this.name);
				this.NameElement.Value = font.FontFamily.GetActualValue(context.Theme).Source;
				if (font.FontFamily.IsFromTheme)
				{
					base.CreateElement(this.scheme);
					this.SchemeElement.Value = FontSchemes.GetFontSchemeName(font.FontFamily.ThemeFontType);
				}
			}
			if (font.UnderlineType != null)
			{
				base.CreateElement(this.underline);
				this.UnderlineElement.Value = UnderlineValues.GetUnderlineValueName(font.UnderlineType.Value);
			}
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			this.fontInfo = new FontInfo?(this.GetFontInfo(context));
			base.ReleaseElement(this.bold);
			base.ReleaseElement(this.italic);
			base.ReleaseElement(this.color);
			base.ReleaseElement(this.fontSize);
			base.ReleaseElement(this.scheme);
			base.ReleaseElement(this.name);
			base.ReleaseElement(this.underline);
		}

		protected override void ClearOverride()
		{
			this.fontInfo = null;
		}

		FontInfo GetFontInfo(IXlsxWorkbookImportContext context)
		{
			FontInfo result = default(FontInfo);
			if (this.BoldElement != null)
			{
				result.Bold = new bool?(this.BoldElement.Value);
			}
			if (this.ItalicElement != null)
			{
				result.Italic = new bool?(this.ItalicElement.Value);
			}
			if (this.ColorElement != null)
			{
				result.ForeColor = this.ColorElement.CreateThemableColor(context);
			}
			if (this.FontSizeElement != null)
			{
				result.FontSize = new double?(UnitHelper.UnitToDip(this.FontSizeElement.Value, UnitType.Point));
			}
			if (this.SchemeElement != null)
			{
				ThemeFontType? fontScheme = FontSchemes.GetFontScheme(this.SchemeElement.Value);
				if (fontScheme != null)
				{
					result.FontFamily = new ThemableFontFamily(fontScheme.Value);
				}
			}
			else if (this.NameElement != null)
			{
				result.FontFamily = new ThemableFontFamily(this.NameElement.Value);
			}
			if (this.UnderlineElement != null)
			{
				result.UnderlineType = new UnderlineType?(UnderlineValues.GetUnderlineValue(this.UnderlineElement.Value));
			}
			return result;
		}

		readonly OpenXmlChildElement<BoldElement> bold;

		readonly OpenXmlChildElement<FontFamilyElement> family;

		readonly OpenXmlChildElement<ColorElement> color;

		readonly OpenXmlChildElement<ItalicElement> italic;

		readonly OpenXmlChildElement<FontNameElement> name;

		readonly OpenXmlChildElement<SchemeElement> scheme;

		readonly OpenXmlChildElement<FontSizeElement> fontSize;

		readonly OpenXmlChildElement<UnderlineElement> underline;

		readonly OpenXmlChildElement<VerticalAlignmentElement> vertAlign;

		FontInfo? fontInfo;
	}
}
