using System;

namespace CsQuery.Implementation
{
	abstract class FormAssociatedElement : DomElement, IFormAssociatedElement
	{
		protected FormAssociatedElement(ushort tokenId)
			: base(tokenId)
		{
		}

		public IHTMLFormElement Form
		{
			get
			{
				return base.Closest(41) as IHTMLFormElement;
			}
		}
	}
}
