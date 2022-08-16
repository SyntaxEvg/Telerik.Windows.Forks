using System;

namespace CsQuery
{
	interface IHTMLLIElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		int Value { get; set; }
	}
}
