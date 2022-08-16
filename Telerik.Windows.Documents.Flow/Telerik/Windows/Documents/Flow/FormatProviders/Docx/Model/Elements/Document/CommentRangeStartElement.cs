using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class CommentRangeStartElement : AnnotationStartEndElementBase
	{
		public CommentRangeStartElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "commentRangeStart";
			}
		}
	}
}
