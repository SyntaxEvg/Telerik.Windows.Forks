using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Telerik.Windows.Documents.Core.Data
{
	class ExpandoList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		public ExpandoList()
		{
			this.list = new List<T>();
		}

		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public T this[int index]
		{
			get
			{
				if (index >= this.list.Count)
				{
					return default(T);
				}
				return this.list[index];
			}
			set
			{
				this.ExpandTo(index + 1);
				this.list[index] = value;
			}
		}

		public int IndexOf(T item)
		{
			return this.list.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			this.ExpandTo(index + 1);
			this.list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		public void Add(T item)
		{
			this.list.Add(item);
		}

		public void Clear()
		{
			this.list.Clear();
		}

		public bool Contains(T item)
		{
			return this.list.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return this.list.Remove(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		void ExpandTo(int length)
		{
			if (this.list.Count < length)
			{
				this.list.AddRange(Enumerable.Repeat<T>(default(T), length - this.list.Count));
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		readonly List<T> list;
	}
}
