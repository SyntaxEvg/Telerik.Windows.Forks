using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class ThemeElementsElement : ThemeElementBase
	{
		public ThemeElementsElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.colorScheme = base.RegisterChildElement<ColorSchemeElement>("clrScheme");
			this.fontScheme = base.RegisterChildElement<FontSchemeElement>("fontScheme");
			this.formatScheme = base.RegisterChildElement<FormatSchemeElement>("fmtScheme");
		}

		public override string ElementName
		{
			get
			{
				return "themeElements";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public ColorSchemeElement ColorSchemeElement
		{
			get
			{
				return this.colorScheme.Element;
			}
			set
			{
				this.colorScheme.Element = value;
			}
		}

		public FontSchemeElement FontSchemeElement
		{
			get
			{
				return this.fontScheme.Element;
			}
			set
			{
				this.fontScheme.Element = value;
			}
		}

		public ThemeColorScheme CreateColorScheme(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			ThemeColorScheme result = this.ColorSchemeElement.CreateColorScheme(context);
			base.ReleaseElement(this.colorScheme);
			return result;
		}

		public ThemeFontScheme CreateFontScheme(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			ThemeFontScheme result = this.FontSchemeElement.CreateFontScheme(context);
			base.ReleaseElement(this.fontScheme);
			return result;
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			base.CreateElement(this.colorScheme);
			base.CreateElement(this.fontScheme);
			base.CreateElement(this.formatScheme);
		}

		protected override void OnAfterRead(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			base.ReleaseElement(this.formatScheme);
		}

		readonly OpenXmlChildElement<ColorSchemeElement> colorScheme;

		readonly OpenXmlChildElement<FontSchemeElement> fontScheme;

		readonly OpenXmlChildElement<FormatSchemeElement> formatScheme;
	}
}
