using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CsQuery.ExtensionMethods;
using CsQuery.ExtensionMethods.Internal;

namespace CsQuery.Implementation
{
	class SelectionSet<T> : ISet<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : IDomObject, ICloneable
	{
		public SelectionSet(SelectionSetOrder outputOrder)
		{
			this.OriginalOrder = SelectionSetOrder.OrderAdded;
			this.OutputOrder = SelectionSetOrder.OrderAdded;
			this.OriginalList = SelectionSet<T>.EmptyList();
		}

		public SelectionSet(IEnumerable<T> elements, SelectionSetOrder inputOrder, SelectionSetOrder outputOrder)
		{
			this.OriginalOrder = ((inputOrder == (SelectionSetOrder)0) ? SelectionSetOrder.OrderAdded : inputOrder);
			this.OutputOrder = ((outputOrder == (SelectionSetOrder)0) ? this.OriginalOrder : outputOrder);
			this.OriginalList = elements ?? SelectionSet<T>.EmptyList();
		}

		protected HashSet<T> MutableList
		{
			get
			{
				if (!this.IsAltered)
				{
					this.ConvertToMutable();
				}
				return this._MutableList;
			}
		}

		List<T> MutableListOrdered
		{
			get
			{
				if (!this.IsAltered)
				{
					this.ConvertToMutable();
				}
				return this._MutableListOrdered;
			}
		}

		protected IEnumerable<T> OrderedList
		{
			get
			{
				if (this.IsDirty || this._OrderedList == null)
				{
					if (!this.IsDirty && this.OriginalOrder == this.OutputOrder)
					{
						this._OrderedList = this.OriginalList;
					}
					else
					{
						switch (this.OutputOrder)
						{
						case SelectionSetOrder.OrderAdded:
							this._OrderedList = this.MutableListOrdered;
							break;
						case SelectionSetOrder.Ascending:
							this._OrderedList = this.MutableList.OrderBy((T item) => item.NodePath, PathKeyComparer.Comparer);
							break;
						case SelectionSetOrder.Descending:
							this._OrderedList = this.MutableList.OrderByDescending((T item) => item.NodePath, PathKeyComparer.Comparer);
							break;
						}
						this.Clean();
					}
				}
				return this._OrderedList;
			}
		}

		protected bool IsDirty
		{
			get
			{
				return this._IsDirty;
			}
		}

		protected bool IsAltered
		{
			get
			{
				return this._MutableList != null;
			}
		}

		public SelectionSetOrder OutputOrder { get; set; }

		public int Count
		{
			get
			{
				if (this.IsAltered)
				{
					return this.MutableList.Count;
				}
				return this.OriginalList.Count<T>();
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Add(T item)
		{
			if (this.MutableList.Add(item))
			{
				this.MutableListOrdered.Add(item);
				this.Touch();
				return true;
			}
			return false;
		}

		public void Clear()
		{
			this.OriginalList = SelectionSet<T>.EmptyList();
			this._OrderedList = null;
			this._MutableList = null;
			this._MutableListOrdered = null;
		}

		public SelectionSet<T> Clone()
		{
			return new SelectionSet<T>(this.CloneImpl(), this.OutputOrder, this.OutputOrder);
		}

		protected IEnumerable<T> CloneImpl()
		{
			foreach (T item in this.OrderedList)
			{
				T t = item;
				yield return (T)((object)t.Clone());
			}
			yield break;
		}

		public bool Contains(T item)
		{
			if (!this.IsAltered)
			{
				return this.OriginalList.Contains(item);
			}
			return this.MutableList.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			int num = 0;
			foreach (T t in this.OrderedList)
			{
				array[num + arrayIndex] = t;
				num++;
			}
		}

		public bool Remove(T item)
		{
			if (this.MutableList.Remove(item))
			{
				this.MutableListOrdered.Remove(item);
				this.Touch();
				return true;
			}
			return false;
		}

		public void ExceptWith(IEnumerable<T> other)
		{
			this.MutableList.ExceptWith(other);
			this.SynchronizeOrderedList();
			this.Touch();
		}

		public void IntersectWith(IEnumerable<T> other)
		{
			this.MutableList.IntersectWith(other);
			this.SynchronizeOrderedList();
			this.Touch();
		}

		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return this.MutableList.IsProperSubsetOf(other);
		}

		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return this.MutableList.IsProperSupersetOf(other);
		}

		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return this.MutableList.IsSubsetOf(other);
		}

		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return this.MutableList.IsSupersetOf(other);
		}

		public bool Overlaps(IEnumerable<T> other)
		{
			return this.MutableList.Overlaps(other);
		}

		public bool SetEquals(IEnumerable<T> other)
		{
			return this.MutableList.SetEquals(other);
		}

		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			this.MutableList.SymmetricExceptWith(other);
			this.SynchronizeOrderedList();
			this.Touch();
		}

		public void UnionWith(IEnumerable<T> other)
		{
			this.AddRange(other);
		}

		public int IndexOf(T item)
		{
			return this.OrderedList.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			if (this.MutableList.Add(item))
			{
				this.MutableListOrdered.Insert(index, item);
				this.Touch();
			}
		}

		public void RemoveAt(int index)
		{
			if (index >= this.Count || this.Count == 0)
			{
				throw new IndexOutOfRangeException("Index out of range");
			}
			T item = this.OrderedList.ElementAt(index);
			this.MutableList.Remove(item);
			this.MutableListOrdered.Remove(item);
			this.Touch();
		}

		public T this[int index]
		{
			get
			{
				return this.OrderedList.ElementAt(index);
			}
			set
			{
				T item = this.OrderedList.ElementAt(index);
				this.MutableList.Remove(item);
				this.MutableList.Add(value);
				int index2 = this.MutableListOrdered.IndexOf(item);
				this.MutableListOrdered[index2] = value;
				this.Touch();
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.OrderedList.GetEnumerator();
		}

		void ConvertToMutable()
		{
			this._MutableList = ((this.OriginalList == null) ? new HashSet<T>() : new HashSet<T>(this.OriginalList));
			this._MutableListOrdered = new List<T>();
			this._MutableListOrdered.AddRange(this.OriginalList);
			this.Touch();
		}

		static IEnumerable<T> EmptyList()
		{
			yield break;
		}

		void Touch()
		{
			this._IsDirty = true;
		}

		void Clean()
		{
			this._IsDirty = false;
		}

		void SynchronizeOrderedList()
		{
			int i = 0;
			while (i < this.MutableListOrdered.Count)
			{
				if (!this.MutableList.Contains(this.MutableListOrdered[i]))
				{
					this.MutableListOrdered.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void ICollection<T>.Add(T item)
		{
			this.Add(item);
		}

		bool _IsDirty;

		SelectionSetOrder OriginalOrder;

		IEnumerable<T> OriginalList;

		IEnumerable<T> _OrderedList;

		HashSet<T> _MutableList;

		List<T> _MutableListOrdered;
	}
}
