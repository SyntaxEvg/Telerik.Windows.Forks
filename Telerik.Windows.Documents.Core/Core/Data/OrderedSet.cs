using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Data
{
	class OrderedSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
	{
		public OrderedSet()
		{
			this.store = new List<T>();
			this.set = new HashSet<T>();
		}

		public T this[int index]
		{
			get
			{
				return this.store[index];
			}
		}

		public void Add(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.AddOverride(item);
		}

		public void Insert(int index, T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.store.Insert(index, item);
			this.set.Add(item);
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<T>>(items, "items");
			this.store.InsertRange(index, items);
			foreach (T item in items)
			{
				this.set.Add(item);
			}
		}

		public void AddRange(IEnumerable<T> items)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<T>>(items, "items");
			foreach (T item in items)
			{
				this.Add(item);
			}
		}

		public void Clear()
		{
			this.store.Clear();
			this.set.Clear();
		}

		public bool Contains(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			return this.set.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Guard.ThrowExceptionIfNull<T[]>(array, "array");
			this.set.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			bool result = this.store.Remove(item);
			if (!this.store.Contains(item))
			{
				this.set.Remove(item);
			}
			return result;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		protected virtual void AddOverride(T item)
		{
			Guard.ThrowExceptionIfNull<T>(item, "item");
			this.store.Add(item);
			this.set.Add(item);
		}

		readonly HashSet<T> set;

		readonly List<T> store;
	}
}
