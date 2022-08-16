using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery.Engine;
using CsQuery.HtmlParser;

namespace CsQuery.Implementation
{
	class DomDocument : DomContainer<DomDocument>, IDomDocument, IDomContainer, IDomObject, IDomNode, ICloneable, IComparable<IDomObject>
	{
		public static IDomDocument Create()
		{
			return new DomDocument();
		}

		public static IDomDocument Create(IEnumerable<IDomObject> elements, HtmlParsingMode parsingMode = HtmlParsingMode.Content, DocType docType = DocType.Default)
		{
			DomDocument domDocument = ((parsingMode == HtmlParsingMode.Document) ? new DomDocument() : new DomFragment());
			if (parsingMode == HtmlParsingMode.Document)
			{
				domDocument.DocType = docType;
			}
			domDocument.Populate(elements);
			return domDocument;
		}

		public static IDomDocument Create(string html, HtmlParsingMode parsingMode = HtmlParsingMode.Auto, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			Encoding utf = Encoding.UTF8;
			IDomDocument result;
			using (MemoryStream memoryStream = new MemoryStream(utf.GetBytes(html)))
			{
				result = ElementFactory.Create(memoryStream, utf, parsingMode, parsingOptions, docType);
			}
			return result;
		}

		public static IDomDocument Create(Stream html, Encoding encoding = null, HtmlParsingMode parsingMode = HtmlParsingMode.Content, HtmlParsingOptions parsingOptions = HtmlParsingOptions.Default, DocType docType = DocType.Default)
		{
			return ElementFactory.Create(html, encoding, parsingMode, parsingOptions, docType);
		}

		public DomDocument()
			: this(null)
		{
		}

		public DomDocument(IDomIndex domIndex)
		{
			this.DocumentIndex = domIndex ?? Config.DomIndexProvider.GetDomIndex();
		}

		protected void Populate(IEnumerable<IDomObject> elements)
		{
			foreach (IDomObject item in elements)
			{
				base.ChildNodesInternal.AddAlways(item);
			}
		}

		public IList<ICSSStyleSheet> StyleSheets
		{
			get
			{
				if (this._StyleSheets == null)
				{
					this._StyleSheets = new List<ICSSStyleSheet>();
				}
				return this._StyleSheets;
			}
		}

		public IDomIndex DocumentIndex { get; protected set; }

		public override IDomContainer ParentNode
		{
			get
			{
				return null;
			}
			internal set
			{
				throw new InvalidOperationException("Cannot set parent for a DOM root node.");
			}
		}

		public override ushort[] NodePath
		{
			get
			{
				return new ushort[0];
			}
		}

		[Obsolete]
		public override string Path
		{
			get
			{
				return "";
			}
		}

		public override int Depth
		{
			get
			{
				return 0;
			}
		}

		[Obsolete]
		public DomRenderingOptions DomRenderingOptions { get; set; }

		public override IDomDocument Document
		{
			get
			{
				return this;
			}
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.DOCUMENT_NODE;
			}
		}

		public IDomDocumentType DocTypeNode
		{
			get
			{
				foreach (IDomObject domObject in this.ChildNodes)
				{
					if (domObject.NodeType == NodeType.DOCUMENT_TYPE_NODE)
					{
						return (DomDocumentType)domObject;
					}
				}
				return null;
			}
			set
			{
				IDomDocumentType docTypeNode = this.DocTypeNode;
				if (docTypeNode != null)
				{
					docTypeNode.Remove();
				}
				this.ChildNodes.Insert(0, value);
			}
		}

		public DocType DocType
		{
			get
			{
				IDomDocumentType docTypeNode = this.DocTypeNode;
				if (docTypeNode == null)
				{
					return Config.DocType;
				}
				return docTypeNode.DocType;
			}
			protected set
			{
				IDomDocumentType docTypeNode = this.DocTypeNode;
				if (docTypeNode != null)
				{
					docTypeNode.Remove();
				}
				this.AppendChild(this.CreateDocumentType(value));
			}
		}

		public override bool InnerHtmlAllowed
		{
			get
			{
				return true;
			}
		}

		public IDictionary<string, object> Data
		{
			get
			{
				if (this._Data == null)
				{
					this._Data = new Dictionary<string, object>();
				}
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		public IDomElement Body
		{
			get
			{
				return this.QuerySelectorAll("body").FirstOrDefault<IDomElement>();
			}
		}

		public override bool IsIndexed
		{
			get
			{
				return true;
			}
		}

		public override bool IsFragment
		{
			get
			{
				return false;
			}
		}

		public override bool IsDisconnected
		{
			get
			{
				return false;
			}
		}

		public IDomElement GetElementById(string id)
		{
			return this.GetElementById<IDomElement>(id);
		}

		public T GetElementById<T>(string id) where T : IDomElement
		{
			Selector selector = new Selector(new SelectorClause
			{
				SelectorType = SelectorType.ID,
				ID = id
			});
			return (T)((object)selector.Select(this.Document).FirstOrDefault<IDomObject>());
		}

		public IDomElement GetElementByTagName(string tagName)
		{
			Selector selector = new Selector(tagName);
			return (IDomElement)selector.Select(this.Document).FirstOrDefault<IDomObject>();
		}

		public INodeList<IDomElement> GetElementsByTagName(string tagName)
		{
			Selector selector = new Selector(tagName);
			return new NodeList<IDomElement>(new List<IDomElement>(DomDocument.OnlyElements(selector.Select(this.Document))));
		}

		public IDomElement QuerySelector(string selector)
		{
			Selector selector2 = new Selector(selector);
			return DomDocument.OnlyElements(selector2.Select(this.Document)).FirstOrDefault<IDomElement>();
		}

		public IList<IDomElement> QuerySelectorAll(string selector)
		{
			Selector selector2 = new Selector(selector);
			return new List<IDomElement>(DomDocument.OnlyElements(selector2.Select(this.Document))).AsReadOnly();
		}

		public IDomElement CreateElement(string nodeName)
		{
			return DomElement.Create(nodeName);
		}

		public IDomText CreateTextNode(string text)
		{
			return new DomText(text);
		}

		public IDomComment CreateComment(string comment)
		{
			return new DomComment(comment);
		}

		public IDomDocumentType CreateDocumentType(string type, string access, string FPI, string URI)
		{
			return new DomDocumentType(type, access, FPI, URI);
		}

		public IDomDocumentType CreateDocumentType(DocType docType)
		{
			return new DomDocumentType(docType);
		}

		public override DomDocument Clone()
		{
			return new DomDocument();
		}

		public override IEnumerable<IDomObject> CloneChildren()
		{
			if (this.HasChildren)
			{
				foreach (IDomObject obj in this.ChildNodes)
				{
					yield return obj.Clone();
				}
			}
			yield break;
		}

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"DOM Root (",
				this.DocType.ToString(),
				", ",
				this.DescendantCount().ToString(),
				" elements)"
			});
		}

		public IDomDocument CreateNew<T>() where T : IDomDocument
		{
			return this.CreateNew(typeof(T));
		}

		public virtual IDomDocument CreateNew()
		{
			return this.CreateNew<IDomDocument>();
		}

		public IDomDocument CreateNew<T>(IEnumerable<IDomObject> elements) where T : IDomDocument
		{
			IDomDocument result;
			if (typeof(T) == typeof(IDomDocument))
			{
				result = DomDocument.Create(elements, HtmlParsingMode.Document, DocType.Default);
			}
			else
			{
				if (!(typeof(T) == typeof(IDomFragment)))
				{
					throw new ArgumentException(string.Format("I don't know about an IDomDocument subclass \"{1}\"", typeof(T).ToString()));
				}
				result = DomDocument.Create(elements, HtmlParsingMode.Fragment, DocType.Default);
			}
			return result;
		}

		IDomDocument CreateNew(Type t)
		{
			IDomDocument result;
			if (t == typeof(IDomDocument))
			{
				result = new DomDocument();
			}
			else
			{
				if (!(t == typeof(IDomFragment)))
				{
					throw new ArgumentException(string.Format("I don't know about an IDomDocument subclass \"{1}\"", t.ToString()));
				}
				result = new DomFragment();
			}
			return result;
		}

		protected static IEnumerable<IDomElement> OnlyElements(IEnumerable<IDomObject> objectList)
		{
			foreach (IDomObject obj in objectList)
			{
				if (obj.NodeType == NodeType.ELEMENT_NODE)
				{
					yield return (IDomElement)obj;
				}
			}
			yield break;
		}

		IList<ICSSStyleSheet> _StyleSheets;

		IDictionary<string, object> _Data;
	}
}
