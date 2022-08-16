using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Data
{
	public abstract class DocumentElementCollectionBase<T, TOwner> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		internal DocumentElementCollectionBase(TOwner owner)
		{
			Guard.ThrowExceptionIfNull<TOwner>(owner, "owner");
			this.Owner = owner;
			this.store = new List<T>();
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		protected TOwner Owner { get; set; }

		public T this[int index]
		{
			get
			{
				return this.store[index];
			}
			set
			{
				this.VerifyDocumentElementOnInsert(value);
				T item = this.store[index];
				this.store[index] = value;
				this.OnAfterRemove(item);
				this.SetParent(item, default(TOwner));
				this.SetParent(value, this.Owner);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return this.store.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			this.VerifyDocumentElementOnInsert(item);
			this.store.Insert(index, item);
			this.SetParent(item, this.Owner);
		}

		public void InsertRange(int index, IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				this.VerifyDocumentElementOnInsert(item);
			}
			this.store.InsertRange(index, items);
			foreach (T item2 in items)
			{
				this.SetParent(item2, this.Owner);
			}
		}

		public void RemoveAt(int index)
		{
			T item = this.store[index];
			this.store.RemoveAt(index);
			this.OnAfterRemove(item);
			this.SetParent(item, default(TOwner));
		}

		public void RemoveRange(int index, int count)
		{
			T[] array = this.store.Skip(index).Take(count).ToArray<T>();
			this.store.RemoveRange(index, count);
			foreach (T item in array)
			{
				this.OnAfterRemove(item);
				this.SetParent(item, default(TOwner));
			}
		}

		protected virtual void OnAfterRemove(T item)
		{
		}

		public void Clear()
		{
			for (int i = this.store.Count - 1; i >= 0; i--)
			{
				this.RemoveAt(i);
			}
		}

		public bool Contains(T item)
		{
			return this.store.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.store.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			bool flag = this.store.Remove(item);
			if (flag)
			{
				this.OnAfterRemove(item);
				this.SetParent(item, default(TOwner));
			}
			return flag;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public void Add(T item)
		{
			this.VerifyDocumentElementOnInsert(item);
			this.store.Add(item);
			this.SetParent(item, this.Owner);
		}

		protected abstract void VerifyDocumentElementOnInsert(T item);

		protected abstract void SetParent(T item, TOwner parent);

		readonly List<T> store;
	}
}
