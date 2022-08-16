using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class ResourceKey
	{
		public int Id { get; set; }

		public ResourceType Type { get; set; }

		public IndirectReferenceOld Reference { get; internal set; }

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.Id.GetHashCode();
			num = num * 23 + this.Type.GetHashCode();
			return (this.Reference == null) ? 0 : (num * 23 + this.Reference.GetHashCode());
		}

		public override bool Equals(object obj)
		{
			ResourceKey resourceKey = obj as ResourceKey;
			return resourceKey != null && (this.Id == resourceKey.Id && this.Type == resourceKey.Type) && this.Reference == resourceKey.Reference;
		}
	}
}
