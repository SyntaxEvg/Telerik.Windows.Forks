using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	class DocxElementsPool : OpenXmlElementsPoolBase<DocxPartsManager>
	{
		public DocxElementsPool(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected override OpenXmlElementBase CreateInstance(string elementName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			if (DocxElementsFactory.CanCreateInstance(elementName))
			{
				return DocxElementsFactory.CreateInstance(elementName, base.PartsManager);
			}
			return base.CreateInstance(elementName);
		}
	}
}
