using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class StyleSheetElement : XlsxPartRootElementBase
	{
		public StyleSheetElement(XlsxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.numberFormats = base.RegisterChildElement<NumberFormatsElement>("numFmts");
			this.fonts = (this.fonts = base.RegisterChildElement<FontsElement>("fonts"));
			this.fills = base.RegisterChildElement<FillsElement>("fills");
			this.borders = base.RegisterChildElement<BordersElement>("borders");
			this.cellStyleFormats = base.RegisterChildElement<CellStyleFormatsElement>("cellStyleXfs");
			this.cellFormats = base.RegisterChildElement<CellFormatsElement>("cellXfs");
			this.cellStyles = base.RegisterChildElement<CellStylesElement>("cellStyles");
			this.differentialFormats = base.RegisterChildElement<DifferentialFormatsElement>("dxfs");
			this.colors = base.RegisterChildElement<ColorsElement>("colors");
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public override string ElementName
		{
			get
			{
				return "styleSheet";
			}
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			base.CreateElement(this.numberFormats);
			base.CreateElement(this.fonts);
			base.CreateElement(this.fills);
			base.CreateElement(this.borders);
			base.CreateElement(this.cellStyleFormats);
			base.CreateElement(this.cellFormats);
			base.CreateElement(this.cellStyles);
			base.CreateElement(this.differentialFormats);
			base.CreateElement(this.colors);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			base.ReleaseElement(this.numberFormats);
			base.ReleaseElement(this.fonts);
			base.ReleaseElement(this.fills);
			base.ReleaseElement(this.borders);
			base.ReleaseElement(this.cellStyleFormats);
			base.ReleaseElement(this.cellFormats);
			base.ReleaseElement(this.cellStyles);
			base.ReleaseElement(this.differentialFormats);
			base.ReleaseElement(this.colors);
			context.ImportStyles();
		}

		readonly OpenXmlChildElement<BordersElement> borders;

		readonly OpenXmlChildElement<CellStylesElement> cellStyles;

		readonly OpenXmlChildElement<CellStyleFormatsElement> cellStyleFormats;

		readonly OpenXmlChildElement<CellFormatsElement> cellFormats;

		readonly OpenXmlChildElement<ColorsElement> colors;

		readonly OpenXmlChildElement<FillsElement> fills;

		readonly OpenXmlChildElement<FontsElement> fonts;

		readonly OpenXmlChildElement<NumberFormatsElement> numberFormats;

		readonly OpenXmlChildElement<DifferentialFormatsElement> differentialFormats;
	}
}
