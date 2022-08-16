using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsQuery.Output;

namespace CsQuery.Implementation
{
	abstract class DomObject : IDomObject, IComparable<IDomObject>, IDomNode, ICloneable
	{
		protected abstract IDomObject CloneImplementation();

		public abstract NodeType NodeType { get; }

		public abstract bool HasChildren { get; }

		public abstract bool InnerHtmlAllowed { get; }

		public virtual ushort NodeNameID
		{
			get
			{
				return 0;
			}
		}

		public virtual bool InnerTextAllowed
		{
			get
			{
				return this.InnerHtmlAllowed;
			}
		}

		public virtual bool ChildrenAllowed
		{
			get
			{
				return this.InnerHtmlAllowed;
			}
		}

		public virtual bool IsIndexed
		{
			get
			{
				return false;
			}
		}

		public virtual ushort[] NodePath
		{
			get
			{
				if (this._ParentNode == null)
				{
					return new ushort[]
					{
						95,
						(ushort)this.Index
					};
				}
				return this.GetPath();
			}
		}

		[Obsolete]
		public virtual string Path
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ushort value in this.NodePath)
				{
					stringBuilder.Append(value);
				}
				return stringBuilder.ToString();
			}
		}

		protected virtual ushort[] GetPath_UnOptimized()
		{
			DomObject domObject = this;
			List<ushort> list = new List<ushort>();
			while (domObject != null)
			{
				list.Add((ushort)domObject.Index);
				domObject = domObject._ParentNode;
			}
			list.Reverse();
			return list.ToArray();
		}

		protected virtual ushort[] GetPath()
		{
			DomObject domObject = this;
			ushort num = 0;
			ushort[] array = new ushort[32];
			int num2 = 32;
			while (domObject != null)
			{
				ushort[] array2 = array;
				ushort num3 = num;
				num = (ushort)(num3 + 1);
				array2[(int)num3] = (ushort)domObject.Index;
				if ((int)num == num2)
				{
					num2 <<= 1;
					ushort[] array3 = new ushort[num2];
					Buffer.BlockCopy(array, 0, array3, 0, (int)num << 1);
					array = array3;
				}
				domObject = domObject._ParentNode;
			}
			ushort[] array4 = new ushort[(int)num];
			int num4 = 0;
			while (num > 0)
			{
				array4[num4++] = array[(int)(num -= 1)];
			}
			return array4;
		}

		public virtual IDomDocument Document
		{
			get
			{
				if ((byte)(this.DocInfo & DomObject.DocumentInfo.IsParentTested) == 0)
				{
					this.UpdateDocumentFlags();
				}
				return this._Document;
			}
		}

		public virtual string InnerText
		{
			get
			{
				throw new InvalidOperationException("Accessing InnerText is not valid for this element type.");
			}
			set
			{
				throw new InvalidOperationException("Assigning InnerText is not valid for this element type.");
			}
		}

		public virtual string TextContent
		{
			get
			{
				throw new InvalidOperationException("Accessing TextContent is not valid for this element type.");
			}
			set
			{
				throw new InvalidOperationException("Assigning TextContent is not valid for this element type.");
			}
		}

		public virtual string InnerHTML
		{
			get
			{
				throw new InvalidOperationException("Accessing InnerHTML is not valid for this element type.");
			}
			set
			{
				throw new InvalidOperationException("Assigning InnerHTML is not valid for this element type.");
			}
		}

		public virtual string OuterHTML
		{
			get
			{
				throw new InvalidOperationException("Accessing OuterHTML is not valid for this element type.");
			}
			set
			{
				throw new InvalidOperationException("Assigning OuterHTML is not valid for this element type.");
			}
		}

		public virtual INodeList ChildNodes
		{
			get
			{
				return null;
			}
		}

		public virtual IDomContainer ParentNode
		{
			get
			{
				return (IDomContainer)this._ParentNode;
			}
			internal set
			{
				this._ParentNode = (DomObject)value;
				this.UpdateDocumentFlags();
			}
		}

		public virtual IEnumerable<IDomContainer> GetAncestors()
		{
			for (IDomContainer current = this.ParentNode; current != null; current = current.ParentNode)
			{
				yield return current;
			}
			yield break;
		}

		public virtual IEnumerable<IDomObject> GetDescendents()
		{
			if (this.HasChildren)
			{
				foreach (IDomObject child in this.ChildNodes)
				{
					yield return child;
					foreach (IDomObject descendent in child.GetDescendents())
					{
						yield return descendent;
					}
				}
			}
			yield break;
		}

		public virtual IEnumerable<IDomElement> GetDescendentElements()
		{
			return this.GetDescendents().OfType<IDomElement>();
		}

		public virtual bool IsFragment
		{
			get
			{
				return (byte)(this.DocInfo & DomObject.DocumentInfo.IsDocument) == 0;
			}
		}

		public virtual bool IsDisconnected
		{
			get
			{
				return (byte)(this.DocInfo & DomObject.DocumentInfo.IsConnected) == 0;
			}
		}

		[Obsolete]
		public virtual char PathID
		{
			get
			{
				return (char)(this.Index + 2);
			}
		}

		public virtual int Depth
		{
			get
			{
				return this.GetDepth();
			}
		}

		protected int GetDepth()
		{
			DomObject parentNode = this._ParentNode;
			int num = 0;
			while (parentNode != null && parentNode.NodeType != NodeType.DOCUMENT_NODE)
			{
				num++;
				parentNode = parentNode._ParentNode;
			}
			return num;
		}

		public virtual IEnumerable<IDomElement> ChildElements
		{
			get
			{
				yield break;
			}
		}

		public int Index { get; internal set; }

		public ushort NodePathID
		{
			get
			{
				return (ushort)this.Index;
			}
		}

		public virtual string DefaultValue
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("DefaultValue is not valid for this node type");
			}
		}

		public virtual string NodeValue
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("You can't set NodeValue for this node type.");
			}
		}

		public virtual string Type
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("You can't set Type for this node type.");
			}
		}

		public virtual string Name
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("You can't set Name for this node type.");
			}
		}

		public virtual string Id
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("Cannot set ID for this node type.");
			}
		}

		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("Cannot set value for this node type.");
			}
		}

		public virtual string ClassName
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("ClassName is not applicable to this node type.");
			}
		}

		public virtual IEnumerable<string> Classes
		{
			get
			{
				yield break;
			}
		}

		public virtual IAttributeCollection Attributes
		{
			get
			{
				return null;
			}
			protected set
			{
				throw new InvalidOperationException("Attributes collection is not applicable to this node type.");
			}
		}

		public virtual CSSStyleDeclaration Style
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException("Style is not applicable to this node type.");
			}
		}

		public virtual string NodeName
		{
			get
			{
				return null;
			}
		}

		public virtual IDomObject FirstChild
		{
			get
			{
				return null;
			}
		}

		public virtual IDomObject LastChild
		{
			get
			{
				return null;
			}
		}

		public virtual IDomElement FirstElementChild
		{
			get
			{
				return null;
			}
		}

		public virtual IDomElement LastElementChild
		{
			get
			{
				return null;
			}
		}

		public virtual bool HasAttributes
		{
			get
			{
				return false;
			}
		}

		public virtual bool HasClasses
		{
			get
			{
				return false;
			}
		}

		public virtual bool HasStyles
		{
			get
			{
				return false;
			}
		}

		public virtual bool Checked
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException("Not valid for this element type.");
			}
		}

		public virtual bool Disabled
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException("Not valid for this element type.");
			}
		}

		public virtual bool ReadOnly
		{
			get
			{
				return true;
			}
			set
			{
				throw new InvalidOperationException("Not valid for this element type.");
			}
		}

		public IDomObject NextSibling
		{
			get
			{
				if (this.ParentNode == null || this.ParentNode.ChildNodes.Count - 1 <= this.Index)
				{
					return null;
				}
				return this.ParentNode.ChildNodes[this.Index + 1];
			}
		}

		public IDomObject PreviousSibling
		{
			get
			{
				if (this.ParentNode == null || this.Index <= 0)
				{
					return null;
				}
				return this.ParentNode.ChildNodes[this.Index - 1];
			}
		}

		public IDomElement NextElementSibling
		{
			get
			{
				if (this.ParentNode == null)
				{
					return null;
				}
				int num = this.Index;
				INodeList childNodes = this.ParentNode.ChildNodes;
				while (++num < childNodes.Count)
				{
					if (childNodes[num].NodeType == NodeType.ELEMENT_NODE)
					{
						return (IDomElement)childNodes[num];
					}
				}
				return null;
			}
		}

		public IDomElement PreviousElementSibling
		{
			get
			{
				if (this.ParentNode == null)
				{
					return null;
				}
				int num = this.Index;
				INodeList childNodes = this.ParentNode.ChildNodes;
				while (--num >= 0)
				{
					if (childNodes[num].NodeType == NodeType.ELEMENT_NODE)
					{
						return (IDomElement)childNodes[num];
					}
				}
				return null;
			}
		}

		public virtual IDomObject this[int index]
		{
			get
			{
				throw new InvalidOperationException("This element type does not have children");
			}
		}

		public virtual string this[string attribute]
		{
			get
			{
				throw new InvalidOperationException("This element type does not have attributes");
			}
			set
			{
				throw new InvalidOperationException("This element type does not have attributes");
			}
		}

		public virtual string Render()
		{
			return this.Render(OutputFormatters.Default);
		}

		public virtual void Render(IOutputFormatter formatter, TextWriter writer)
		{
			formatter.Render(this, writer);
		}

		public virtual string Render(IOutputFormatter formatter)
		{
			return formatter.Render(this);
		}

		public virtual string Render(DomRenderingOptions options)
		{
			FormatDefault formatDefault = new FormatDefault(options, HtmlEncoders.Default);
			return formatDefault.Render(this);
		}

		[Obsolete]
		public virtual void Render(StringBuilder sb)
		{
			this.Render(sb, DomRenderingOptions.Default);
		}

		[Obsolete]
		public virtual void Render(StringBuilder sb, DomRenderingOptions options)
		{
			sb.Append(this.Render(options));
		}

		public CQ Cq()
		{
			return new CQ(this);
		}

		public virtual IDomObject Clone()
		{
			return this.CloneImplementation();
		}

		public virtual void Remove()
		{
			if (this.ParentNode == null)
			{
				throw new InvalidOperationException("This element has no parent.");
			}
			this.ParentNode.ChildNodes.Remove(this);
		}

		public virtual int DescendantCount()
		{
			return 0;
		}

		public virtual void AppendChild(IDomObject element)
		{
			throw new InvalidOperationException("This type of element does not have children.");
		}

		internal virtual void AppendChildUnsafe(IDomObject element)
		{
			throw new InvalidOperationException("This type of element does not have children.");
		}

		public virtual void RemoveChild(IDomObject element)
		{
			throw new InvalidOperationException("This type of element does not have children.");
		}

		public virtual void InsertBefore(IDomObject newNode, IDomObject referenceNode)
		{
			throw new InvalidOperationException("This type of element does not have children.");
		}

		public virtual void InsertAfter(IDomObject newNode, IDomObject referenceNode)
		{
			throw new InvalidOperationException("This type of element does not have children.");
		}

		public virtual void SetAttribute(string name)
		{
			throw new InvalidOperationException("You can't set attributes for this element type.");
		}

		public virtual void SetAttribute(string name, string value)
		{
			throw new InvalidOperationException("You can't set attributes for this element type.");
		}

		public virtual string GetAttribute(string name)
		{
			return null;
		}

		public virtual string GetAttribute(string name, string defaultValue)
		{
			return null;
		}

		public virtual bool TryGetAttribute(string name, out string value)
		{
			value = null;
			return false;
		}

		public virtual bool HasAttribute(string name)
		{
			return false;
		}

		public virtual bool RemoveAttribute(string name)
		{
			throw new InvalidOperationException("You can't remove attributes from this element type.");
		}

		public virtual bool HasClass(string className)
		{
			return false;
		}

		public virtual bool AddClass(string className)
		{
			throw new InvalidOperationException("You can't add classes to this element type.");
		}

		public virtual bool RemoveClass(string className)
		{
			throw new InvalidOperationException("You can't remove classes from this element type.");
		}

		public virtual bool HasStyle(string styleName)
		{
			return false;
		}

		public virtual void AddStyle(string styleString)
		{
			throw new InvalidOperationException("You can't add styles to this element type.");
		}

		public virtual void AddStyle(string styleString, bool strict)
		{
			throw new InvalidOperationException("You can't add styles to this element type.");
		}

		public virtual bool RemoveStyle(string name)
		{
			throw new InvalidOperationException("You can't remove styles to this element type.");
		}

		public override string ToString()
		{
			return this.Render();
		}

		public virtual int ElementIndex
		{
			get
			{
				throw new InvalidOperationException("This is not an Element object.");
			}
		}

		public virtual IEnumerable<IDomObject> CloneChildren()
		{
			throw new InvalidOperationException("This is not a Container object.");
		}

		public virtual string ElementHtml()
		{
			throw new InvalidOperationException("This is not an Element object.");
		}

		public virtual bool IsBlock
		{
			get
			{
				throw new InvalidOperationException("This is not an Element object.");
			}
		}

		public virtual IEnumerable<ushort[]> IndexKeysRanged()
		{
			throw new InvalidOperationException("This is not an indexed object.");
		}

		public virtual IEnumerable<ushort[]> IndexKeys()
		{
			throw new InvalidOperationException("This is not an indexed object.");
		}

		public virtual IDomObject IndexReference
		{
			get
			{
				throw new InvalidOperationException("This is not an indexed object.");
			}
		}

		public virtual bool Selected
		{
			get
			{
				throw new Exception("The Selected property cannot does not apply to this element type.");
			}
			set
			{
				throw new Exception("The Selected property cannot be set for this element type.");
			}
		}

		protected void UpdateDocumentFlags()
		{
			this.UpdateDocumentFlags((this.ParentNode == null) ? null : this.ParentNode.Document);
		}

		protected void UpdateDocumentFlags(IDomDocument document)
		{
			this._Document = document;
			this.SetDocFlags();
			if (this.HasChildren && this._Document != null)
			{
				foreach (DomObject domObject in this.ChildNodes.Cast<DomObject>())
				{
					domObject.UpdateDocumentFlags(this._Document);
				}
			}
		}

		void SetDocFlags()
		{
			this.DocInfo = (DomObject.DocumentInfo)(8 | ((this._Document == null) ? 0 : ((byte)(4 | ((this._Document.NodeType == NodeType.DOCUMENT_NODE) ? 2 : 0)) | (this._Document.IsIndexed ? 1 : 0))));
		}

		IDomNode IDomNode.Clone()
		{
			return this.Clone();
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public int CompareTo(IDomObject other)
		{
			return PathKeyComparer.Comparer.Compare(this.NodePath, other.NodePath);
		}

		IDomDocument _Document;

		DomObject _ParentNode;

		protected DomObject.DocumentInfo DocInfo;

		[Flags]
		protected enum DocumentInfo : byte
		{
			IsIndexed = 1,
			IsDocument = 2,
			IsConnected = 4,
			IsParentTested = 8
		}
	}
}
