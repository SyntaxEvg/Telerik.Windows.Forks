using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class CommentReferenceElement : AnnotationStartEndElementBase
	{
		public CommentReferenceElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "commentReference";
			}
		}
	}
}
