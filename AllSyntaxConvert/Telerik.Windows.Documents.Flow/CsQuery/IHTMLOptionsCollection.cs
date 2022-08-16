using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CsQuery
{
	interface IHTMLOptionsCollection : IEnumerable<IDomObject>, IEnumerable
	{
		IDomElement Item(int index);

		[IndexerName("Indexer")]
		IDomElement this[int index] { get; }

		IDomElement NamedItem(string name);

		[IndexerName("Indexer")]
		IDomElement this[string name] { get; }
	}
}
