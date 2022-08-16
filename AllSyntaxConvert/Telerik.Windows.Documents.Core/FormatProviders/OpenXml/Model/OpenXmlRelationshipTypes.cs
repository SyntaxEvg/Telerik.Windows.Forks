using System;
using System.Collections.Generic;
using System.Globalization;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model
{
	static class OpenXmlRelationshipTypes
	{
		static OpenXmlRelationshipTypes()
		{
			OpenXmlRelationshipTypes.RegisterRelationshipType("jpg", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("jpeg", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("gif", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("tif", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("tiff", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("bmp", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("png", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("icon", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("wmf", OpenXmlRelationshipTypes.ImageRelationshipType);
			OpenXmlRelationshipTypes.RegisterRelationshipType("emf", OpenXmlRelationshipTypes.ImageRelationshipType);
		}

		public static string GetRelationshipTypeByExtension(string extension)
		{
			extension = PathExtension.StripExtension(extension);
			return OpenXmlRelationshipTypes.extensionToRelationshipType[extension.ToLower(CultureInfo.InvariantCulture)];
		}

		static void RegisterRelationshipType(string extension, string relationshipType)
		{
			OpenXmlRelationshipTypes.extensionToRelationshipType[extension] = relationshipType;
		}

		public static readonly string ThemeRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme";

		public static readonly string ImageRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image";

		public static readonly string ChartRelationshipType = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/chart";

		static readonly Dictionary<string, string> extensionToRelationshipType = new Dictionary<string, string>();
	}
}
