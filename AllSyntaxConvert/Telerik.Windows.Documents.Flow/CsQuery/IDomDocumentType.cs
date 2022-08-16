using System;

namespace CsQuery
{
	interface IDomDocumentType : IDomSpecialElement, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		DocType DocType { get; }
	}
}
