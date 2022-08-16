using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class ResourceEntry
	{
		public ResourceEntry(string resourceKey, IndirectObject resource)
		{
			Guard.ThrowExceptionIfNullOrEmpty(resourceKey, "resourceKey");
			Guard.ThrowExceptionIfNull<IndirectObject>(resource, "resource");
			this.resourceKey = resourceKey;
			this.resource = resource;
		}

		public IndirectObject Resource
		{
			get
			{
				return this.resource;
			}
		}

		public string ResourceKey
		{
			get
			{
				return this.resourceKey;
			}
		}

		readonly string resourceKey;

		readonly IndirectObject resource;
	}
}
