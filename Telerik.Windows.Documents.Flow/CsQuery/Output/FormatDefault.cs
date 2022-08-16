using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery.HtmlParser;

namespace CsQuery.Output
{
	class FormatDefault : IOutputFormatter
	{
		public FormatDefault(DomRenderingOptions options, IHtmlEncoder encoder)
		{
			this.DomRenderingOptions = options;
			this.MergeDefaultOptions();
			this.HtmlEncoder = encoder ?? HtmlEncoders.Default;
		}

		public FormatDefault()
			: this(DomRenderingOptions.Default, HtmlEncoders.Default)
		{
		}

		protected Stack<FormatDefault.NodeStackElement> OutputStack
		{
			get
			{
				if (this._OutputStack == null)
				{
					this._OutputStack = new Stack<FormatDefault.NodeStackElement>();
				}
				return this._OutputStack;
			}
		}

		public void Render(IDomObject node, TextWriter writer)
		{
			this.SetDocType(node);
			this.RenderInternal(node, writer);
		}

		public string Render(IDomObject node)
		{
			this.SetDocType(node);
			StringBuilder stringBuilder = new StringBuilder();
			StringWriter writer = new StringWriter(stringBuilder);
			if (node is IDomDocument)
			{
				this.RenderChildrenInternal(node, writer);
			}
			else
			{
				this.Render(node, writer);
			}
			return stringBuilder.ToString();
		}

		public virtual void RenderElement(IDomObject element, TextWriter writer, bool includeChildren)
		{
			this.SetDocType(element);
			this.RenderElementInternal(element, writer, includeChildren);
			this.RenderStack(writer);
		}

		public void RenderChildren(IDomObject element, TextWriter writer)
		{
			this.SetDocType(element);
			this.RenderChildrenInternal(element, writer);
		}

		void RenderInternal(IDomObject node, TextWriter writer)
		{
			this.OutputStack.Push(new FormatDefault.NodeStackElement(node, false, false));
			this.RenderStack(writer);
		}

		void RenderChildrenInternal(IDomObject element, TextWriter writer)
		{
			if (element.HasChildren)
			{
				this.ParseChildren(element);
			}
			else
			{
				this.OutputStack.Push(new FormatDefault.NodeStackElement(element, false, false));
			}
			this.RenderStack(writer);
		}

		protected virtual void RenderElementInternal(IDomObject element, TextWriter writer, bool includeChildren)
		{
			bool quoteAll = this.DomRenderingOptions.HasFlag(DomRenderingOptions.QuoteAllAttributes);
			writer.Write("<");
			writer.Write(element.NodeName.ToLower());
			if (element.HasAttributes)
			{
				foreach (KeyValuePair<string, string> keyValuePair in element.Attributes)
				{
					writer.Write(" ");
					this.RenderAttribute(writer, keyValuePair.Key, keyValuePair.Value, quoteAll);
				}
			}
			if (!element.InnerHtmlAllowed && !element.InnerTextAllowed)
			{
				writer.Write(this.IsXHTML ? " />" : ">");
				return;
			}
			writer.Write(">");
			this.EndElement(element);
			if (includeChildren)
			{
				this.ParseChildren(element);
				return;
			}
			writer.Write(element.HasChildren ? "..." : string.Empty);
		}

		protected virtual void EndElement(IDomObject element)
		{
			this.OutputStack.Push(new FormatDefault.NodeStackElement(element, false, true));
		}

		protected void RenderStack(TextWriter writer)
		{
			while (this.OutputStack.Count > 0)
			{
				FormatDefault.NodeStackElement nodeStackElement = this.OutputStack.Pop();
				IDomObject element = nodeStackElement.Element;
				if (!nodeStackElement.IsClose)
				{
					switch (element.NodeType)
					{
					case NodeType.ELEMENT_NODE:
						this.RenderElementInternal(element, writer, true);
						continue;
					case NodeType.TEXT_NODE:
						this.RenderTextNode(element, writer, nodeStackElement.IsRaw);
						continue;
					case NodeType.CDATA_SECTION_NODE:
						this.RenderCdataNode(element, writer);
						continue;
					case NodeType.COMMENT_NODE:
						this.RenderCommentNode(element, writer);
						continue;
					case NodeType.DOCUMENT_NODE:
					case NodeType.DOCUMENT_FRAGMENT_NODE:
						this.RenderElements(element.ChildNodes, writer);
						continue;
					case NodeType.DOCUMENT_TYPE_NODE:
						this.RenderDocTypeNode(element, writer);
						continue;
					}
					throw new NotImplementedException("An unknown node type was found while rendering the CsQuery document.");
				}
				this.RenderElementCloseTag(element, writer);
			}
		}

		protected void RenderElements(IEnumerable<IDomObject> elements, TextWriter writer)
		{
			foreach (IDomObject node in elements)
			{
				this.Render(node, writer);
			}
		}

		protected virtual void RenderElementCloseTag(IDomObject element, TextWriter writer)
		{
			writer.Write("</");
			writer.Write(element.NodeName.ToLower());
			writer.Write(">");
		}

		protected virtual void ParseChildren(IDomObject element)
		{
			if (element.HasChildren)
			{
				foreach (IDomObject domObject in element.ChildNodes.Reverse<IDomObject>())
				{
					FormatDefault.NodeStackElement item = new FormatDefault.NodeStackElement(domObject, domObject.NodeType == NodeType.TEXT_NODE && HtmlData.HtmlChildrenNotAllowed(element.NodeNameID), false);
					this.OutputStack.Push(item);
				}
			}
		}

		protected virtual void RenderTextNode(IDomObject textNode, TextWriter writer, bool raw)
		{
			if (raw)
			{
				writer.Write(textNode.NodeValue);
				return;
			}
			this.HtmlEncoder.Encode(textNode.NodeValue, writer);
		}

		protected void RenderCdataNode(IDomObject element, TextWriter writer)
		{
			writer.Write("<![CDATA[" + element.NodeValue + ">");
		}

		protected void RenderCommentNode(IDomObject element, TextWriter writer)
		{
			if (this.DomRenderingOptions.HasFlag(DomRenderingOptions.RemoveComments))
			{
				return;
			}
			writer.Write("<!--" + element.NodeValue + "-->");
		}

		protected void RenderDocTypeNode(IDomObject element, TextWriter writer)
		{
			writer.Write("<!DOCTYPE " + ((IDomSpecialElement)element).NonAttributeData + ">");
		}

		protected void RenderAttribute(TextWriter writer, string name, string value, bool quoteAll)
		{
			if (value != null && value != "")
			{
				string value3;
				string value2 = HtmlData.AttributeEncode(value, quoteAll, out value3);
				writer.Write(name.ToLower());
				writer.Write("=");
				writer.Write(value3);
				writer.Write(value2);
				writer.Write(value3);
				return;
			}
			writer.Write(name);
		}

		protected void MergeDefaultOptions()
		{
			if (this.DomRenderingOptions.HasFlag(DomRenderingOptions.Default))
			{
				this.DomRenderingOptions = Config.DomRenderingOptions | (this.DomRenderingOptions & ~DomRenderingOptions.Default);
			}
		}

		protected void SetDocType(IDomObject element)
		{
			DocType docType = ((element.Document == null) ? Config.DocType : element.Document.DocType);
			this.IsXHTML = docType == DocType.XHTML || docType == DocType.XHTMLStrict;
		}

		DomRenderingOptions DomRenderingOptions;

		IHtmlEncoder HtmlEncoder;

		Stack<FormatDefault.NodeStackElement> _OutputStack;

		bool IsXHTML;

		protected class NodeStackElement
		{
			public NodeStackElement(IDomObject element, bool isRaw, bool isClose)
			{
				this.Element = element;
				this.IsRaw = isRaw;
				this.IsClose = isClose;
			}

			public IDomObject Element;

			public bool IsRaw;

			public bool IsClose;
		}
	}
}
