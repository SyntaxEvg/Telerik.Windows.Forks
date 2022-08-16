using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Comments;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class CommentsPart : DocxPartBase
	{
		public CommentsPart(DocxPartsManager partsManager)
			: this(partsManager, "/word/comments.xml")
		{
			base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.CommentsRelationshipType, null);
		}

		public CommentsPart(DocxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.commentsElement = new CommentsElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.CommentsContentType;
			}
		}

		public override int Level
		{
			get
			{
				return 5;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.commentsElement;
			}
		}

		readonly CommentsElement commentsElement;
	}
}
