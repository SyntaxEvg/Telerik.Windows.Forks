using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	static class OpenXmlHelper
	{
		public static bool IsSupportedImageSourceExtension(string extension)
		{
			return OpenXmlHelper.imageExtensions.Contains(extension);
		}

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

		public static bool IsValidId(int id)
		{
			return id >= 0;
		}

		const double ColorComponentMaxValue = 255.0;

		const double Percentage = 1000.0;

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
				OpenXmlContentTypeNames.RelationshipContentType
			}
		};

		static readonly HashSet<string> imageExtensions = new HashSet<string> { "jpg", "jpeg", "png", "bmp", "tiff", "tif", "gif", "icon", "wmf", "emf" };
	}
}
