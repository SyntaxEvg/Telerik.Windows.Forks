using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model
{
	class XlsxPartsManager : OpenXmlPartsManager
	{
		public XlsxPartsManager()
		{
			this.elementsPool = new XlsxElementsPool(this);
		}

		public override OpenXmlElementBase CreateElement(string elementName, OpenXmlPartBase part)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			return this.elementsPool.CreateElement(elementName, part);
		}

		public override OpenXmlPartBase CreatePart(string partType, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			if (XlsxPartsFactory.CanCreateInstance(partType))
			{
				return XlsxPartsFactory.CreateInstance(partType, this, partName);
			}
			return base.CreatePart(partType, partName);
		}

		public override void ReleaseElement(OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			this.elementsPool.ReleaseElement(element);
		}

		public string CreateWorkbookRelationship(string target, string type, string targetMode = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			return base.CreateRelationship("/xl/workbook.xml", target, type, targetMode);
		}

		public string GetWorkbookRelationshipId(string target)
		{
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			return base.GetRelationshipId("/xl/workbook.xml", target);
		}

		public string CreateWorksheetRelationship(WorksheetPart worksheetPart, string target, string type, string targetMode = null)
		{
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			return base.CreateRelationship(worksheetPart.Name, target, type, targetMode);
		}

		public string GetWorksheetRelationshipTarget(WorksheetPart worksheetPart, string relationshipId)
		{
			Guard.ThrowExceptionIfNull<WorksheetPart>(worksheetPart, "worksheetPart");
			Guard.ThrowExceptionIfNullOrEmpty(relationshipId, "relationshipId");
			return base.GetRelationshipTarget(worksheetPart.Name, relationshipId);
		}

		readonly XlsxElementsPool elementsPool;
	}
}
