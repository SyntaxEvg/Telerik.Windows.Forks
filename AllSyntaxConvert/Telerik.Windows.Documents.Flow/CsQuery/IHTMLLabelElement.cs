using System;

namespace CsQuery
{
	interface IHTMLLabelElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormReassociateableElement, IFormAssociatedElement
	{
		string HtmlFor { get; set; }

		IDomElement Control { get; }
	}
}
