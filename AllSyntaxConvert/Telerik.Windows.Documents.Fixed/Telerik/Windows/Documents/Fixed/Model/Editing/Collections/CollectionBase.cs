using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Collections
{
	public abstract class CollectionBase<T> : IEnumerable<T>, IEnumerable
	{
		internal CollectionBase()
		{
			this.store = new List<T>();
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public T this[int index]
		{
			get
			{
				return this.store[index];
			}
			internal set
			{
				Guard.ThrowExceptionIfNull<T>(value, "value");
				this.store[index] = value;
			}
		}

		public void Add(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.OnAddingElement(item);
			this.store.Add(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal virtual void OnAddingElement(T element)
		{
		}

		internal void AddRange(IEnumerable<T> range)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<T>>(range, "range");
			this.store.AddRange(range);
		}

		internal void RemoveRange(int index, int count)
		{
			this.store.RemoveRange(index, count);
		}

		readonly List<T> store;
	}
}
