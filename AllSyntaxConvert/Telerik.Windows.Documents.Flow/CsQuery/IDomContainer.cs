using System;
using System.Collections.Generic;

namespace CsQuery
{
	interface IDomContainer : IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		IEnumerable<IDomObject> CloneChildren();
	}
}
