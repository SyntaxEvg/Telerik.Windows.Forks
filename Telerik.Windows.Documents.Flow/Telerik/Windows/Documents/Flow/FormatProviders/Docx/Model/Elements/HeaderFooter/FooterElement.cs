using System;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.HeaderFooter
{
	class FooterElement : HeaderFooterElementBase<Footer>
	{
		public FooterElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
		}

		public override string ElementName
		{
			get
			{
				return "ftr";
			}
		}
	}
}
