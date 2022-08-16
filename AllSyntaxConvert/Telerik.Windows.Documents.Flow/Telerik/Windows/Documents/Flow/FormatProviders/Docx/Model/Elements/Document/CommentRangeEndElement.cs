using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class CommentRangeEndElement : AnnotationStartEndElementBase
	{
		public CommentRangeEndElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "commentRangeEnd";
			}
		}
	}
}
