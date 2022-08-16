using System;
using System.Collections;
using System.Collections.Generic;
using CsQuery.Implementation;

namespace CsQuery
{
	interface IHTMLFormElement : IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, INodeList<IDomElement>, CsQuery.Implementation.IReadOnlyList<IDomElement>, CsQuery.Implementation.IReadOnlyCollection<IDomElement>, IEnumerable<IDomElement>, IEnumerable
	{
		string AcceptCharset { get; set; }

		string Action { get; set; }

		string Autocomplete { get; set; }

		string Enctype { get; set; }

		string Encoding { get; set; }

		string Method { get; set; }

		bool NoValidate { get; set; }

		string Target { get; set; }

		INodeList<IDomElement> Elements { get; }
	}
}
