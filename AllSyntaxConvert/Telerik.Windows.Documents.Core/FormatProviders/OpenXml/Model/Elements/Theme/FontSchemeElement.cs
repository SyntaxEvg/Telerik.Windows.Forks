using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Theme
{
	class FontSchemeElement : ThemeElementBase
	{
		public FontSchemeElement(OpenXmlPartsManager partsManager)
			: base(partsManager)
		{
			this.name = base.RegisterAttribute<string>("name", true);
			this.majorFont = base.RegisterChildElement<MajorFontElement>("majorFont");
			this.minorFont = base.RegisterChildElement<MinorFontElement>("minorFont");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.DrawingMLNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "fontScheme";
			}
		}

		public string Name
		{
			get
			{
				return this.name.Value;
			}
			set
			{
				this.name.Value = value;
			}
		}

		public MajorFontElement MajorFontElement
		{
			get
			{
				return this.majorFont.Element;
			}
			set
			{
				this.majorFont.Element = value;
			}
		}

		public MinorFontElement MinorFontElement
		{
			get
			{
				return this.minorFont.Element;
			}
			set
			{
				this.minorFont.Element = value;
			}
		}

		public ThemeFontScheme CreateFontScheme(IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			string latinFontName = this.MajorFontElement.GetLatinFontName(context);
			string eastAsianFontName = this.MajorFontElement.GetEastAsianFontName(context);
			string complexScriptFontName = this.MajorFontElement.GetComplexScriptFontName(context);
			string latinFontName2 = this.MinorFontElement.GetLatinFontName(context);
			string eastAsianFontName2 = this.MinorFontElement.GetEastAsianFontName(context);
			string complexScriptFontName2 = this.MinorFontElement.GetComplexScriptFontName(context);
			base.ReleaseElement(this.majorFont);
			base.ReleaseElement(this.minorFont);
			return new ThemeFontScheme(this.Name, latinFontName, latinFontName2, eastAsianFontName, eastAsianFontName2, complexScriptFontName, complexScriptFontName2);
		}

		protected override void OnBeforeWrite(IOpenXmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlExportContext>(context, "context");
			this.Name = context.Theme.FontScheme.Name;
			base.CreateElement(this.majorFont);
			base.CreateElement(this.minorFont);
		}

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlChildElement<MajorFontElement> majorFont;

		readonly OpenXmlChildElement<MinorFontElement> minorFont;
	}
}
