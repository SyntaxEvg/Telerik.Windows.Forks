using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes
{
	class OverrideContentType
	{
		public OverrideContentType()
		{
		}

		public OverrideContentType(string path, string contentType)
		{
			this.Path = path;
			this.ContentType = contentType;
		}

		public string Path { get; set; }

		public string ContentType { get; set; }

		public override bool Equals(object obj)
		{
			OverrideContentType overrideContentType = obj as OverrideContentType;
			return overrideContentType != null && ObjectExtensions.EqualsOfT<string>(this.Path, overrideContentType.Path) && ObjectExtensions.EqualsOfT<string>(this.ContentType, overrideContentType.ContentType);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Path.GetHashCodeOrZero(), this.ContentType.GetHashCodeOrZero());
		}
	}
}
