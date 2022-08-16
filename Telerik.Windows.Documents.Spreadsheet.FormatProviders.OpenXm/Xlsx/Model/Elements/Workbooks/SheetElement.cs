using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class SheetElement : WorkbookElementBase
	{
		public SheetElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.sheetId = base.RegisterAttribute<IntOpenXmlAttribute>(new IntOpenXmlAttribute("sheetId", true));
			this.name = base.RegisterAttribute<string>("name", true);
			this.state = base.RegisterAttribute<MappedOpenXmlAttribute<SheetVisibility>>(new MappedOpenXmlAttribute<SheetVisibility>("state", null, TypeMappers.SheetVisibilityMapper, false));
			this.relationId = base.RegisterAttribute<OpenXmlAttribute<string>>(new OpenXmlAttribute<string>("id", OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace, true));
		}

		public override string ElementName
		{
			get
			{
				return "sheet";
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

		public int SheetId
		{
			get
			{
				return this.sheetId.Value;
			}
			set
			{
				this.sheetId.Value = value;
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

		public SheetVisibility State
		{
			get
			{
				return this.state.Value;
			}
			set
			{
				this.state.Value = value;
			}
		}

		public void CopyPropertiesFrom(IXlsxWorkbookExportContext context, IXlsxWorksheetExportContext sheetContext, int sheetId)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			this.Name = sheetContext.WorksheetName;
			this.SheetId = sheetId;
			this.State = sheetContext.Worksheet.Visibility;
			WorksheetPart worksheetPartFromWorksheet = context.GetWorksheetPartFromWorksheet(sheetContext.Worksheet);
			this.RelationshipId = base.PartsManager.CreateWorkbookRelationship(worksheetPartFromWorksheet.Name, XlsxRelationshipTypes.WorksheetRelationshipType, null);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			string relationshipTarget = base.PartsManager.GetRelationshipTarget(base.Part.Name, this.RelationshipId);
			string resourceName = base.Part.GetResourceName(relationshipTarget);
			SheetPartBase part = base.PartsManager.GetPart<SheetPartBase>(resourceName);
			if (part != null && part.SheetType == SheetPartType.Worksheet)
			{
				IXlsxWorksheetImportContext xlsxWorksheetImportContext = context.AddWorksheet(this.Name);
				context.RegisterWorksheetPart(xlsxWorksheetImportContext.Worksheet, (WorksheetPart)part);
				xlsxWorksheetImportContext.Worksheet.Visibility = this.State;
			}
		}

		readonly IntOpenXmlAttribute sheetId;

		readonly OpenXmlAttribute<string> name;

		readonly OpenXmlAttribute<string> relationId;

		readonly MappedOpenXmlAttribute<SheetVisibility> state;
	}
}
