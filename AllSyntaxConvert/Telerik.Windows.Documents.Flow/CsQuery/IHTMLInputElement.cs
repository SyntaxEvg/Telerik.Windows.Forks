using System;

namespace CsQuery
{
	interface IHTMLInputElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		bool Autofocus { get; set; }

		bool Required { get; set; }
	}
}
