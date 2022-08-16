using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.ContentTypes
{
	class DefaultContentType
	{
		public DefaultContentType(string extension, string contentType)
		{
			this.Extension = extension;
			this.ContentType = contentType;
		}

		public DefaultContentType()
		{
		}

		public string Extension
		{
			get
			{
				return this.extension;
			}
			set
			{
				this.extension = value;
				string contentType;
				if (OpenXmlHelper.TryGetContentTypeByExtension(this.Extension, out contentType))
				{
					this.ContentType = contentType;
				}
			}
		}

		public string ContentType { get; set; }

		public override bool Equals(object obj)
		{
			DefaultContentType defaultContentType = obj as DefaultContentType;
			return defaultContentType != null && ObjectExtensions.EqualsOfT<string>(this.Extension, defaultContentType.Extension) && ObjectExtensions.EqualsOfT<string>(this.ContentType, defaultContentType.ContentType);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Extension.GetHashCodeOrZero(), this.ContentType.GetHashCodeOrZero());
		}

		string extension;
	}
}
