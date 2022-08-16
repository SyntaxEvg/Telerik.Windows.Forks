using System;
using System.Linq;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Utilities;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export
{
	class XlsxExporter : OpenXmlExporter<XlsxPartsManager, IXlsxWorkbookExportContext>
	{
		protected override XlsxPartsManager CreatePartsManager()
		{
			return new XlsxPartsManager();
		}

		protected override void InitializeParts(XlsxPartsManager partsManager, IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<XlsxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			base.InitializeParts(partsManager, context);
			WorkbookPart part = new WorkbookPart(partsManager);
			partsManager.RegisterPart(new SharedStringsPart(partsManager));
			partsManager.RegisterPart(new StylesPart(partsManager));
			ThemePart themePart = new ThemePart(partsManager, "/xl/theme/theme1.xml");
			partsManager.RegisterPart(themePart);
			partsManager.CreateWorkbookRelationship(themePart.Name, OpenXmlRelationshipTypes.ThemeRelationshipType, null);
			foreach (IXlsxWorksheetExportContext worksheetContext in context.WorksheetContexts)
			{
				this.InitializeWorksheetPart(partsManager, context, worksheetContext);
			}
			int num = 1;
			foreach (IXlsxWorksheetExportContext xlsxWorksheetExportContext in context.WorksheetContexts)
			{
				foreach (FloatingChartShape floatingChartShape in xlsxWorksheetExportContext.Charts)
				{
					this.InitializeChartPart(partsManager, context, num, (ChartShape)floatingChartShape.Shape);
					num++;
				}
			}
			partsManager.RegisterPart(part);
		}

		protected override string CreateResourceName(IResource resource)
		{
			return XlsxHelper.CreateResourceName(resource);
		}

		void InitializeWorksheetPart(XlsxPartsManager partsManager, IXlsxWorkbookExportContext workbookContext, IXlsxWorksheetExportContext worksheetContext)
		{
			Guard.ThrowExceptionIfNull<XlsxPartsManager>(partsManager, "partsManager");
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(workbookContext, "workbookContext");
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(worksheetContext, "worksheetContext");
			WorksheetPart worksheetPart = new WorksheetPart(partsManager, worksheetContext);
			workbookContext.RegisterWorksheetPart(worksheetContext.Worksheet, worksheetPart);
			partsManager.RegisterPart(worksheetPart);
			if (this.HasShapes(worksheetContext))
			{
				this.InitializeDrawingPart(partsManager, worksheetContext);
			}
		}

		void InitializeDrawingPart(XlsxPartsManager partsManager, IXlsxWorksheetExportContext context)
		{
			DrawingPart drawingPart = new DrawingPart(partsManager, context);
			partsManager.RegisterPart(drawingPart);
			context.WorkbookContext.RegisterDrawingPart(context.Worksheet, drawingPart);
		}

		void InitializeChartPart(XlsxPartsManager partsManager, IXlsxWorkbookExportContext workbookContext, int chartNumber, ChartShape chart)
		{
			ChartPart chartPart = new ChartPart(partsManager, string.Format("/xl/charts/chart{0}.xml", chartNumber));
			partsManager.RegisterPart(chartPart);
			workbookContext.RegisterChartPartForChart(chart, chartPart);
		}

		bool HasShapes(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			return context.Shapes.Any<FloatingShapeBase>();
		}
	}
}
