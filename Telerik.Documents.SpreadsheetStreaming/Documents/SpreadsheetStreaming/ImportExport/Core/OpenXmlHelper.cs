using System;
using System.Collections.Generic;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	static class OpenXmlHelper
	{
		public static bool TryGetContentTypeByExtension(string extension, out string contentType)
		{
			return OpenXmlHelper.contentTypeDefinitions.TryGetValue(extension, out contentType);
		}

		public static string GetExtension(string partName)
		{
			return partName.Substring(partName.LastIndexOf('.') + 1).ToLowerInvariant();
		}

		public static string CreateRelationshipId(int id)
		{
			return string.Format("rId{0}", id);
		}

		public static int GetRelationshipId(string id)
		{
			return int.Parse(id.Replace("rId", string.Empty));
		}

		static readonly Dictionary<string, string> contentTypeDefinitions = new Dictionary<string, string>
		{
			{ "xml", "application/xml" },
			{ "jpg", "image/jpeg" },
			{ "jpeg", "image/jpeg" },
			{ "png", "image/png" },
			{ "bmp", "image/bitmap" },
			{ "tiff", "image/tiff" },
			{ "tif", "image/tiff" },
			{ "gif", "image/gif" },
			{ "icon", "image/x-icon" },
			{ "wmf", "application/x-msmetafile" },
			{ "emf", "application/x-msmetafile" },
			{
				"rels",
				XlsxContentTypeNames.RelationshipContentType
			}
		};
	}
}
