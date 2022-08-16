using System;
using CsQuery.Utility;

namespace CsQuery.Implementation
{
	class HTMLLIElement : DomElement, IHTMLLIElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable
	{
		public HTMLLIElement()
			: base(21)
		{
		}

		public new int Value
		{
			get
			{
				return Support.IntOrZero(base.GetAttribute(4));
			}
			set
			{
				base.SetAttribute(4, value.ToString());
			}
		}
	}
}
