using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.Implementation;

namespace CsQuery.Engine
{
	class DomIndexSimple : IDomIndex, IDomIndexSimple
	{
		public DomIndexSimple()
		{
			this.Index = new Dictionary<ushort[], DomIndexSimple.IndexValue>(PathKeyComparer.Comparer);
		}

		public void AddToIndex(IDomIndexedNode element)
		{
			if (element.HasChildren)
			{
				foreach (IDomElement domElement in ((IDomContainer)element).ChildElements)
				{
					DomElement element2 = (DomElement)domElement;
					this.AddToIndex(element2);
				}
			}
			foreach (ushort[] key in element.IndexKeys())
			{
				this.AddToIndex(key, element);
			}
		}

		public void AddToIndex(ushort[] key, IDomIndexedNode element)
		{
			DomIndexSimple.IndexValue value;
			if (!this.Index.TryGetValue(key, out value))
			{
				value.Initialize();
				value.Set.Add(element.IndexReference);
				this.Index.Add(key, value);
				return;
			}
			value.Set.Add(element.IndexReference);
			value.IsSorted = false;
		}

		public void RemoveFromIndex(ushort[] key, IDomIndexedNode element)
		{
			DomIndexSimple.IndexValue indexValue;
			if (this.Index.TryGetValue(key, out indexValue))
			{
				indexValue.Set.Remove(element.IndexReference);
				indexValue.IsSorted = false;
			}
		}

		public void RemoveFromIndex(IDomIndexedNode element)
		{
			if (element.HasChildren)
			{
				foreach (IDomElement domElement in ((IDomContainer)element).ChildElements)
				{
					if (domElement.IsIndexed)
					{
						this.RemoveFromIndex(domElement);
					}
				}
			}
			foreach (ushort[] key in element.IndexKeys())
			{
				this.RemoveFromIndex(key, element);
			}
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] subKey)
		{
			DomIndexSimple.IndexValue indexValue;
			if (this.Index.TryGetValue(subKey, out indexValue))
			{
				if (!indexValue.IsSorted)
				{
					indexValue.Set.Sort();
				}
				return indexValue.Set;
			}
			return Enumerable.Empty<IDomObject>();
		}

		public void Clear()
		{
			this.Index.Clear();
		}

		public int Count
		{
			get
			{
				return this.Index.Count;
			}
		}

		public bool QueueChanges
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] subKey, int depth, bool includeDescendants)
		{
			throw new NotImplementedException();
		}

		IDictionary<ushort[], DomIndexSimple.IndexValue> Index;

		struct IndexValue
		{
			public void Initialize()
			{
				this.Set = new List<IDomObject>();
			}

			public void Sort()
			{
				this.Set.Sort();
				this.IsSorted = true;
			}

			public List<IDomObject> Set;

			public bool IsSorted;
		}
	}
}
