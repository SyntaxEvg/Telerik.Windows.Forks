using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Collections
{
	public class DocumentElementCollection<T, TOwner> : DocumentElementCollectionBase<T, TOwner> where T : DocumentElementBase where TOwner : DocumentElementBase
	{
		internal DocumentElementCollection(TOwner owner)
			: base(owner)
		{
		}

		internal void AddClonedChildrenFrom(DocumentElementCollection<T, TOwner> fromCollection, CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Guard.ThrowExceptionIfNull<DocumentElementCollection<T, TOwner>>(fromCollection, "fromCollection");
			foreach (T t in fromCollection)
			{
				T t2 = (T)((object)t.CloneCore(cloneContext));
				if (t2 != null)
				{
					base.Add(t2);
					t.OnAfterCloneCore(cloneContext, t2);
				}
			}
		}

		protected override void SetParent(T item, TOwner parent)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			item.SetParent(parent);
		}

		protected override void VerifyDocumentElementOnInsert(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			RadFlowDocument document = item.Document;
			TOwner owner = base.Owner;
			if (document != owner.Document)
			{
				throw new ArgumentException("The document element is associated with another document.", "item");
			}
			if (item.Parent != null)
			{
				throw new ArgumentException("The document element is already associated with a parent.", "item");
			}
		}
	}
}
