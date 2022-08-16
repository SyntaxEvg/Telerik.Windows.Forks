using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class PermissionEndElement : AnnotationStartEndElementBase
	{
		public PermissionEndElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "permEnd";
			}
		}
	}
}
