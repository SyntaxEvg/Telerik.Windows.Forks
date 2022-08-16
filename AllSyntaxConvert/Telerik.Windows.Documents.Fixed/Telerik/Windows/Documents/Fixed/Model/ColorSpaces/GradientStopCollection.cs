using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class GradientStopCollection : IList<GradientStop>, ICollection<GradientStop>, IEnumerable<GradientStop>, IEnumerable
	{
		public GradientStopCollection()
		{
			this.store = new List<GradientStop>();
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

		public GradientStop this[int index]
		{
			get
			{
				return this.store[index];
			}
			set
			{
				this.store[index] = value;
			}
		}

		public int IndexOf(GradientStop item)
		{
			return this.store.IndexOf(item);
		}

		public void Insert(int index, GradientStop item)
		{
			this.store.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.store.RemoveAt(index);
		}

		public void Add(GradientStop item)
		{
			this.store.Add(item);
		}

		public void Clear()
		{
			this.store.Clear();
		}

		public bool Contains(GradientStop item)
		{
			return this.store.Contains(item);
		}

		public void CopyTo(GradientStop[] array, int arrayIndex)
		{
			this.store.CopyTo(array, arrayIndex);
		}

		public bool Remove(GradientStop item)
		{
			return this.store.Remove(item);
		}

		public IEnumerator<GradientStop> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		readonly List<GradientStop> store;
	}
}
