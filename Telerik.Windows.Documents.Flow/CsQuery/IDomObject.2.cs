using System;

namespace CsQuery
{
	interface IDomObject<out T> : IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		T Clone();
	}
}
