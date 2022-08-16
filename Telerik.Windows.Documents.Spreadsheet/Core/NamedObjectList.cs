using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public class NamedObjectList<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : INamedObject
	{
		public int Count
		{
			get
			{
				return this.itemList.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<T>)this.itemList).IsReadOnly;
			}
		}

		public NamedObjectList()
		{
			this.nameToItem = new Dictionary<string, T>();
			this.itemList = new LinkedList<T>();
		}

		void AddToDictionary(T item)
		{
			if (this.nameToItem.ContainsKey(item.Name))
			{
				throw new LocalizableException(string.Format("Item with name {0} already exists.", item.Name), new InvalidOperationException(string.Format("Item with name {0} already exists.", item.Name)), "Spreadsheet_ErrorExpressions_NamedObjectExistsError", new string[] { item.Name.ToString() });
			}
			this.nameToItem.Add(item.Name, item);
		}

		void RemoveFromDictionary(T item)
		{
			this.nameToItem.Remove(item.Name);
		}

		LinkedListNode<T> GetNodeByName(string itemName)
		{
			LinkedListNode<T> linkedListNode;
			for (linkedListNode = this.itemList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				T value = linkedListNode.Value;
				if (!(value.Name != itemName))
				{
					break;
				}
			}
			return linkedListNode;
		}

		public T GetByName(string itemName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(itemName, "itemName");
			T result = default(T);
			if (this.nameToItem.TryGetValue(itemName, out result))
			{
				return result;
			}
			return result;
		}

		void ICollection<T>.Add(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.AddToDictionary(item);
			this.itemList.AddLast(item);
		}

		public bool AddBefore(string nextItemName, T item)
		{
			Guard.ThrowExceptionIfNullOrEmpty(nextItemName, "nextItemName");
			Guard.ThrowExceptionIfNull<T>(item, "item");
			LinkedListNode<T> nodeByName = this.GetNodeByName(nextItemName);
			if (nodeByName != null)
			{
				this.AddToDictionary(item);
				this.itemList.AddBefore(nodeByName, item);
				return true;
			}
			return false;
		}

		public bool AddAfter(string previousItemName, T item)
		{
			Guard.ThrowExceptionIfNullOrEmpty(previousItemName, "previousItemName");
			Guard.ThrowExceptionIfNull<T>(item, "item");
			LinkedListNode<T> nodeByName = this.GetNodeByName(previousItemName);
			if (nodeByName != null)
			{
				this.AddToDictionary(item);
				this.itemList.AddAfter(nodeByName, item);
				return true;
			}
			return false;
		}

		public void AddFirst(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.AddToDictionary(item);
			this.itemList.AddFirst(item);
		}

		public void AddLast(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.AddToDictionary(item);
			this.itemList.AddLast(item);
		}

		public bool Contains(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			return this.Contains(item.Name);
		}

		public bool Contains(string itemName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(itemName, "itemName");
			return this.nameToItem.ContainsKey(itemName);
		}

		protected virtual void RemoveOverride(T item)
		{
		}

		public bool Remove(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.RemoveOverride(item);
			this.RemoveFromDictionary(item);
			return this.itemList.Remove(item);
		}

		public bool Remove(string itemName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(itemName, "itemName");
			T item;
			return this.nameToItem.TryGetValue(itemName, out item) && this.Remove(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.itemList.CopyTo(array, arrayIndex);
		}

		public void Clear()
		{
			while (this.itemList.Count > 0)
			{
				T value = this.itemList.First.Value;
				this.Remove(value.Name);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.itemList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.itemList).GetEnumerator();
		}

		readonly Dictionary<string, T> nameToItem;

		readonly LinkedList<T> itemList;
	}
}
