using System;

namespace CsQuery.Implementation
{
	class HTMLButtonElement : FormSubmittableElement, IHTMLButtonElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		public HTMLButtonElement()
			: base(37)
		{
		}

		public override string Type
		{
			get
			{
				return base.GetAttribute(44, "submit").ToLower();
			}
			set
			{
				base.Type = value;
			}
		}
	}
}
