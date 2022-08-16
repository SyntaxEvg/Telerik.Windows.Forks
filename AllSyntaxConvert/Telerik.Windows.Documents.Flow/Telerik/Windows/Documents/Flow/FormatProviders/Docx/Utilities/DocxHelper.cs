using System;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities
{
	static class DocxHelper
	{
		public static string CreateResourceName(IResource resource)
		{
			return string.Format("/word/media/{0}", resource.Name);
		}

		public static void AddHangingAnnotation(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "bookmarkStart")
				{
					BookmarkStartElement bookmarkStartElement = (BookmarkStartElement)childElement;
					Bookmark bookmark = BookmarkContext.CreateBookmark(context.Document, bookmarkStartElement.Name, bookmarkStartElement.ColFirst, bookmarkStartElement.ColLast);
					context.BookmarkContext.AddHangingBookmarkStart(bookmark.BookmarkRangeStart);
					context.BookmarkContext.RegisterAnnotationToImport(bookmarkStartElement.Id, bookmark);
					return;
				}
				if (elementName == "bookmarkEnd")
				{
					BookmarkEndElement bookmarkEndElement = (BookmarkEndElement)childElement;
					context.BookmarkContext.AddHangingBookmarkEndId(bookmarkEndElement.Id);
					return;
				}
				if (elementName == "permStart")
				{
					PermissionStartElement permissionStartElement = (PermissionStartElement)childElement;
					PermissionRange permissionRange = PermissionContext.CreatePermission(context.Document, permissionStartElement);
					context.PermissionContext.AddHangingPermissionStart(permissionRange.Start);
					context.PermissionContext.RegisterAnnotationToImport(permissionStartElement.Id, permissionRange);
					return;
				}
				if (!(elementName == "permEnd"))
				{
					return;
				}
				PermissionEndElement permissionEndElement = (PermissionEndElement)childElement;
				context.PermissionContext.AddHangingPermissionEndId(permissionEndElement.Id);
			}
		}
	}
}
