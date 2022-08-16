using System;

namespace CsQuery
{
	interface IHTMLOptionElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		IHTMLFormElement Form { get; }

		string Label { get; set; }
	}
}
