using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class FooterReferenceElement : HeaderFooterReferenceElementBase
	{
		public FooterReferenceElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "footerReference";
			}
		}
	}
}
