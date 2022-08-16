using System;
using CsQuery.Engine;
using CsQuery.Implementation;
using HtmlParserSharp.Common;
using HtmlParserSharp.Core;

namespace CsQuery.HtmlParser
{
	class CsQueryTreeBuilder : CoalescingTreeBuilder<DomObject>
	{
		public CsQueryTreeBuilder(IDomIndexProvider domIndexProvider)
		{
			this.DomIndexProvider = domIndexProvider;
		}

		protected override void AddAttributesToElement(DomObject element, HtmlAttributes attributes)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				string name = this.AttributeName(attributes.GetLocalName(i), attributes.GetURI(i));
				element.SetAttribute(name, attributes.GetValue(i));
			}
		}

		protected override void AppendCharacters(DomObject parent, string text)
		{
			IDomText domText = parent.LastChild as IDomText;
			if (domText != null)
			{
				IDomText domText2 = domText;
				domText2.NodeValue += text;
				return;
			}
			domText = this.Document.CreateTextNode(text);
			parent.AppendChildUnsafe(domText);
		}

		protected override void AppendChildrenToNewParent(DomObject oldParent, DomObject newParent)
		{
			while (oldParent.HasChildren)
			{
				newParent.AppendChild(oldParent.FirstChild);
			}
		}

		protected override void AppendDoctypeToDocument(string name, string fpi, string uri)
		{
			IDomDocumentType element = this.Document.CreateDocumentType(name, "PUBLIC", fpi, uri);
			this.Document.AppendChildUnsafe(element);
		}

		protected override void AppendComment(DomObject parent, string comment)
		{
			parent.AppendChildUnsafe(new DomComment(comment));
		}

		protected override void AppendCommentToDocument(string comment)
		{
			this.Document.AppendChildUnsafe(this.Document.CreateComment(comment));
		}

		protected override DomObject CreateElement(string ns, string name, HtmlAttributes attributes)
		{
			DomElement domElement = DomElement.Create(name);
			for (int i = 0; i < attributes.Length; i++)
			{
				string name2 = this.AttributeName(attributes.GetLocalName(i), attributes.GetURI(i));
				domElement.SetAttribute(name2, attributes.GetValue(i));
			}
			return domElement;
		}

		protected override DomObject CreateHtmlElementSetAsRoot(HtmlAttributes attributes)
		{
			if (!this.isFragment)
			{
				DomElement domElement = DomElement.Create("html");
				for (int i = 0; i < attributes.Length; i++)
				{
					string name = this.AttributeName(attributes.GetLocalName(i), attributes.GetURI(i));
					domElement.SetAttribute(name, attributes.GetValue(i));
				}
				this.Document.AppendChildUnsafe(domElement);
				return domElement;
			}
			return this.Document;
		}

		protected override void AppendElement(DomObject child, DomObject newParent)
		{
			newParent.AppendChildUnsafe(child);
		}

		protected override bool HasChildren(DomObject element)
		{
			return element.HasChildren;
		}

		protected override DomObject CreateElement(string ns, string name, HtmlAttributes attributes, DomObject form)
		{
			return this.CreateElement(ns, name, attributes);
		}

		protected override void Start(bool fragment)
		{
			this.isFragment = fragment;
			if (this.Document == null)
			{
				this.Document = (fragment ? new DomFragment(this.DomIndexProvider.GetDomIndex()) : new DomDocument(this.DomIndexProvider.GetDomIndex()));
			}
			IDomIndexQueue domIndexQueue = this.Document.DocumentIndex as IDomIndexQueue;
			if (domIndexQueue != null)
			{
				domIndexQueue.QueueChanges = false;
			}
		}

		protected override void ReceiveDocumentMode(DocumentMode mode, string publicIdentifier, string systemIdentifier, bool html4SpecificAddcionalErrorChecks)
		{
		}

		protected override void InsertFosterParentedCharacters(string text, DomObject table, DomObject stackParent)
		{
			IDomObject parentNode = table.ParentNode;
			if (parentNode != null)
			{
				IDomObject previousSibling = table.PreviousSibling;
				if (previousSibling != null && previousSibling.NodeType == NodeType.TEXT_NODE)
				{
					IDomText domText = (IDomText)previousSibling;
					IDomText domText2 = domText;
					domText2.NodeValue += text;
					return;
				}
				parentNode.InsertBefore(this.Document.CreateTextNode(text), table);
				return;
			}
			else
			{
				IDomText domText3 = stackParent.LastChild as IDomText;
				if (domText3 != null)
				{
					IDomText domText4 = domText3;
					domText4.NodeValue += text;
					return;
				}
				stackParent.AppendChildUnsafe(this.Document.CreateTextNode(text));
				return;
			}
		}

		protected override void InsertFosterParentedChild(DomObject child, DomObject table, DomObject stackParent)
		{
			IDomObject parentNode = table.ParentNode;
			if (parentNode != null)
			{
				parentNode.InsertBefore(child, table);
				return;
			}
			stackParent.AppendChildUnsafe(child);
		}

		protected override void DetachFromParent(DomObject element)
		{
			IDomObject parentNode = element.ParentNode;
			if (parentNode != null)
			{
				parentNode.RemoveChild(element);
			}
		}

		string AttributeName(string localName, string uri)
		{
			if (!string.IsNullOrEmpty(uri))
			{
				return localName = localName + ":" + uri;
			}
			return localName;
		}

		internal DomDocument Document;

		IDomIndexProvider DomIndexProvider;

		bool isFragment;
	}
}
