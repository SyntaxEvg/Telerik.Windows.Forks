using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	class HTMLInputElement : FormSubmittableElement, IHTMLInputElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		public HTMLInputElement()
			: base(9)
		{
		}

		public bool Autofocus
		{
			get
			{
				return base.HasAttribute(43);
			}
			set
			{
				base.SetProp(43, value);
			}
		}

		public bool Required
		{
			get
			{
				return base.HasAttribute(42);
			}
			set
			{
				base.SetProp(42, value);
			}
		}

		public override string Type
		{
			get
			{
				return base.GetAttribute(44, "text").ToLower();
			}
			set
			{
				base.Type = value;
			}
		}

		public override IEnumerable<ushort[]> IndexKeysRanged()
		{
			return base.IndexKeysRanged();
		}

		public override bool HasChildren
		{
			get
			{
				return false;
			}
		}
	}
}
