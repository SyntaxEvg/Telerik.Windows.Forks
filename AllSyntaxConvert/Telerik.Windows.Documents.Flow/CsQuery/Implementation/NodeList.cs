using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CsQuery.Implementation
{
	class NodeList<T> : INodeList<T>, IReadOnlyList<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable where T : IDomObject
	{
		public NodeList(IList<T> list)
		{
			this.InnerList = list;
			this.IsReadOnly = true;
		}

		public NodeList(IEnumerable<T> sequence)
		{
			this.InnerList = new List<T>(sequence);
			this.IsReadOnly = true;
		}

		public int Length
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		public T Item(int index)
		{
			return this[index];
		}

		public int IndexOf(T item)
		{
			return this.InnerList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			this.InnerList.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.InnerList.RemoveAt(index);
		}

		[IndexerName("Indexer")]
		public T this[int index]
		{
			get
			{
				return this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		public void Add(T item)
		{
			this.InnerList.Add(item);
		}

		public void Clear()
		{
			this.InnerList.Clear();
		}

		public bool Contains(T item)
		{
			return this.InnerList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.InnerList.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		public bool IsReadOnly { get; protected set; }

		public bool Remove(T item)
		{
			return this.InnerList.Remove(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public IList<T> ToList()
		{
			return new List<T>(this).AsReadOnly();
		}

		//T IReadOnlyList<T>.get_Item(int A_1)
		//{
		//	return this[A_1];
		//}

		protected IList<T> InnerList;
	}
}
