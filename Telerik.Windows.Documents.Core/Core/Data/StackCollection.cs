using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Data
{
	public class StackCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : IStackCollectionElement
	{
		public StackCollection()
		{
			this.nameToElement = new Dictionary<string, T>();
			this.elements = new LinkedList<T>();
		}

		public int Count
		{
			get
			{
				return this.elements.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<T>)this.elements).IsReadOnly;
			}
		}

		public T GetElementByName(string elementName)
		{
			T result;
			if (this.nameToElement.TryGetValue(elementName, out result))
			{
				return result;
			}
			return default(T);
		}

		public void Add(T item)
		{
			this.AddToDictionary(item);
			this.elements.AddLast(item);
		}

		public bool AddBefore(string presentedElementName, T layer)
		{
			LinkedListNode<T> nodeByName = this.GetNodeByName(presentedElementName);
			if (nodeByName != null)
			{
				this.AddToDictionary(layer);
				this.elements.AddBefore(nodeByName, layer);
				return true;
			}
			return false;
		}

		public bool AddAfter(string presentedElementName, T element)
		{
			LinkedListNode<T> nodeByName = this.GetNodeByName(presentedElementName);
			if (nodeByName != null)
			{
				this.AddToDictionary(element);
				this.elements.AddAfter(nodeByName, element);
				return true;
			}
			return false;
		}

		public void AddFirst(T element)
		{
			this.AddToDictionary(element);
			this.elements.AddFirst(element);
		}

		public void AddLast(T element)
		{
			this.AddToDictionary(element);
			this.elements.AddLast(element);
		}

		public bool Contains(T item)
		{
			return this.Contains(item.Name);
		}

		public bool Contains(string elementName)
		{
			return this.nameToElement.ContainsKey(elementName);
		}

		public bool Remove(T item)
		{
			this.RemoveFromDictionary(item);
			return this.elements.Remove(item);
		}

		public bool Remove(string elementName)
		{
			T item;
			return this.nameToElement.TryGetValue(elementName, out item) && this.Remove(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.elements.CopyTo(array, arrayIndex);
		}

		public void Clear()
		{
			this.nameToElement.Clear();
			this.elements.Clear();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.elements).GetEnumerator();
		}

		void AddToDictionary(T element)
		{
			if (this.nameToElement.ContainsKey(element.Name))
			{
				throw new InvalidOperationException("Element with name " + element.Name + " already exists.");
			}
			this.nameToElement.Add(element.Name, element);
		}

		void RemoveFromDictionary(T element)
		{
			this.nameToElement.Remove(element.Name);
		}

		LinkedListNode<T> GetNodeByName(string elementName)
		{
			LinkedListNode<T> linkedListNode;
			for (linkedListNode = this.elements.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				T value = linkedListNode.Value;
				if (!(value.Name != elementName))
				{
					break;
				}
			}
			return linkedListNode;
		}

		readonly Dictionary<string, T> nameToElement;

		readonly LinkedList<T> elements;
	}
}
