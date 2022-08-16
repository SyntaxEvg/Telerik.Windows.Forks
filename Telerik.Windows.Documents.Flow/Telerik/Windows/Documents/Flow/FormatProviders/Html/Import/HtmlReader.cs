using System;
using System.Collections.Generic;
using CsQuery;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Import
{
	class HtmlReader : IHtmlReader
	{
		public HtmlReader(IDomObject domObject)
		{
			this.domObject = domObject;
			this.domObjectsStack = new Stack<IDomObject>();
		}

		public HtmlReader(CQ cq)
			: this(HtmlReader.GetFirstDomObject(cq))
		{
		}

		IDomObject CurrentObject
		{
			get
			{
				return this.domObjectsStack.Peek();
			}
		}

		public bool BeginReadElement()
		{
			if (this.domObject != null)
			{
				if (this.domObject.NodeName.ToLowerInvariant() == "span")
				{
					this.isInSpan = !this.isInSpan;
				}
				this.SetCurrentDomObject(this.domObject);
				return true;
			}
			return false;
		}

		public bool MoveToNextAttribute()
		{
			return this.attribute != null && this.attribute.MoveNext();
		}

		public bool IsInsideSpanElement()
		{
			return this.isInSpan;
		}

		public bool HasChildElements()
		{
			return this.domObject != null;
		}

		public HtmlElementType GetCurrentChildElementType()
		{
			if (HtmlReader.IsTextElement(this.domObject))
			{
				return HtmlElementType.Text;
			}
			return HtmlElementType.Element;
		}

		public string GetCurrentChildElementName()
		{
			return this.domObject.NodeName.ToLowerInvariant();
		}

		public string GetAttributeName()
		{
			KeyValuePair<string, string> keyValuePair = this.attribute.Current;
			return keyValuePair.Key.ToLowerInvariant();
		}

		public string GetAttributeValue()
		{
			KeyValuePair<string, string> keyValuePair = this.attribute.Current;
			return keyValuePair.Value;
		}

		public string GetInnerText()
		{
			if (HtmlReader.IsTextElement(this.CurrentObject))
			{
				return this.CurrentObject.NodeValue;
			}
			return string.Empty;
		}

		public bool MoveToNextChildElement()
		{
			if (this.domObject != null)
			{
				this.domObject = this.domObject.NextSibling;
			}
			return this.domObject != null;
		}

		public string GetElementName()
		{
			return this.CurrentObject.NodeName.ToLowerInvariant();
		}

		public void EndReadElement()
		{
			this.domObject = this.domObjectsStack.Pop();
			if (this.domObject.NodeName.ToLowerInvariant() == "span")
			{
				this.isInSpan = !this.isInSpan;
			}
		}

		static bool IsTextElement(IDomObject element)
		{
			return element != null && element.NodeType == NodeType.TEXT_NODE;
		}

		static IDomObject GetFirstDomObject(CQ dom)
		{
			IDomObject domObject = dom.Document.FirstChild;
			while (domObject != null && domObject.NodeType == NodeType.DOCUMENT_TYPE_NODE)
			{
				domObject = domObject.NextSibling;
			}
			return domObject;
		}

		void SetCurrentDomObject(IDomObject node)
		{
			Guard.ThrowExceptionIfNull<IDomObject>(node, "node");
			this.domObjectsStack.Push(node);
			if (this.domObject.HasAttributes)
			{
				this.attribute = this.domObject.Attributes.GetEnumerator();
			}
			else
			{
				this.attribute = null;
			}
			this.domObject = this.domObject.FirstChild;
		}

		readonly Stack<IDomObject> domObjectsStack;

		bool isInSpan;

		IDomObject domObject;

		IEnumerator<KeyValuePair<string, string>> attribute;
	}
}
