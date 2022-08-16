using System;

namespace CsQuery
{
	interface IDomComment : IDomSpecialElement, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		bool IsQuoted { get; set; }
	}
}
