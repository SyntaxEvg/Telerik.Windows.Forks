using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery.ExtensionMethods.Internal;
using CsQuery.HtmlParser;
using CsQuery.Output;
using CsQuery.StringScanner;

namespace CsQuery.Implementation
{
	class DomElement : DomContainer<DomElement>, IDomElement, IDomContainer, IDomIndexedNode, IDomObject, IComparable<IDomObject>, IDomNode, ICloneable, IAttributeCollection, IEnumerable<KeyValuePair<string, string>>, IEnumerable
	{
		protected AttributeCollection InnerAttributes
		{
			get
			{
				if (this._InnerAttributes == null)
				{
					this._InnerAttributes = new AttributeCollection();
				}
				return this._InnerAttributes;
			}
			set
			{
				this._InnerAttributes = value;
			}
		}

		public bool HasInnerAttributes
		{
			get
			{
				return this._InnerAttributes != null && this._InnerAttributes.HasAttributes;
			}
		}

		public DomElement()
		{
			throw new InvalidOperationException("You can't create a DOM element using the default constructor. Please use the DomElement.Create static method instead.");
		}

		protected DomElement(ushort tokenId)
		{
			this._NodeNameID = tokenId;
		}

		public static DomElement Create(string nodeName)
		{
			return DomElement.Create(HtmlData.Tokenize(nodeName));
		}

		internal static DomElement Create(ushort nodeNameId)
		{
			switch (nodeNameId)
			{
			case 9:
				return new HTMLInputElement();
			case 10:
				return new HTMLSelectElement();
			case 11:
				return new HTMLOptionElement();
			default:
				if (nodeNameId != 21)
				{
					switch (nodeNameId)
					{
					case 32:
						return new HTMLScriptElement();
					case 33:
						return new HTMLTextAreaElement();
					case 34:
						return new HTMLStyleElement();
					case 37:
						return new HTMLButtonElement();
					case 39:
						return new HtmlAnchorElement();
					case 41:
						return new HtmlFormElement();
					case 45:
						return new HTMLProgressElement();
					case 46:
						return new HTMLLabelElement();
					case 48:
						return new HTMLMeterElement();
					}
					return new DomElement(nodeNameId);
				}
				return new HTMLLIElement();
			}
		}

		public override CSSStyleDeclaration Style
		{
			get
			{
				if (this._Style == null)
				{
					this.SetStyle(new CSSStyleDeclaration());
				}
				return this._Style;
			}
			set
			{
				bool hasStyleAttribute = this.HasStyleAttribute;
				this.SetStyle(value);
				if (this.HasStyleAttribute != hasStyleAttribute)
				{
					this.StyleAttributeIndexChanged();
				}
			}
		}

		void SetStyle(CSSStyleDeclaration style)
		{
			this._Style = style;
			if (style != null)
			{
				this._Style.OnHasStylesChanged += this._Style_OnHasStylesChanged;
			}
		}

		void _Style_OnHasStylesChanged(object sender, CSSStyleChangedArgs e)
		{
			this.StyleAttributeIndexChanged();
		}

		void StyleAttributeIndexChanged()
		{
			if (this.HasStyleAttribute)
			{
				this.AttributeAddToIndex(34);
				return;
			}
			this.AttributeRemoveFromIndex(34);
		}

		public override IAttributeCollection Attributes
		{
			get
			{
				return this;
			}
		}

		public override string ClassName
		{
			get
			{
				if (this.HasClasses)
				{
					string text = "";
					foreach (ushort tokenId in this._Classes)
					{
						text = text + ((text == "") ? "" : " ") + HtmlData.TokenName(tokenId);
					}
					return text;
				}
				return "";
			}
			set
			{
				this.SetClassName(value);
			}
		}

		public override string Id
		{
			get
			{
				return this.GetAttribute(5, string.Empty);
			}
			set
			{
				if (!this.IsFragment)
				{
					if (this.InnerAttributes.ContainsKey(5))
					{
						this.Document.DocumentIndex.RemoveFromIndex(this.IDIndexKey(this.Id), this);
					}
					if (value != null)
					{
						this.Document.DocumentIndex.AddToIndex(this.IDIndexKey(this.Id), this);
					}
				}
				this.SetAttributeRaw(5, value);
			}
		}

		public override string NodeName
		{
			get
			{
				return HtmlData.TokenName(this._NodeNameID).ToUpper();
			}
		}

		public override ushort NodeNameID
		{
			get
			{
				return this._NodeNameID;
			}
		}

		public override string Type
		{
			get
			{
				return this.GetAttribute(44);
			}
			set
			{
				this.SetAttribute(44, value);
			}
		}

		public override string Name
		{
			get
			{
				return this.GetAttribute("name");
			}
			set
			{
				this.SetAttribute("name", value);
			}
		}

		public override string DefaultValue
		{
			get
			{
				if (!this.hasDefaultValue())
				{
					return base.DefaultValue;
				}
				if (this.NodeNameID != 33)
				{
					return this.GetAttribute("value");
				}
				return this.TextContent;
			}
			set
			{
				if (!this.hasDefaultValue())
				{
					base.DefaultValue = value;
					return;
				}
				if (this.NodeNameID == 33)
				{
					this.TextContent = value;
					return;
				}
				this.SetAttribute("value", value);
			}
		}

		public override string Value
		{
			get
			{
				if (!HtmlData.HasValueProperty(this.NodeNameID))
				{
					return null;
				}
				return this.GetAttribute(4);
			}
			set
			{
				this.SetAttribute(4, value);
			}
		}

		public override NodeType NodeType
		{
			get
			{
				return NodeType.ELEMENT_NODE;
			}
		}

		public override IDomContainer ParentNode
		{
			get
			{
				return base.ParentNode;
			}
			internal set
			{
				base.ParentNode = value;
			}
		}

		public override bool HasAttributes
		{
			get
			{
				return this.HasClasses || this.HasStyleAttribute || this.HasInnerAttributes;
			}
		}

		public override bool HasStyles
		{
			get
			{
				return this.HasStyleAttribute && this._Style.HasStyles;
			}
		}

		protected bool HasStyleAttribute
		{
			get
			{
				return this._Style != null && this._Style.HasStyleAttribute;
			}
		}

		public override bool HasClasses
		{
			get
			{
				return this._Classes != null && this._Classes.Count > 0;
			}
		}

		public override bool IsIndexed
		{
			get
			{
				return (byte)(this.DocInfo & DomObject.DocumentInfo.IsIndexed) != 0;
			}
		}

		public override string OuterHTML
		{
			get
			{
				FormatDefault formatDefault = new FormatDefault();
				StringWriter stringWriter = new StringWriter();
				formatDefault.RenderElement(this, stringWriter, true);
				return stringWriter.ToString();
			}
			set
			{
				CQ cq = CQ.CreateFragment(value, this.NodeName);
				int index = base.Index;
				IDomContainer parentNode = this.ParentNode;
				this.Remove();
				List<IDomObject> list = cq.Document.ChildNodes.ToList<IDomObject>();
				foreach (IDomObject item in list)
				{
					parentNode.ChildNodes.Insert(index++, item);
				}
			}
		}

		public override bool InnerHtmlAllowed
		{
			get
			{
				return !HtmlData.HtmlChildrenNotAllowed(this._NodeNameID);
			}
		}

		public override bool InnerTextAllowed
		{
			get
			{
				return HtmlData.ChildrenAllowed(this._NodeNameID);
			}
		}

		public override bool ChildrenAllowed
		{
			get
			{
				return HtmlData.ChildrenAllowed(this._NodeNameID);
			}
		}

		public override string this[string attribute]
		{
			get
			{
				return this.GetAttribute(attribute);
			}
			set
			{
				this.SetAttribute(attribute, value);
			}
		}

		public override bool Checked
		{
			get
			{
				return this.HasAttribute(8);
			}
			set
			{
				this.SetAttribute(8, value ? "" : null);
			}
		}

		public override bool Disabled
		{
			get
			{
				return this.HasAttribute(47);
			}
			set
			{
				this.SetAttribute(47, value ? "" : null);
			}
		}

		public override bool ReadOnly
		{
			get
			{
				return this.HasAttribute(7);
			}
			set
			{
				this.SetAttribute(7, value ? "" : null);
			}
		}

		public override string InnerHTML
		{
			get
			{
				if (!this.HasChildren)
				{
					return string.Empty;
				}
				FormatDefault formatDefault = new FormatDefault();
				StringBuilder stringBuilder = new StringBuilder();
				StringWriter writer = new StringWriter(stringBuilder);
				formatDefault.RenderChildren(this, writer);
				return stringBuilder.ToString();
			}
			set
			{
				if (!this.InnerHtmlAllowed)
				{
					throw new InvalidOperationException(string.Format("You can't set the innerHTML for a {0} element.", this.NodeName));
				}
				this.ChildNodes.Clear();
				CQ cq = CQ.CreateFragment(value, this.NodeName);
				this.ChildNodes.AddRange(cq.Document.ChildNodes);
			}
		}

		public override string TextContent
		{
			get
			{
				if (!this.HasChildren)
				{
					return string.Empty;
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in this.GetTextContent(this.ChildNodes))
				{
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
			set
			{
				if (!this.InnerTextAllowed)
				{
					throw new InvalidOperationException(string.Format("You can't set the InnerText for a {0} element.", this.NodeName));
				}
				this.ChildNodes.Clear();
				this.ChildNodes.Add(new DomText(value));
			}
		}

		public override string InnerText
		{
			get
			{
				if (!this.HasChildren)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in this.GetInnerText(this.ChildNodes))
				{
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
			set
			{
				this.TextContent = value;
			}
		}

		public override int ElementIndex
		{
			get
			{
				int num = -1;
				IDomElement domElement = this;
				while (domElement != null)
				{
					domElement = domElement.PreviousElementSibling;
					num++;
				}
				return num;
			}
		}

		public override IDomObject IndexReference
		{
			get
			{
				return this;
			}
		}

		public override bool IsBlock
		{
			get
			{
				return HtmlData.IsBlock(this._NodeNameID);
			}
		}

		public override IEnumerable<string> Classes
		{
			get
			{
				if (this.HasClasses)
				{
					foreach (ushort id in this._Classes)
					{
						yield return HtmlData.TokenName(id);
					}
				}
				yield break;
			}
		}

		public override string ElementHtml()
		{
			FormatDefault formatDefault = new FormatDefault();
			StringWriter stringWriter = new StringWriter();
			formatDefault.RenderElement(this, stringWriter, false);
			return stringWriter.ToString();
		}

		public override IEnumerable<ushort[]> IndexKeys()
		{
			yield return new ushort[] { 43, this._NodeNameID };
			string id = this.Id;
			if (!string.IsNullOrEmpty(id))
			{
				yield return this.IDIndexKey(id);
			}
			if (this.HasClasses)
			{
				foreach (ushort clsId in this._Classes)
				{
					yield return this.ClassIndexKey(clsId);
				}
			}
			if (this.HasClasses)
			{
				yield return this.AttributeIndexKey(3);
			}
			if (this.HasStyleAttribute)
			{
				yield return this.AttributeIndexKey(34);
			}
			foreach (ushort token in this.IndexAttributesTokens())
			{
				yield return this.AttributeIndexKey(token);
			}
			yield break;
		}

		public override DomElement Clone()
		{
			DomElement domElement = DomElement.Create(this._NodeNameID);
			if (this.HasAttributes)
			{
				domElement._InnerAttributes = this.InnerAttributes.Clone();
			}
			if (this.HasClasses)
			{
				domElement._Classes = new List<ushort>(this._Classes);
			}
			if (this.HasStyleAttribute)
			{
				domElement.Style = this.Style.Clone();
			}
			ChildNodeList childNodeList = (ChildNodeList)domElement.ChildNodes;
			foreach (IDomObject item in this.CloneChildren())
			{
				childNodeList.AddAlways(item);
			}
			return domElement;
		}

		public override IEnumerable<IDomObject> CloneChildren()
		{
			if (base.ChildNodesInternal != null)
			{
				foreach (IDomObject obj in this.ChildNodes)
				{
					yield return obj.Clone();
				}
			}
			yield break;
		}

		public override bool HasStyle(string name)
		{
			return this.HasStyles && this.Style.HasStyle(name);
		}

		public override bool HasClass(string name)
		{
			return this.HasClasses && this._Classes.Contains(HtmlData.TokenizeCaseSensitive(name));
		}

		public override bool AddClass(string name)
		{
			bool flag = false;
			bool hasClasses = this.HasClasses;
			foreach (string text in name.SplitClean(CharacterData.charsHtmlSpaceArray))
			{
				if (!this.HasClass(text))
				{
					if (this._Classes == null)
					{
						this._Classes = new List<ushort>();
					}
					ushort num = HtmlData.TokenizeCaseSensitive(text);
					this._Classes.Add(num);
					if (this.IsIndexed)
					{
						this.Document.DocumentIndex.AddToIndex(this.ClassIndexKey(num), this);
					}
					flag = true;
				}
			}
			if (flag && !hasClasses && !this.IsDisconnected)
			{
				this.Document.DocumentIndex.AddToIndex(this.AttributeIndexKey(3), this);
			}
			return flag;
		}

		public override bool RemoveClass(string name)
		{
			bool result = false;
			bool hasClasses = this.HasClasses;
			foreach (string text in name.SplitClean())
			{
				if (this.HasClass(text))
				{
					ushort num = HtmlData.TokenizeCaseSensitive(text);
					this._Classes.Remove(num);
					if (!this.IsDisconnected)
					{
						this.Document.DocumentIndex.RemoveFromIndex(this.ClassIndexKey(num), this);
					}
					result = true;
				}
			}
			if (!this.HasClasses && hasClasses && !this.IsDisconnected)
			{
				this.Document.DocumentIndex.RemoveFromIndex(this.AttributeIndexKey(3), this);
			}
			return result;
		}

		public override bool HasAttribute(string name)
		{
			return this.HasAttribute(HtmlData.Tokenize(name));
		}

		public override void SetAttribute(string name, string value)
		{
			this.SetAttribute(HtmlData.Tokenize(name), value);
		}

		protected void SetAttribute(ushort tokenId, string value)
		{
			switch (tokenId)
			{
			case 3:
				this.ClassName = value;
				return;
			case 4:
				break;
			case 5:
				this.Id = value;
				goto IL_C6;
			default:
				if (tokenId == 34)
				{
					this.Style = new CSSStyleDeclaration(value, false);
					return;
				}
				break;
			}
			if (tokenId == 8 && this._NodeNameID == 9 && this.Type == "radio" && !string.IsNullOrEmpty(this.Name) && value != null && this.Document != null)
			{
				IList<IDomElement> list = this.Document.QuerySelectorAll("input[type='radio'][name='" + this.Name + "']:checked");
				foreach (IDomElement domElement in list)
				{
					domElement.Checked = false;
				}
			}
			IL_C6:
			this.SetAttributeRaw(tokenId, value);
		}

		public override void SetAttribute(string name)
		{
			this.SetAttribute(HtmlData.Tokenize(name));
		}

		public void SetAttribute(ushort tokenId)
		{
			if (tokenId == 3 || tokenId == 34)
			{
				throw new InvalidOperationException("You can't set class or style attributes as a boolean property.");
			}
			this.AttributeAddToIndex(tokenId);
			this.InnerAttributes.SetBoolean(tokenId);
		}

		protected void SetAttributeRaw(ushort tokenId, string value)
		{
			if (value == null)
			{
				this.InnerAttributes.Unset(tokenId);
				this.AttributeRemoveFromIndex(tokenId);
				return;
			}
			this.AttributeAddToIndex(tokenId);
			this.InnerAttributes[tokenId] = value;
		}

		public override bool RemoveAttribute(string name)
		{
			return this.RemoveAttribute(HtmlData.Tokenize(name));
		}

		public bool RemoveAttribute(ushort tokenId)
		{
			if (!this.HasAttributes)
			{
				return false;
			}
			switch (tokenId)
			{
			case 3:
				if (this.HasClasses)
				{
					this.SetClassName(null);
					return true;
				}
				return false;
			case 4:
				break;
			case 5:
				if (this.HasInnerAttributes && this.InnerAttributes.ContainsKey(5))
				{
					this.Id = null;
					return true;
				}
				return false;
			default:
				if (tokenId == 34)
				{
					if (this.HasStyleAttribute)
					{
						this.Style = null;
						return true;
					}
					return false;
				}
				break;
			}
			bool flag = this.InnerAttributes.Remove(tokenId);
			if (flag)
			{
				this.AttributeRemoveFromIndex(tokenId);
			}
			return flag;
		}

		public override string GetAttribute(string name)
		{
			return this.GetAttribute(name, null);
		}

		internal string GetAttribute(ushort tokenId)
		{
			return this.GetAttribute(tokenId, null);
		}

		public override string GetAttribute(string name, string defaultValue)
		{
			return this.GetAttribute(HtmlData.Tokenize(name), defaultValue);
		}

		internal string GetAttribute(ushort tokenId, string defaultValue)
		{
			string text = null;
			if (this.TryGetAttribute(tokenId, out text))
			{
				return text ?? "";
			}
			return defaultValue;
		}

		public bool TryGetAttribute(ushort tokenId, out string value)
		{
			if (tokenId == 3)
			{
				value = this.ClassName;
				return true;
			}
			if (tokenId == 34)
			{
				value = this.Style.ToString();
				return true;
			}
			if (this.HasInnerAttributes)
			{
				return this.InnerAttributes.TryGetValue(tokenId, out value);
			}
			value = null;
			return false;
		}

		public override bool TryGetAttribute(string name, out string value)
		{
			return this.TryGetAttribute(HtmlData.Tokenize(name), out value);
		}

		public override string ToString()
		{
			return this.ElementHtml();
		}

		public override void AddStyle(string style)
		{
			this.AddStyle(style, true);
		}

		public override void AddStyle(string style, bool strict)
		{
			this.Style.AddStyles(style, strict);
		}

		public override bool RemoveStyle(string name)
		{
			return this._Style != null && this._Style.RemoveStyle(name);
		}

		public void SetStyles(string styles)
		{
			this.SetStyles(styles, true);
		}

		public void SetStyles(string styles, bool strict)
		{
			this.Style.SetStyles(styles, strict);
		}

		bool isWhitespace(string what)
		{
			return what == "" || what == " " || what == Environment.NewLine;
		}

		IEnumerable<string> GetTextContent(IEnumerable<IDomObject> nodes)
		{
			List<string> list = new List<string>();
			Stack<IDomObject> stack = new Stack<IDomObject>(nodes.Reverse<IDomObject>());
			while (stack.Count > 0)
			{
				IDomObject domObject = stack.Pop();
				if (domObject.HasChildren)
				{
					using (IEnumerator<IDomObject> enumerator = domObject.ChildNodes.Reverse<IDomObject>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IDomObject item = enumerator.Current;
							stack.Push(item);
						}
						continue;
					}
				}
				this.AddOwnText_TextContent(list, domObject);
			}
			return list;
		}

		IEnumerable<string> GetInnerText(IEnumerable<IDomObject> nodes)
		{
			List<string> list = new List<string>();
			Stack<IDomObject> stack = new Stack<IDomObject>(nodes.Reverse<IDomObject>());
			while (stack.Count > 0)
			{
				IDomObject domObject = stack.Pop();
				if (HtmlData.IsBlock(domObject.NodeNameID) && list.Count > 0 && list[list.Count - 1] != Environment.NewLine)
				{
					if (this.isWhitespace(list[list.Count - 1]))
					{
						list[list.Count - 1] = Environment.NewLine;
					}
					else
					{
						list.Add(Environment.NewLine);
					}
				}
				if (domObject.HasChildren)
				{
					using (IEnumerator<IDomObject> enumerator = domObject.ChildNodes.Reverse<IDomObject>().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IDomObject item = enumerator.Current;
							stack.Push(item);
						}
						continue;
					}
				}
				this.AddOwnText_InnerText(list, domObject);
			}
			if (list.Count > 0 && this.isWhitespace(list[list.Count - 1]))
			{
				list[list.Count - 1] = Environment.NewLine;
			}
			return list;
		}

		void AddOwnText_TextContent(List<string> list, IDomObject obj)
		{
			NodeType nodeType = obj.NodeType;
			if (nodeType != NodeType.TEXT_NODE)
			{
				return;
			}
			list.Add(obj.NodeValue);
		}

		void AddOwnText_InnerText(List<string> list, IDomObject obj)
		{
			ushort nodeNameID = obj.ParentNode.NodeNameID;
			if (nodeNameID == 32 || nodeNameID == 34 || nodeNameID == 33)
			{
				return;
			}
			NodeType nodeType = obj.NodeType;
			if (nodeType != NodeType.TEXT_NODE)
			{
				return;
			}
			string text = obj.NodeValue.Trim();
			if (!this.isWhitespace(text))
			{
				list.Add(text);
				if (obj.NodeValue.TrimEnd(new char[0]) != obj.NodeValue)
				{
					list.Add(" ");
					return;
				}
			}
			else if (list.Count > 0 && !this.isWhitespace(list[list.Count - 1]))
			{
				list.Add(" ");
			}
		}

		void SetNodeName(string nodeName)
		{
			if (this._NodeNameID < 1)
			{
				this._NodeNameID = HtmlData.Tokenize(nodeName);
				return;
			}
			throw new InvalidOperationException("You can't change the tag of an element once it has been created.");
		}

		ushort[] ClassIndexKey(ushort classID)
		{
			return new ushort[] { 46, classID };
		}

		ushort[] IDIndexKey(string id)
		{
			return new ushort[]
			{
				35,
				HtmlData.TokenizeCaseSensitive(id)
			};
		}

		protected ushort[] AttributeIndexKey(string attrName)
		{
			return this.AttributeIndexKey(HtmlData.Tokenize(attrName));
		}

		protected ushort[] AttributeIndexKey(ushort attrId)
		{
			return new ushort[] { 33, attrId };
		}

		protected void AttributeRemoveFromIndex(ushort attrId)
		{
			if (!this.IsDisconnected)
			{
				this.Document.DocumentIndex.RemoveFromIndex(this.AttributeIndexKey(attrId), this);
			}
		}

		protected void AttributeAddToIndex(ushort attrId)
		{
			if (!this.IsDisconnected && !this.InnerAttributes.ContainsKey(attrId))
			{
				this.Document.DocumentIndex.AddToIndex(this.AttributeIndexKey(attrId), this);
			}
		}

		protected void SetClassName(string className)
		{
			if (this.HasClasses)
			{
				foreach (string className2 in this.Classes.ToList<string>())
				{
					this.RemoveClass(className2);
				}
			}
			if (!string.IsNullOrEmpty(className))
			{
				this.AddClass(className);
			}
		}

		protected bool hasDefaultValue()
		{
			return this.NodeNameID == 9 || this.NodeNameID == 33;
		}

		internal IEnumerable<IDomElement> DescendantElements()
		{
			foreach (IDomElement el in this.ChildElements)
			{
				yield return el;
				if (el.HasChildren)
				{
					foreach (IDomElement child in ((DomElement)el).DescendantElements())
					{
						yield return child;
					}
				}
			}
			yield break;
		}

		internal bool HasAttribute(ushort tokenId)
		{
			if (tokenId == 3)
			{
				return this.HasClasses;
			}
			if (tokenId != 34)
			{
				return this._InnerAttributes != null && this.InnerAttributes.ContainsKey(tokenId);
			}
			return this.HasStyleAttribute;
		}

		internal virtual bool TryGetAttributeForMatching(ushort attributeId, out string value)
		{
			return this.TryGetAttribute(attributeId, out value);
		}

		internal IDomElement Closest(ushort tagID)
		{
			for (IDomContainer parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
			{
				if (parentNode.NodeNameID == tagID)
				{
					return (IDomElement)parentNode;
				}
			}
			return null;
		}

		internal void SetProp(ushort tagId, bool value)
		{
			if (value)
			{
				this.SetAttribute(tagId);
				return;
			}
			this.RemoveAttribute(tagId);
		}

		internal void SetProp(string tagName, bool value)
		{
			this.SetProp(HtmlData.Tokenize(tagName), value);
		}

		internal IEnumerable<T> ChildElementsOfTag<T>(ushort nodeNameId)
		{
			return this.ChildElementsOfTag<T>(this, nodeNameId);
		}

		IEnumerable<T> ChildElementsOfTag<T>(IDomElement parent, ushort nodeNameId)
		{
			foreach (IDomObject el in this.ChildNodes)
			{
				if (el.NodeType == NodeType.ELEMENT_NODE && el.NodeNameID == nodeNameId)
				{
					yield return (T)((object)el);
				}
				if (el.HasChildren)
				{
					foreach (T child in ((DomElement)el).ChildElementsOfTag<T>(nodeNameId))
					{
						yield return child;
					}
				}
			}
			yield break;
		}

		string IAttributeCollection.this[string attributeName]
		{
			get
			{
				return this.GetAttribute(attributeName);
			}
			set
			{
				this.SetAttribute(attributeName, value);
			}
		}

		int IAttributeCollection.Length
		{
			get
			{
				int num = (this.HasClasses ? 1 : 0) + (this.HasStyleAttribute ? 1 : 0);
				return num + ((!this.HasInnerAttributes) ? 0 : this.InnerAttributes.Count);
			}
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			return this.AttributesCollection().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.AttributesCollection().GetEnumerator();
		}

		protected IEnumerable<KeyValuePair<string, string>> AttributesCollection()
		{
			if (this.HasClasses)
			{
				yield return new KeyValuePair<string, string>("class", this.ClassName);
			}
			if (this.HasStyleAttribute)
			{
				yield return new KeyValuePair<string, string>("style", this.Style.ToString());
			}
			if (this.HasInnerAttributes)
			{
				foreach (KeyValuePair<string, string> kvp in this.InnerAttributes)
				{
					yield return kvp;
				}
			}
			yield break;
		}

		protected virtual IEnumerable<ushort> IndexAttributesTokens()
		{
			if (this.HasInnerAttributes)
			{
				foreach (ushort token in this.InnerAttributes.GetAttributeIds())
				{
					yield return token;
				}
			}
			yield break;
		}

		AttributeCollection _InnerAttributes;

		CSSStyleDeclaration _Style;

		List<ushort> _Classes;

		ushort _NodeNameID;
	}
}
