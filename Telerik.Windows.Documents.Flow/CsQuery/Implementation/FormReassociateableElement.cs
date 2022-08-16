using System;

namespace CsQuery.Implementation
{
	abstract class FormReassociateableElement : DomElement, IFormReassociateableElement, IFormAssociatedElement
	{
		protected FormReassociateableElement(ushort tokenId)
			: base(tokenId)
		{
		}

		public IHTMLFormElement Form
		{
			get
			{
				string attribute = base.GetAttribute(41);
				IHTMLFormElement ihtmlformElement = null;
				if (!string.IsNullOrEmpty(attribute))
				{
					ihtmlformElement = this.Document.GetElementById(attribute) as IHTMLFormElement;
				}
				return ihtmlformElement ?? (base.Closest(41) as IHTMLFormElement);
			}
		}
	}
}
