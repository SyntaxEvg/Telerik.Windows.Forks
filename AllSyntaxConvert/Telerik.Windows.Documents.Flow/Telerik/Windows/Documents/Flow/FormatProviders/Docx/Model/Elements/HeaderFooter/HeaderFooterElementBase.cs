using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.HeaderFooter
{
	abstract class HeaderFooterElementBase<T> : BlockLevelElementsContainerElementBase<T> where T : HeaderFooterBase
	{
		public HeaderFooterElementBase(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager)
		{
			base.Part = part;
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
