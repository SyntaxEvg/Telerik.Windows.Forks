using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public class DocumentElementCollection<T, TOwner> : DocumentElementCollectionBase<T, TOwner> where T : FixedDocumentElementBase where TOwner : IFixedDocumentElement
	{
		public DocumentElementCollection(TOwner parent)
			: base(parent)
		{
		}

		protected override void SetParent(T item, TOwner parent)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			item.SetParent(parent);
		}

		protected override void VerifyDocumentElementOnInsert(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			if (item.Parent != null && !item.Parent.Equals(base.Owner))
			{
				throw new ArgumentException("The document element is associated with another parent.", "item");
			}
		}
	}
}
