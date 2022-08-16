using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements.Drawing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model
{
	static class XlsxElementsFactory
	{
		static XlsxElementsFactory()
		{
			XlsxElementsFactory.RegisterFactoryMethod("si", (XlsxPartsManager pm) => new StringItemElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("t", (XlsxPartsManager pm) => new TextElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("r", (XlsxPartsManager pm) => new RichTextRunElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("x:t", (XlsxPartsManager pm) => new TextElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("x:r", (XlsxPartsManager pm) => new RichTextRunElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("is", (XlsxPartsManager pm) => new RichTextInlineElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("u", (XlsxPartsManager pm) => new UnderlineElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("vertAlign", (XlsxPartsManager pm) => new VerticalAlignmentElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("alignment", (XlsxPartsManager pm) => new AlignmentElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("protection", (XlsxPartsManager pm) => new ProtectionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("bgColor", (XlsxPartsManager pm) => new BackgroundColorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("b", (XlsxPartsManager pm) => new BoldElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("border", (XlsxPartsManager pm) => new BorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("left", (XlsxPartsManager pm) => new LeftBorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("right", (XlsxPartsManager pm) => new RightBorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("top", (XlsxPartsManager pm) => new TopBorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("bottom", (XlsxPartsManager pm) => new BottomBorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("diagonal", (XlsxPartsManager pm) => new DiagonalBorderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("borders", (XlsxPartsManager pm) => new BordersElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("color", (XlsxPartsManager pm) => new ColorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("cellXfs", (XlsxPartsManager pm) => new CellFormatsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("xf", (XlsxPartsManager pm) => new FormatElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("cellStyle", (XlsxPartsManager pm) => new CellStyleElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("cellStyleXfs", (XlsxPartsManager pm) => new CellStyleFormatsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("cellStyles", (XlsxPartsManager pm) => new CellStylesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("indexedColors", (XlsxPartsManager pm) => new IndexedColorsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("gradientFill", (XlsxPartsManager pm) => new GradientFillElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("patternFill", (XlsxPartsManager pm) => new PatternFillElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("fill", (XlsxPartsManager pm) => new FillElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("fills", (XlsxPartsManager pm) => new FillsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("name", (XlsxPartsManager pm) => new FontNameElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("family", (XlsxPartsManager pm) => new FontFamilyElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("i", (XlsxPartsManager pm) => new ItalicElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sz", (XlsxPartsManager pm) => new FontSizeElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("scheme", (XlsxPartsManager pm) => new SchemeElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("font", (XlsxPartsManager pm) => new FontElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("fonts", (XlsxPartsManager pm) => new FontsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("fgColor", (XlsxPartsManager pm) => new ForegroundColorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("stop", (XlsxPartsManager pm) => new StopElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("rgbColor", (XlsxPartsManager pm) => new RgbColorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("numFmt", (XlsxPartsManager pm) => new NumberFormatElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("x:numFmt", (XlsxPartsManager pm) => new NumberFormatElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("numFmts", (XlsxPartsManager pm) => new NumberFormatsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("colors", (XlsxPartsManager pm) => new ColorsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheet", (XlsxPartsManager pm) => new SheetElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheets", (XlsxPartsManager pm) => new SheetsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("bookViews", (XlsxPartsManager pm) => new BookViewsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("workbookView", (XlsxPartsManager pm) => new WorkbookViewElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("f", (XlsxPartsManager pm) => new FormulaElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("x:f", (XlsxPartsManager pm) => new FormulaElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("definedNames", (XlsxPartsManager pm) => new DefinedNamesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("definedName", (XlsxPartsManager pm) => new DefinedNameElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("hyperlinks", (XlsxPartsManager pm) => new HyperlinksElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("hyperlink", (XlsxPartsManager pm) => new HyperlinkElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("v", (XlsxPartsManager pm) => new CellValueElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("x:v", (XlsxPartsManager pm) => new CellValueElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("col", (XlsxPartsManager pm) => new ColumnElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("cols", (XlsxPartsManager pm) => new ColumnsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("mergeCell", (XlsxPartsManager pm) => new MergedCellElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("mergeCells", (XlsxPartsManager pm) => new MergedCellsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("row", (XlsxPartsManager pm) => new RowElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pane", (XlsxPartsManager pm) => new PaneElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("selection", (XlsxPartsManager pm) => new SelectionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetView", (XlsxPartsManager pm) => new SheetViewElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetViews", (XlsxPartsManager pm) => new SheetViewsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetData", (XlsxPartsManager pm) => new SheetDataElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetProtection", (XlsxPartsManager pm) => new SheetProtectionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("workbookProtection", (XlsxPartsManager pm) => new WorkbookProtectionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetFormatPr", (XlsxPartsManager pm) => new SheetFormatPropertiesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("c", (XlsxPartsManager pm) => new CellElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dxfs", (XlsxPartsManager pm) => new DifferentialFormatsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dxf", (XlsxPartsManager pm) => new DifferentialFormatElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dimension", (XlsxPartsManager pm) => new DimensionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sqref", (XlsxPartsManager pm) => new SqRefElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sheetPr", (XlsxPartsManager pm) => new SheetPropertiesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("outlinePr", (XlsxPartsManager pm) => new OutlinePropertiesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pageSetUpPr", (XlsxPartsManager pm) => new PageSetUpPropertiesElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("printOptions", (XlsxPartsManager pm) => new PrintOptionsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pageMargins", (XlsxPartsManager pm) => new PageMarginsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pageSetup", (XlsxPartsManager pm) => new PageSetupElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("rowBreaks", (XlsxPartsManager pm) => new RowBreaksElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("colBreaks", (XlsxPartsManager pm) => new ColumnBreaksElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("brk", (XlsxPartsManager pm) => new BreakElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("headerFooter", (XlsxPartsManager pm) => new HeaderFooterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("oddHeader", (XlsxPartsManager pm) => new OddHeaderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("oddFooter", (XlsxPartsManager pm) => new OddFooterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("evenHeader", (XlsxPartsManager pm) => new EvenHeaderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("evenFooter", (XlsxPartsManager pm) => new EvenFooterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("firstHeader", (XlsxPartsManager pm) => new FirstHeaderElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("firstFooter", (XlsxPartsManager pm) => new FirstFooterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("autoFilter", (XlsxPartsManager pm) => new AutoFilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("filterColumn", (XlsxPartsManager pm) => new FilterColumnElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("filters", (XlsxPartsManager pm) => new FiltersElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("filter", (XlsxPartsManager pm) => new FilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dateGroupItem", (XlsxPartsManager pm) => new DateGroupItemElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("customFilters", (XlsxPartsManager pm) => new CustomFiltersElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("customFilter", (XlsxPartsManager pm) => new CustomFilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("top10", (XlsxPartsManager pm) => new Top10FilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dynamicFilter", (XlsxPartsManager pm) => new DynamicFilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("colorFilter", (XlsxPartsManager pm) => new ColorFilterElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sortState", (XlsxPartsManager pm) => new SortStateElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("sortCondition", (XlsxPartsManager pm) => new SortConditionElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("colOff", (XlsxPartsManager pm) => new OneDimentionOffsetElement(pm, "colOff", OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("rowOff", (XlsxPartsManager pm) => new OneDimentionOffsetElement(pm, "rowOff", OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("from", (XlsxPartsManager pm) => new MarkerElement(pm, "from"));
			XlsxElementsFactory.RegisterFactoryMethod("to", (XlsxPartsManager pm) => new MarkerElement(pm, "to"));
			XlsxElementsFactory.RegisterFactoryMethod("absoluteAnchor", (XlsxPartsManager pm) => new AbsoluteAnchorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pos", (XlsxPartsManager pm) => new OffsetElement(pm, "pos", OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("oneCellAnchor", (XlsxPartsManager pm) => new OneCellAnchorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("twoCellAnchor", (XlsxPartsManager pm) => new TwoCellAnchorElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("drawing", (XlsxPartsManager pm) => new DrawingElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("pic", (XlsxPartsManager pm) => new PictureElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("clientData", (XlsxPartsManager pm) => new ClientDataElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("blipFill", (XlsxPartsManager pm) => new BlipFillElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("spPr", (XlsxPartsManager pm) => new ShapePropertiesElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("nvPicPr", (XlsxPartsManager pm) => new NonVisualPicturePropertiesElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("cNvPicPr", (XlsxPartsManager pm) => new NonVisualPictureDrawingPropertiesElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("cNvPr", (XlsxPartsManager pm) => new NonVisualDrawingPropertiesElement(pm, "cNvPr", OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("graphicFrame", (XlsxPartsManager pm) => new GraphicFrameElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("nvGraphicFramePr", (XlsxPartsManager pm) => new NonVisualGraphicFramePropertiesElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("cNvGraphicFramePr", (XlsxPartsManager pm) => new NonVisualGraphicFrameDrawingPropertiesElement(pm, OpenXmlNamespaces.SpreadsheetDrawingMLNamespace));
			XlsxElementsFactory.RegisterFactoryMethod("dataValidations", (XlsxPartsManager pm) => new DataValidationsElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("dataValidation", (XlsxPartsManager pm) => new DataValidationElement(pm));
			XlsxElementsFactory.RegisterFactoryMethod("formula1", (XlsxPartsManager pm) => new Formula1Element(pm));
			XlsxElementsFactory.RegisterFactoryMethod("formula2", (XlsxPartsManager pm) => new Formula2Element(pm));
			XlsxElementsFactory.RegisterFactoryMethod("extLst", (XlsxPartsManager pm) => new FutureFeatureDataStorageAreaElement(pm));
		}

		public static OpenXmlElementBase CreateInstance(string elementName, XlsxPartsManager partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<XlsxPartsManager>(partsManager, "partsManager");
			return XlsxElementsFactory.elementNameToFactoryMethod[elementName](partsManager);
		}

		public static bool CanCreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			return XlsxElementsFactory.elementNameToFactoryMethod.ContainsKey(elementName);
		}

		static void RegisterFactoryMethod(string elementName, Func<XlsxPartsManager, OpenXmlElementBase> factoryMethod)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<Func<XlsxPartsManager, OpenXmlElementBase>>(factoryMethod, "factoryMethod");
			XlsxElementsFactory.elementNameToFactoryMethod.Add(elementName, factoryMethod);
		}

		static readonly Dictionary<string, Func<XlsxPartsManager, OpenXmlElementBase>> elementNameToFactoryMethod = new Dictionary<string, Func<XlsxPartsManager, OpenXmlElementBase>>();
	}
}
