using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Model.Formatting
{
	class UniqueItemsStore<T> : IEnumerable<T>, IEnumerable
	{
		public UniqueItemsStore(int startIndex = 0)
		{
			this.startIndex = startIndex;
			this.indexToValue = new Dictionary<int, T>();
			this.valueToIndex = new Dictionary<T, int>();
		}

		public int Count
		{
			get
			{
				return this.indexToValue.Count;
			}
		}

		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				this.startIndex = value;
			}
		}

		public T GetValueByIndex(int index)
		{
			return this.indexToValue[index];
		}

		public int GetIndexByValue(T value)
		{
			return this.valueToIndex[value];
		}

		public int GetNextIndex(T value)
		{
			if (this.ContainsValue(value))
			{
				return this.valueToIndex[value];
			}
			return this.GetNextIndexInternal();
		}

		public int Add(T value)
		{
			if (this.ContainsValue(value))
			{
				return this.valueToIndex[value];
			}
			int nextIndexInternal = this.GetNextIndexInternal();
			this.Add(nextIndexInternal, value);
			return nextIndexInternal;
		}

		public void Add(int index, T value)
		{
			if (this.ContainsIndex(index))
			{
				if (!ObjectExtensions.EqualsOfT<T>(this.indexToValue[index], value))
				{
					throw new InvalidOperationException("Different item with the same index exists.");
				}
				return;
			}
			else
			{
				if (!this.ContainsValue(value))
				{
					this.indexToValue.Add(index, value);
					this.valueToIndex.Add(value, index);
					return;
				}
				if (!ObjectExtensions.EqualsOfT<int>(this.valueToIndex[value], index))
				{
					throw new InvalidOperationException("Same item with different index exists.");
				}
				return;
			}
		}

		public void Remove(T value)
		{
			int index = this.valueToIndex[value];
			this.Remove(index, value);
		}

		public void RemoveAtIndex(int index)
		{
			T value = this.indexToValue[index];
			this.Remove(index, value);
		}

		public bool ContainsValue(T value)
		{
			return this.valueToIndex.ContainsKey(value);
		}

		public bool ContainsIndex(int index)
		{
			return this.indexToValue.ContainsKey(index);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return (from p in this.indexToValue
				orderby p.Key
				select p.Value).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal void Remove(int index, T value)
		{
			this.indexToValue.Remove(index);
			this.valueToIndex.Remove(value);
		}

		int GetNextIndexInternal()
		{
			return this.startIndex++;
		}

		readonly Dictionary<int, T> indexToValue;

		readonly Dictionary<T, int> valueToIndex;

		int startIndex;
	}
}
