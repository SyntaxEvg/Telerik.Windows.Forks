using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	abstract class DomContainer<T> : DomObject<T>, IDomContainer, IDomObject, IDomNode, ICloneable, IComparable<IDomObject> where T : IDomObject, IDomContainer, new()
	{
		public DomContainer()
		{
		}

		public DomContainer(IEnumerable<IDomObject> elements)
		{
			this.ChildNodesInternal.AddRange(elements);
		}

		public override INodeList ChildNodes
		{
			get
			{
				return this.ChildNodesInternal;
			}
		}

		protected ChildNodeList ChildNodesInternal
		{
			get
			{
				if (this._ChildNodes == null)
				{
					this._ChildNodes = new ChildNodeList(this);
				}
				return this._ChildNodes;
			}
		}

		public override bool HasChildren
		{
			get
			{
				return this.ChildNodesInternal != null && this.ChildNodes.Count > 0;
			}
		}

		public override IDomObject FirstChild
		{
			get
			{
				if (this.HasChildren)
				{
					return this.ChildNodes[0];
				}
				return null;
			}
		}

		public override IDomElement FirstElementChild
		{
			get
			{
				if (this.HasChildren)
				{
					int num = 0;
					while (num < this.ChildNodes.Count && this.ChildNodes[num].NodeType != NodeType.ELEMENT_NODE)
					{
						num++;
					}
					if (num < this.ChildNodes.Count)
					{
						return (IDomElement)this.ChildNodes[num];
					}
				}
				return null;
			}
		}

		public override IDomObject LastChild
		{
			get
			{
				if (!this.HasChildren)
				{
					return null;
				}
				return this.ChildNodes[this.ChildNodes.Count - 1];
			}
		}

		public override IDomElement LastElementChild
		{
			get
			{
				if (this.HasChildren)
				{
					int num = this.ChildNodes.Count - 1;
					while (num >= 0 && this.ChildNodes[num].NodeType != NodeType.ELEMENT_NODE)
					{
						num--;
					}
					if (num >= 0)
					{
						return (IDomElement)this.ChildNodes[num];
					}
				}
				return null;
			}
		}

		public override void AppendChild(IDomObject item)
		{
			this.ChildNodes.Add(item);
		}

		internal override void AppendChildUnsafe(IDomObject item)
		{
			this.ChildNodesInternal.AddAlways(item);
		}

		public override void RemoveChild(IDomObject item)
		{
			this.ChildNodes.Remove(item);
		}

		public override void InsertBefore(IDomObject newNode, IDomObject referenceNode)
		{
			if (referenceNode.ParentNode != this)
			{
				throw new InvalidOperationException("The reference node is not a child of this node");
			}
			this.ChildNodes.Insert(referenceNode.Index, newNode);
		}

		public override void InsertAfter(IDomObject newNode, IDomObject referenceNode)
		{
			if (referenceNode.ParentNode != this)
			{
				throw new InvalidOperationException("The reference node is not a child of this node");
			}
			this.ChildNodes.Insert(referenceNode.Index + 1, newNode);
		}

		public override IEnumerable<IDomElement> ChildElements
		{
			get
			{
				if (this.HasChildren)
				{
					foreach (IDomObject obj in this.ChildNodes)
					{
						IDomElement elm = obj as IDomElement;
						if (elm != null)
						{
							yield return elm;
						}
					}
				}
				yield break;
			}
		}

		public override int DescendantCount()
		{
			int num = 0;
			if (this.HasChildren)
			{
				foreach (IDomObject domObject in this.ChildNodes)
				{
					num += 1 + domObject.DescendantCount();
				}
			}
			return num;
		}

		public override IDomObject this[int index]
		{
			get
			{
				return this.ChildNodes[index];
			}
		}

		IDomObject IDomObject.Clone()
		{
			return this.Clone();
		}

		IDomNode IDomNode.Clone()
		{
			return this.Clone();
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		ChildNodeList _ChildNodes;
	}
}
