using System;

namespace CsQuery
{
	interface IDomSpecialElement : IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		string NonAttributeData { get; set; }
	}
}
