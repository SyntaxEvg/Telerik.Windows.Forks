using System;
using System.Xml;
using HtmlParserSharp.Common;
using HtmlParserSharp.Core;

namespace HtmlParserSharp
{
	class XmlTreeBuilder : CoalescingTreeBuilder<XmlElement>
	{
		protected override void AddAttributesToElement(XmlElement element, HtmlAttributes attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				string localName = attributes.GetLocalName(i);
				string uri = attributes.GetURI(i);
				if (!element.HasAttribute(localName, uri))
				{
					element.SetAttribute(localName, uri, attributes.GetValue(i));
				}
			}
		}

		protected override void AppendCharacters(XmlElement parent, string text)
		{
			XmlNode lastChild = parent.LastChild;
			if (lastChild != null && lastChild.NodeType == XmlNodeType.Text)
			{
				XmlText xmlText = (XmlText)lastChild;
				XmlText xmlText2 = xmlText;
				xmlText2.Data += text;
				return;
			}
			parent.AppendChild(this.document.CreateTextNode(text));
		}

		protected override void AppendChildrenToNewParent(XmlElement oldParent, XmlElement newParent)
		{
			while (oldParent.HasChildNodes)
			{
				newParent.AppendChild(oldParent.FirstChild);
			}
		}

		protected override void AppendDoctypeToDocument(string name, string publicIdentifier, string systemIdentifier)
		{
			this.document.XmlResolver = null;
			if (publicIdentifier == string.Empty)
			{
				publicIdentifier = null;
			}
			if (systemIdentifier == string.Empty)
			{
				systemIdentifier = null;
			}
			XmlDocumentType newChild = this.document.CreateDocumentType(name, publicIdentifier, systemIdentifier, null);
			this.document.XmlResolver = new XmlUrlResolver();
			this.document.AppendChild(newChild);
		}

		protected override void AppendComment(XmlElement parent, string comment)
		{
			parent.AppendChild(this.document.CreateComment(comment));
		}

		protected override void AppendCommentToDocument(string comment)
		{
			this.document.AppendChild(this.document.CreateComment(comment));
		}

		protected override global::System.Xml.XmlElement CreateElement(string ns, string name, global::HtmlParserSharp.Core.HtmlAttributes attributes)
		{
			global::System.Xml.XmlElement xmlElement = this.document.CreateElement(name, ns);
			for (int i = 0; i < attributes.Length; i++)
			{
				xmlElement.SetAttribute(attributes.GetLocalName(i), attributes.GetURI(i), attributes.GetValue(i));
				//attributes.GetType(i) == "ID";
			}
			return xmlElement;
		}

		protected override XmlElement CreateHtmlElementSetAsRoot(HtmlAttributes attributes)
		{
			XmlElement xmlElement = this.document.CreateElement("html", "http://www.w3.org/1999/xhtml");
			for (int i = 0; i < attributes.Length; i++)
			{
				xmlElement.SetAttribute(attributes.GetLocalName(i), attributes.GetURI(i), attributes.GetValue(i));
			}
			this.document.AppendChild(xmlElement);
			return xmlElement;
		}

		protected override void AppendElement(XmlElement child, XmlElement newParent)
		{
			newParent.AppendChild(child);
		}

		protected override bool HasChildren(XmlElement element)
		{
			return element.HasChildNodes;
		}

		protected override XmlElement CreateElement(string ns, string name, HtmlAttributes attributes, XmlElement form)
		{
			return this.CreateElement(ns, name, attributes);
		}

		protected override void Start(bool fragment)
		{
			this.document = new XmlDocument();
		}

		protected override void ReceiveDocumentMode(DocumentMode mode, string publicIdentifier, string systemIdentifier, bool html4SpecificAdditionalErrorChecks)
		{
		}

		internal XmlDocument Document
		{
			get
			{
				return this.document;
			}
		}

		internal XmlDocumentFragment getDocumentFragment()
		{
			XmlDocumentFragment xmlDocumentFragment = this.document.CreateDocumentFragment();
			XmlNode firstChild = this.document.FirstChild;
			while (firstChild.HasChildNodes)
			{
				xmlDocumentFragment.AppendChild(firstChild.FirstChild);
			}
			this.document = null;
			return xmlDocumentFragment;
		}

		protected override void InsertFosterParentedCharacters(string text, XmlElement table, XmlElement stackParent)
		{
			XmlNode parentNode = table.ParentNode;
			if (parentNode != null)
			{
				XmlNode previousSibling = table.PreviousSibling;
				if (previousSibling != null && previousSibling.NodeType == XmlNodeType.Text)
				{
					XmlText xmlText = (XmlText)previousSibling;
					XmlText xmlText2 = xmlText;
					xmlText2.Data += text;
					return;
				}
				parentNode.InsertBefore(this.document.CreateTextNode(text), table);
				return;
			}
			else
			{
				XmlNode lastChild = stackParent.LastChild;
				if (lastChild != null && lastChild.NodeType == XmlNodeType.Text)
				{
					XmlText xmlText3 = (XmlText)lastChild;
					XmlText xmlText4 = xmlText3;
					xmlText4.Data += text;
					return;
				}
				stackParent.AppendChild(this.document.CreateTextNode(text));
				return;
			}
		}

		protected override void InsertFosterParentedChild(XmlElement child, XmlElement table, XmlElement stackParent)
		{
			XmlNode parentNode = table.ParentNode;
			if (parentNode != null)
			{
				parentNode.InsertBefore(child, table);
				return;
			}
			stackParent.AppendChild(child);
		}

		protected override void DetachFromParent(XmlElement element)
		{
			XmlNode parentNode = element.ParentNode;
			if (parentNode != null)
			{
				parentNode.RemoveChild(element);
			}
		}

		XmlDocument document;
	}
}
