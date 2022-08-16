using System;
using CsQuery.HtmlParser;

namespace CsQuery.Implementation
{
	class HTMLLabelElement : FormAssociatedElement, IHTMLLabelElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormReassociateableElement, IFormAssociatedElement
	{
		public HTMLLabelElement()
			: base(46)
		{
		}

		public string HtmlFor
		{
			get
			{
				return this.GetAttribute("for");
			}
			set
			{
				this.SetAttribute("for", value);
			}
		}

		public IDomElement Control
		{
			get
			{
				string htmlFor = this.HtmlFor;
				if (!string.IsNullOrEmpty(htmlFor))
				{
					return this.Document.GetElementById(htmlFor);
				}
				foreach (IDomElement domElement in base.DescendantElements())
				{
					if (HtmlData.IsFormInputControl(domElement.NodeNameID))
					{
						return domElement;
					}
				}
				return null;
			}
		}
	}
}
