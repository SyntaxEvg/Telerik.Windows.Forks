using System;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	public abstract class FixedDocumentElementBase : IFixedDocumentElement
	{
		internal FixedDocumentElementBase()
		{
		}

		internal FixedDocumentElementBase(IFixedDocumentElement parent)
		{
			this.SetParent(parent);
		}

		public IFixedDocumentElement Parent
		{
			get
			{
				return this.parent;
			}
		}

		internal void SetParent(IFixedDocumentElement newParent)
		{
			if (this.Parent == newParent)
			{
				return;
			}
			this.parent = newParent;
		}

		internal abstract FixedDocumentElementType ElementType { get; }

		IFixedDocumentElement parent;
	}
}
