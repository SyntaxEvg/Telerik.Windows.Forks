using System;

namespace CsQuery
{
	[Obsolete]
	interface IDomInvalidElement : IDomText, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
	}
}
