using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class DocumentElementBase : DocxElementBase
	{
		public DocumentElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
