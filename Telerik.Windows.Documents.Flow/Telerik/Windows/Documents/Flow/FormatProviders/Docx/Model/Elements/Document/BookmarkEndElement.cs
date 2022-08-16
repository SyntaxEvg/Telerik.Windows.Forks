using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class BookmarkEndElement : AnnotationStartEndElementBase
	{
		public BookmarkEndElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "bookmarkEnd";
			}
		}
	}
}
