using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class ParagraphElement : DocumentElementBase
	{
		public ParagraphElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.paragraphPropertiesChildElement = base.RegisterChildElement<ParagraphPropertiesElement>("pPr");
		}

		public override string ElementName
		{
			get
			{
				return "p";
			}
		}

		public Paragraph Paragraph
		{
			get
			{
				return this.paragraph;
			}
		}

		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
			set
			{
				Guard.ThrowExceptionIfNull<Watermark>(value, "value");
				this.watermark = value;
			}
		}

		ParagraphPropertiesElement ParagraphPropertiesElement
		{
			get
			{
				return this.paragraphPropertiesChildElement.Element;
			}
		}

		public virtual void SetAssociatedFlowModelElement(Paragraph paragraph, Section section = null)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.paragraph = paragraph;
			this.section = section;
		}

		protected override bool ShouldImport(IDocxImportContext context)
		{
			return true;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return true;
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			if (this.paragraph.Properties.HasLocalValues() || this.paragraph.IsLastInSectionButNotLastInDocument(this.section))
			{
				base.CreateElement(this.paragraphPropertiesChildElement);
				this.ParagraphPropertiesElement.SetAssociatedFlowModelElement(this.paragraph, this.section);
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (InlineBase inline in this.paragraph.Inlines)
			{
				switch (inline.Type)
				{
				case DocumentElementType.BookmarkRangeStart:
				{
					BookmarkRangeStart bookmarkRangeStart = (BookmarkRangeStart)inline;
					BookmarkStartElement bookmarkStartElement = base.CreateElement<BookmarkStartElement>("bookmarkStart");
					Bookmark bookmark = bookmarkRangeStart.Bookmark;
					bookmarkStartElement.CopyPropertiesFrom(bookmark);
					context.BookmarkContext.RegisterAnnotationToExport(bookmark, bookmarkStartElement.Id);
					yield return bookmarkStartElement;
					break;
				}
				case DocumentElementType.BookmarkRangeEnd:
				{
					BookmarkRangeEnd bookmarkRangeEnd = (BookmarkRangeEnd)inline;
					BookmarkEndElement bookmarkEndElement = base.CreateElement<BookmarkEndElement>("bookmarkEnd");
					bookmarkEndElement.Id = context.BookmarkContext.PopIdByRegisteredAnnotation(bookmarkRangeEnd.Bookmark);
					yield return bookmarkEndElement;
					break;
				}
				case DocumentElementType.CommentRangeStart:
				{
					CommentRangeStart commentRangeStart = (CommentRangeStart)inline;
					CommentRangeStartElement commentStartElement = base.CreateElement<CommentRangeStartElement>("commentRangeStart");
					commentStartElement.Id = context.CommentContext.GetIdByRegisteredAnnotation(commentRangeStart.Comment);
					yield return commentStartElement;
					break;
				}
				case DocumentElementType.CommentRangeEnd:
				{
					CommentRangeEnd commentRangeEnd = (CommentRangeEnd)inline;
					CommentRangeEndElement commentEndElement = base.CreateElement<CommentRangeEndElement>("commentRangeEnd");
					commentEndElement.Id = context.CommentContext.GetIdByRegisteredAnnotation(commentRangeEnd.Comment);
					yield return commentEndElement;
					RunElement commentReferenceRun = base.CreateElement<RunElement>("r");
					commentReferenceRun.SetAssociatedFlowModelElement(inline);
					yield return commentReferenceRun;
					break;
				}
				case DocumentElementType.PermissionRangeStart:
				{
					PermissionRangeStart permissionRangeStart = (PermissionRangeStart)inline;
					PermissionStartElement permissionStartElement = base.CreateElement<PermissionStartElement>("permStart");
					PermissionRange permission = permissionRangeStart.Permission;
					permissionStartElement.CopyPropertiesFrom(permission);
					context.PermissionContext.RegisterAnnotationToExport(permission, permissionStartElement.Id);
					yield return permissionStartElement;
					break;
				}
				case DocumentElementType.PermissionRangeEnd:
				{
					PermissionRangeEnd permissionRangeEnd = (PermissionRangeEnd)inline;
					PermissionEndElement permissionEndElement = base.CreateElement<PermissionEndElement>("permEnd");
					permissionEndElement.Id = context.PermissionContext.PopIdByRegisteredAnnotation(permissionRangeEnd.Permission);
					yield return permissionEndElement;
					break;
				}
				default:
				{
					RunElement runElement = base.CreateElement<RunElement>("r");
					runElement.SetAssociatedFlowModelElement(inline);
					if (this.watermark != null)
					{
						runElement.Watermark = this.watermark;
					}
					yield return runElement;
					break;
				}
				}
			}
			yield break;
		}

		protected override void OnAfterReadAttributes(IDocxImportContext context)
		{
			this.ProcessHangingAnnotations(context);
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "element");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "r")
				{
					((RunElement)childElement).SetAssociatedFlowModelElementParent(this.paragraph);
					return;
				}
				if (elementName == "pPr")
				{
					this.ParagraphPropertiesElement.SetAssociatedFlowModelElement(this.paragraph, null);
					return;
				}
				if (!(elementName == "hyperlink") && !(elementName == "fldSimple"))
				{
					return;
				}
				((ParagraphContentElementBase)childElement).SetAssociatedFlowModelElementParent(this.paragraph);
			}
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "element");
			string elementName;
			switch (elementName = childElement.ElementName)
			{
			case "bookmarkStart":
			{
				BookmarkStartElement bookmarkStartElement = (BookmarkStartElement)childElement;
				Bookmark bookmark = BookmarkContext.CreateBookmark(context.Document, bookmarkStartElement.Name, bookmarkStartElement.ColFirst, bookmarkStartElement.ColLast);
				this.paragraph.Inlines.Add(bookmark.BookmarkRangeStart);
				context.BookmarkContext.RegisterAnnotationToImport(bookmarkStartElement.Id, bookmark);
				return;
			}
			case "bookmarkEnd":
			{
				BookmarkRangeEnd bookmarkRangeEnd = context.BookmarkContext.PopRegisteredAnnotationById(((BookmarkEndElement)childElement).Id).BookmarkRangeEnd;
				this.paragraph.Inlines.Add(bookmarkRangeEnd);
				return;
			}
			case "commentRangeStart":
			{
				CommentRangeStartElement commentRangeStartElement = (CommentRangeStartElement)childElement;
				Comment comment = context.CommentContext.GetImportedCommentById(commentRangeStartElement.Id);
				if (comment == null)
				{
					comment = CommentContext.CreateComment(context.Document);
					context.CommentContext.AddImportedComment(commentRangeStartElement.Id, comment);
				}
				else if (comment.CommentRangeEnd.Paragraph != null)
				{
					CommentRangeStart commentRangeStart = comment.CommentRangeStart;
					commentRangeStart.Paragraph.Inlines.Remove(commentRangeStart);
				}
				this.paragraph.Inlines.Add(comment.CommentRangeStart);
				return;
			}
			case "commentRangeEnd":
			{
				CommentRangeEndElement commentRangeEndElement = (CommentRangeEndElement)childElement;
				Comment comment2 = context.CommentContext.GetImportedCommentById(commentRangeEndElement.Id);
				if (comment2 == null)
				{
					comment2 = CommentContext.CreateComment(context.Document);
					context.CommentContext.AddImportedComment(commentRangeEndElement.Id, comment2);
				}
				else if (comment2.CommentRangeEnd.Paragraph != null)
				{
					CommentRangeEnd commentRangeEnd = comment2.CommentRangeEnd;
					commentRangeEnd.Paragraph.Inlines.Remove(commentRangeEnd);
				}
				this.paragraph.Inlines.Add(comment2.CommentRangeEnd);
				return;
			}
			case "permStart":
			{
				PermissionStartElement permissionStartElement = (PermissionStartElement)childElement;
				PermissionRange permissionRange = PermissionContext.CreatePermission(context.Document, permissionStartElement);
				if (permissionRange != null)
				{
					this.paragraph.Inlines.Add(permissionRange.Start);
					context.PermissionContext.RegisterAnnotationToImport(permissionStartElement.Id, permissionRange);
					return;
				}
				break;
			}
			case "permEnd":
			{
				PermissionRangeEnd end = context.PermissionContext.PopRegisteredAnnotationById(((PermissionEndElement)childElement).Id).End;
				if (end != null)
				{
					this.paragraph.Inlines.Add(end);
					return;
				}
				break;
			}
			case "r":
			{
				RunElement runElement = (RunElement)childElement;
				if (runElement.Watermark != null)
				{
					this.watermark = runElement.Watermark;
				}
				break;
			}

				return;
			}
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.section = null;
			this.paragraph = null;
			this.watermark = null;
		}

		void ProcessHangingAnnotations(IDocxImportContext context)
		{
			this.ProcessHangingBookmarks(context);
			this.ProcessHangingPermissions(context);
		}

		void ProcessHangingBookmarks(IDocxImportContext context)
		{
			foreach (BookmarkRangeStart item in context.BookmarkContext.GetHangingBookmarkStarts())
			{
				this.paragraph.Inlines.Add(item);
			}
			context.BookmarkContext.ClearHangingBookmarkStarts();
			foreach (int id in context.BookmarkContext.GetHangingBookmarkEndIds())
			{
				this.paragraph.Inlines.Add(context.BookmarkContext.PopRegisteredAnnotationById(id).BookmarkRangeEnd);
			}
			context.BookmarkContext.ClearHangingBookmarkEnds();
		}

		void ProcessHangingPermissions(IDocxImportContext context)
		{
			foreach (PermissionRangeStart item in context.PermissionContext.GetHangingPermissionStarts())
			{
				this.paragraph.Inlines.Add(item);
			}
			context.PermissionContext.ClearHangingPermissionStarts();
			foreach (int id in context.PermissionContext.GetHangingPermissionEndIds())
			{
				this.paragraph.Inlines.Add(context.PermissionContext.PopRegisteredAnnotationById(id).End);
			}
			context.PermissionContext.ClearHangingPermissionEnds();
		}

		readonly OpenXmlChildElement<ParagraphPropertiesElement> paragraphPropertiesChildElement;

		Paragraph paragraph;

		Section section;

		Watermark watermark;
	}
}
