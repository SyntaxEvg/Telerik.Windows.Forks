using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	class DocxPartsManager : OpenXmlPartsManager
	{
		public DocxPartsManager()
		{
			this.elementsPool = new DocxElementsPool(this);
		}

		public override OpenXmlElementBase CreateElement(string elementName, OpenXmlPartBase part)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			return this.elementsPool.CreateElement(elementName, part);
		}

		public override void ReleaseElement(OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(element, "element");
			this.elementsPool.ReleaseElement(element);
		}

		public override OpenXmlPartBase CreatePart(string partType, string partName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(partType, "partType");
			Guard.ThrowExceptionIfNullOrEmpty(partName, "partName");
			if (DocxPartsFactory.CanCreateInstance(partType))
			{
				return DocxPartsFactory.CreateInstance(partType, this, partName);
			}
			return base.CreatePart(partType, partName);
		}

		public string CreateDocumentRelationship(string target, string type, string targetMode = null)
		{
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			Guard.ThrowExceptionIfNullOrEmpty(type, "type");
			return base.CreateRelationship("/word/document.xml", target, type, targetMode);
		}

		public string GetDocumentRelationshipId(string target)
		{
			Guard.ThrowExceptionIfNullOrEmpty(target, "target");
			return base.GetRelationshipId("/word/document.xml", target);
		}

		readonly DocxElementsPool elementsPool;
	}
}
