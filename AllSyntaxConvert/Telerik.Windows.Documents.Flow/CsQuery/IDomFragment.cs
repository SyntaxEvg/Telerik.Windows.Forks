using System;

namespace CsQuery
{
	interface IDomFragment : IDomDocument, IDomContainer, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
	}
}
