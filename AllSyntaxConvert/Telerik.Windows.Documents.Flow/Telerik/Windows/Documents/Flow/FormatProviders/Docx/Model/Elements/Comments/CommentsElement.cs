using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Comments
{
	class CommentsElement : DocxPartRootElementBase
	{
		public CommentsElement(DocxPartsManager partsManager, CommentsPart part)
			: base(partsManager, part)
		{
		}

		public override string ElementName
		{
			get
			{
				return "comments";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return context.CommentContext.HasCommentsToExport;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (Comment comment in context.Document.Comments)
			{
				CommentElement commentElement = base.CreateElement<CommentElement>("comment");
				commentElement.SetAssociatedFlowModelElement(comment);
				commentElement.Id = AnnotationIdGenerator.GetNext();
				commentElement.Author = comment.Author;
				commentElement.Initials = comment.Initials;
				commentElement.Date = comment.Date.ToString("s");
				context.CommentContext.RegisterAnnotationToExport(comment, commentElement.Id);
				yield return commentElement;
			}
			yield break;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "comment"))
				{
					return;
				}
				CommentElement commentElement = (CommentElement)childElement;
				commentElement.SetAssociatedFlowModelElement(context.CommentContext.PopImportedComment());
			}
		}
	}
}
