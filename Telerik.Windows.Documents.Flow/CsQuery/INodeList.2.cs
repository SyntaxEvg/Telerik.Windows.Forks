using System;
using System.Collections;
using System.Collections.Generic;
using CsQuery.Implementation;

namespace CsQuery
{
	interface INodeList : IList<IDomObject>, ICollection<IDomObject>, IEnumerable<IDomObject>, IEnumerable
	{
		int Length { get; }

		IDomObject Item(int index);

		event EventHandler<NodeEventArgs> OnChanged;
	}
}
