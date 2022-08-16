using System;

namespace CsQuery
{
	interface IHTMLProgressElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		int Value { get; set; }

		double Max { get; set; }

		double Position { get; }

		INodeList<IHTMLLabelElement> Labels { get; }
	}
}
