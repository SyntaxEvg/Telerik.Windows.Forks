using System;
using System.Linq;

namespace CsQuery.Implementation
{
	class FormSubmittableElement : FormReassociateableElement, IFormSubmittableElement, IFormAssociatedElement
	{
		protected FormSubmittableElement(ushort tokenId)
			: base(tokenId)
		{
		}

		public override bool Disabled
		{
			get
			{
				if (base.Disabled)
				{
					return true;
				}
				IDomContainer domContainer = this.GetAncestors().FirstOrDefault((IDomContainer c) => c.NodeName == "FIELDSET");
				if (domContainer == null || !domContainer.Disabled)
				{
					return false;
				}
				IDomElement domElement = domContainer.GetDescendentElements().FirstOrDefault((IDomElement o) => o.NodeName == "LEGEND");
				return domElement == null || !this.GetAncestors().Contains(domElement);
			}
			set
			{
				base.SetProp(47, value);
			}
		}
	}
}
