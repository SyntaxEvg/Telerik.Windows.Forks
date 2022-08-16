using System;

namespace CsQuery
{
	interface IDomElement : IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		bool IsBlock { get; }

		string ElementHtml();

		int ElementIndex { get; }
	}
}
