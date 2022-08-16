using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CsQuery.Implementation
{
	class ChildNodeList : INodeList, IList<IDomObject>, ICollection<IDomObject>, IEnumerable<IDomObject>, IEnumerable
	{
		public ChildNodeList(IDomContainer owner)
		{
			this.Owner = owner;
		}

		protected List<IDomObject> InnerList
		{
			get
			{
				if (this._InnerList == null)
				{
					this._InnerList = new List<IDomObject>();
				}
				return this._InnerList;
			}
		}

		public event EventHandler<NodeEventArgs> OnChanged;

		public IDomContainer Owner { get; set; }

		public IDomObject Item(int index)
		{
			return this[index];
		}

		public int IndexOf(IDomObject item)
		{
			if (this._InnerList != null)
			{
				return this.InnerList.IndexOf(item);
			}
			return -1;
		}

		public void Add(IDomObject item)
		{
			if (item.ParentNode != null)
			{
				item.Remove();
			}
			if (!string.IsNullOrEmpty(item.Id) && !this.Owner.IsFragment && this.Owner.Document.GetElementById(item.Id) != null)
			{
				item.Id = null;
			}
			this.InnerList.Add(item);
			this.AddParent(item, this.InnerList.Count - 1);
			this.RaiseChangedEvent(item);
		}

		public void AddAlways(IDomObject item)
		{
			this.InnerList.Add(item);
			this.AddParent(item, this.InnerList.Count - 1);
		}

		public void Insert(int index, IDomObject item)
		{
			if (item.ParentNode != null)
			{
				item.Remove();
			}
			this.ReindexFromRight(index);
			if (index == this.InnerList.Count)
			{
				this.InnerList.Add(item);
			}
			else
			{
				this.InnerList.Insert(index, item);
			}
			this.AddParent(item, index);
			this.RaiseChangedEvent(item);
		}

		public void RemoveAt(int index)
		{
			IDomObject domObject = this.InnerList[index];
			this.InnerList.RemoveAt(index);
			this.RemoveParent(domObject);
			this.ReindexFromLeft(index);
			this.RaiseChangedEvent(domObject);
		}

		public bool Remove(IDomObject item)
		{
			if (item.ParentNode != this.Owner)
			{
				return false;
			}
			this.RemoveAt(item.Index);
			return true;
		}

		[IndexerName("Indexer")]
		public IDomObject this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				this.RemoveAt(index);
				if (index < this.InnerList.Count)
				{
					this.Insert(index, value);
					return;
				}
				this.Add(value);
			}
		}

		void RaiseChangedEvent(IDomObject node)
		{
			EventHandler<NodeEventArgs> onChanged = this.OnChanged;
			if (onChanged != null)
			{
				onChanged(this, new NodeEventArgs(node));
			}
		}

		void RemoveParent(IDomObject element)
		{
			if (element.ParentNode != null)
			{
				DomObject domObject = element as DomObject;
				if (!element.IsDisconnected && element.IsIndexed)
				{
					domObject.Document.DocumentIndex.RemoveFromIndex((IDomIndexedNode)element);
				}
				domObject.ParentNode = null;
			}
		}

		void AddParent(IDomObject element, int index)
		{
			DomObject domObject = element as DomObject;
			domObject.ParentNode = this.Owner;
			domObject.Index = index;
			if (element.IsIndexed)
			{
				domObject.Document.DocumentIndex.AddToIndex((IDomIndexedNode)element);
			}
		}

		void ReindexFromLeft(int index)
		{
			if (index < this.InnerList.Count)
			{
				bool isDisconnected = this.Owner.IsDisconnected;
				int count = this.InnerList.Count;
				for (int i = index; i < count; i++)
				{
					if (!isDisconnected && this.InnerList[i].IsIndexed)
					{
						DomElement domElement = (DomElement)this.InnerList[i];
						this.Owner.Document.DocumentIndex.RemoveFromIndex(domElement);
						domElement.Index = i;
						this.Owner.Document.DocumentIndex.AddToIndex(domElement);
					}
					else
					{
						((DomObject)this.InnerList[i]).Index = i;
					}
				}
			}
		}

		void ReindexFromRight(int index)
		{
			if (index < this.InnerList.Count)
			{
				bool isDisconnected = this.Owner.IsDisconnected;
				int num = index - 1;
				for (int i = this.InnerList.Count - 1; i > num; i--)
				{
					if (!isDisconnected && this.InnerList[i].IsIndexed)
					{
						DomElement domElement = (DomElement)this.InnerList[i];
						this.Owner.Document.DocumentIndex.RemoveFromIndex(domElement);
						domElement.Index = i + 1;
						this.Owner.Document.DocumentIndex.AddToIndex(domElement);
					}
					else
					{
						((DomObject)this.InnerList[i]).Index = i + 1;
					}
				}
			}
		}

		public void AddRange(IEnumerable<IDomObject> elements)
		{
			List<IDomObject> list = new List<IDomObject>(elements);
			foreach (IDomObject item in list)
			{
				this.Add(item);
			}
		}

		public void Clear()
		{
			if (this._InnerList != null)
			{
				for (int i = this.InnerList.Count - 1; i >= 0; i--)
				{
					this.Remove(this.InnerList[i]);
				}
			}
		}

		public bool Contains(IDomObject item)
		{
			return this._InnerList != null && this.InnerList.Contains(item);
		}

		public void CopyTo(IDomObject[] array, int arrayIndex)
		{
			this.InnerList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				if (this._InnerList != null)
				{
					return this.InnerList.Count;
				}
				return 0;
			}
		}

		public int Length
		{
			get
			{
				return this.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator<IDomObject> IEnumerable<IDomObject>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IEnumerator<IDomObject> GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		IDomObject IList<IDomObject>.get_Item(int A_1)
		{
			return this[A_1];
		}

		void IList<IDomObject>.set_Item(int A_1, IDomObject A_2)
		{
			this[A_1] = A_2;
		}

		List<IDomObject> _InnerList;
	}
}
