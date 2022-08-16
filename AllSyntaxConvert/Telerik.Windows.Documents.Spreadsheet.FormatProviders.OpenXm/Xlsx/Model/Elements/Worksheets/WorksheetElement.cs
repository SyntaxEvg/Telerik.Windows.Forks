using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class WorksheetElement : XlsxPartRootElementBase
	{
		public WorksheetElement(XlsxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.sheetProperties = base.RegisterChildElement<SheetPropertiesElement>("sheetPr");
			this.dimension = base.RegisterChildElement<DimensionElement>("dimension");
			this.sheetViews = base.RegisterChildElement<SheetViewsElement>("sheetViews");
			this.sheetFormatProperties = base.RegisterChildElement<SheetFormatPropertiesElement>("sheetFormatPr");
			this.columns = base.RegisterChildElement<ColumnsElement>("cols");
			this.sheetData = base.RegisterChildElement<SheetDataElement>("sheetData");
			this.sheetProtection = base.RegisterChildElement<SheetProtectionElement>("sheetProtection");
			this.autoFilter = base.RegisterChildElement<AutoFilterElement>("autoFilter");
			this.sortState = base.RegisterChildElement<SortStateElement>("sortState");
			this.mergedCells = base.RegisterChildElement<MergedCellsElement>("mergeCells");
			this.hyperlinks = base.RegisterChildElement<HyperlinksElement>("hyperlinks");
			this.dataValidations = base.RegisterChildElement<DataValidationsElement>("dataValidations");
			this.printOptions = base.RegisterChildElement<PrintOptionsElement>("printOptions");
			this.pageMargins = base.RegisterChildElement<PageMarginsElement>("pageMargins");
			this.pageSetup = base.RegisterChildElement<PageSetupElement>("pageSetup");
			this.headerFooter = base.RegisterChildElement<HeaderFooterElement>("headerFooter");
			this.rowBreaks = base.RegisterChildElement<RowBreaksElement>("rowBreaks");
			this.columnBreaks = base.RegisterChildElement<ColumnBreaksElement>("colBreaks");
			this.drawing = base.RegisterChildElement<DrawingElement>("drawing");
			this.extList = base.RegisterChildElement<FutureFeatureDataStorageAreaElement>("extLst");
		}

		public override string ElementName
		{
			get
			{
				return "worksheet";
			}
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			base.CreateElement(this.sheetProperties);
			base.CreateElement(this.dimension);
			base.CreateElement(this.sheetViews);
			base.CreateElement(this.sheetFormatProperties);
			base.CreateElement(this.columns);
			base.CreateElement(this.sheetData);
			base.CreateElement(this.sheetProtection);
			base.CreateElement(this.autoFilter);
			base.CreateElement(this.sortState);
			base.CreateElement(this.mergedCells);
			base.CreateElement(this.hyperlinks);
			base.CreateElement(this.dataValidations);
			base.CreateElement(this.printOptions);
			base.CreateElement(this.pageMargins);
			base.CreateElement(this.pageSetup);
			base.CreateElement(this.headerFooter);
			base.CreateElement(this.rowBreaks);
			base.CreateElement(this.columnBreaks);
			base.CreateElement(this.drawing);
			base.CreateElement(this.extList);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			base.ReleaseElement(this.sheetProperties);
			base.ReleaseElement(this.dimension);
			base.ReleaseElement(this.sheetViews);
			base.ReleaseElement(this.sheetFormatProperties);
			base.ReleaseElement(this.columns);
			base.ReleaseElement(this.sheetData);
			base.ReleaseElement(this.sheetProtection);
			base.ReleaseElement(this.autoFilter);
			base.ReleaseElement(this.sortState);
			base.ReleaseElement(this.mergedCells);
			base.ReleaseElement(this.hyperlinks);
			base.ReleaseElement(this.dataValidations);
			base.ReleaseElement(this.printOptions);
			base.ReleaseElement(this.pageMargins);
			base.ReleaseElement(this.pageSetup);
			base.ReleaseElement(this.headerFooter);
			base.ReleaseElement(this.rowBreaks);
			base.ReleaseElement(this.columnBreaks);
			base.ReleaseElement(this.drawing);
			base.ReleaseElement(this.extList);
		}

		readonly OpenXmlChildElement<SheetPropertiesElement> sheetProperties;

		readonly OpenXmlChildElement<SheetDataElement> sheetData;

		readonly OpenXmlChildElement<SheetProtectionElement> sheetProtection;

		readonly OpenXmlChildElement<MergedCellsElement> mergedCells;

		readonly OpenXmlChildElement<ColumnsElement> columns;

		readonly OpenXmlChildElement<SheetFormatPropertiesElement> sheetFormatProperties;

		readonly OpenXmlChildElement<AutoFilterElement> autoFilter;

		readonly OpenXmlChildElement<SortStateElement> sortState;

		readonly OpenXmlChildElement<HyperlinksElement> hyperlinks;

		readonly OpenXmlChildElement<DrawingElement> drawing;

		readonly OpenXmlChildElement<DimensionElement> dimension;

		readonly OpenXmlChildElement<SheetViewsElement> sheetViews;

		readonly OpenXmlChildElement<PrintOptionsElement> printOptions;

		readonly OpenXmlChildElement<PageMarginsElement> pageMargins;

		readonly OpenXmlChildElement<PageSetupElement> pageSetup;

		readonly OpenXmlChildElement<HeaderFooterElement> headerFooter;

		readonly OpenXmlChildElement<RowBreaksElement> rowBreaks;

		readonly OpenXmlChildElement<ColumnBreaksElement> columnBreaks;

		readonly OpenXmlChildElement<DataValidationsElement> dataValidations;

		readonly OpenXmlChildElement<FutureFeatureDataStorageAreaElement> extList;
	}
}
