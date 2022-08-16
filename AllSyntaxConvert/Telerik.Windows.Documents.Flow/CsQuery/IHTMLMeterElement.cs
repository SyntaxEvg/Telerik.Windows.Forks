using System;

namespace CsQuery
{
	interface IHTMLMeterElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		int Value { get; set; }

		double Min { get; set; }

		double Max { get; set; }

		double Low { get; set; }

		double High { get; set; }

		double Optimum { get; set; }

		INodeList<IDomElement> Labels { get; }
	}
}
