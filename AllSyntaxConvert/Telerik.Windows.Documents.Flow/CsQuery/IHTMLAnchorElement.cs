using System;

namespace CsQuery
{
	interface IHTMLAnchorElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		string Target { get; set; }

		string Href { get; set; }

		RelAnchor Rel { get; set; }

		string Hreflang { get; set; }

		string Media { get; set; }
	}
}
