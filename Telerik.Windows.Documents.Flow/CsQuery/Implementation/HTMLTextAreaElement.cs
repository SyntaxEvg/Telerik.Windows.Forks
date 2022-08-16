using System;
using System.IO;
using CsQuery.Output;

namespace CsQuery.Implementation
{
	class HTMLTextAreaElement : FormSubmittableElement, IHTMLTextAreaElement, IDomElement, IDomContainer, IDomObject, IComparable<IDomObject>, IDomIndexedNode, IDomNode, ICloneable, IFormSubmittableElement, IFormReassociateableElement, IFormAssociatedElement
	{
		public HTMLTextAreaElement()
			: base(33)
		{
		}

		public override string Value
		{
			get
			{
				FormatDefault formatDefault = new FormatDefault();
				StringWriter stringWriter = new StringWriter();
				formatDefault.RenderChildren(this, stringWriter);
				return stringWriter.ToString();
			}
			set
			{
				this.ChildNodes.Clear();
				this.ChildNodes.Add(this.Document.CreateTextNode(value));
			}
		}

		public override string Type
		{
			get
			{
				return "textarea";
			}
		}

		public new string InnerText
		{
			get
			{
				return "";
			}
			set
			{
			}
		}
	}
}
