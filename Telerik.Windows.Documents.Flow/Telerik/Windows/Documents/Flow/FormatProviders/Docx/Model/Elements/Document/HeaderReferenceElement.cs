using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class HeaderReferenceElement : HeaderFooterReferenceElementBase
	{
		public HeaderReferenceElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "headerReference";
			}
		}
	}
}
