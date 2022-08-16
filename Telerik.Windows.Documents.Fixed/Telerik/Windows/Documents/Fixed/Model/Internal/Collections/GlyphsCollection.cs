using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Collections
{
	class GlyphsCollection : IList<GlyphOld>, ICollection<GlyphOld>, IEnumerable<GlyphOld>, IEnumerable
	{
		public GlyphsCollection()
		{
			this.store = new List<GlyphOld>();
		}

		public GlyphOld this[int index]
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		void Compare(Rect rect, ref double minX, ref double maxX, ref double minY, ref double maxY)
		{
			minX = System.Math.Min(rect.Left, minX);
			maxX = Math.Max(rect.Right, maxX);
			minY = System.Math.Min(rect.Top, minY);
			maxY = Math.Max(rect.Bottom, maxY);
		}

		internal Rect GetBoundingRect()
		{
			double positiveInfinity = double.PositiveInfinity;
			double positiveInfinity2 = double.PositiveInfinity;
			double negativeInfinity = double.NegativeInfinity;
			double negativeInfinity2 = double.NegativeInfinity;
			foreach (GlyphOld glyphOld in this)
			{
				this.Compare(glyphOld.BoundingRect, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
			}
			return new Rect(new Point(positiveInfinity, positiveInfinity2), new Point(negativeInfinity, negativeInfinity2));
		}

		public int IndexOf(GlyphOld item)
		{
			return this.store.IndexOf(item);
		}

		public void Insert(int index, GlyphOld item)
		{
			this.store.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.store.RemoveAt(index);
		}

		public void Add(GlyphOld item)
		{
			this.store.Add(item);
		}

		public void Clear()
		{
			this.store.Clear();
		}

		public bool Contains(GlyphOld item)
		{
			return this.store.Contains(item);
		}

		public void CopyTo(GlyphOld[] array, int arrayIndex)
		{
			this.store.CopyTo(array, arrayIndex);
		}

		public bool Remove(GlyphOld item)
		{
			return this.store.Remove(item);
		}

		public IEnumerator<GlyphOld> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public void AddRange(IEnumerable<GlyphOld> glyphs)
		{
			foreach (GlyphOld item in glyphs)
			{
				this.store.Add(item);
			}
		}

		readonly List<GlyphOld> store;
	}
}
