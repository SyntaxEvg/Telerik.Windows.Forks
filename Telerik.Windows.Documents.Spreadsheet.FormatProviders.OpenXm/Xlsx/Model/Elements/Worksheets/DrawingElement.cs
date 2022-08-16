using System;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class DrawingElement : WorksheetElementBase
	{
		public DrawingElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.relationId = base.RegisterAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "drawing";
			}
		}

		public string RelationshipId
		{
			get
			{
				return this.relationId.Value;
			}
			set
			{
				this.relationId.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return context.Shapes.Any<FloatingShapeBase>() || base.ShouldExport(context);
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			if (context.Shapes.Any<FloatingShapeBase>())
			{
				DrawingPart drawingPartFromWorksheet = context.WorkbookContext.GetDrawingPartFromWorksheet(context.Worksheet);
				this.RelationshipId = base.PartsManager.CreateRelationship(base.Part.Name, drawingPartFromWorksheet.Name, XlsxRelationshipTypes.DrawingRelationshipType, null);
			}
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, this.RelationshipId);
			string resourceName = base.Part.GetResourceName(relationshipTarget);
			DrawingPart part = base.PartsManager.GetPart<DrawingPart>(resourceName);
			context.WorkbookContext.RegisterDrawingPart(context.Worksheet, part);
		}

		readonly OpenXmlAttribute<string> relationId;
	}
}
