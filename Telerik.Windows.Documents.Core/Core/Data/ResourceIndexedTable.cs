using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Core.Data
{
	class ResourceIndexedTable<T> : IEnumerable<T>, IEnumerable
	{
		public ResourceIndexedTable(bool uniqueResources = true, int startIndex = 0)
		{
			this.startIndex = startIndex;
			this.uniqueResources = uniqueResources;
			if (uniqueResources)
			{
				this.resourceToIndex = new QueueDictionary<T, int>();
			}
			this.indexToResource = new QueueDictionary<int, T>();
		}

		public T this[int index]
		{
			get
			{
				return this.indexToResource[index];
			}
			set
			{
				this.indexToResource[index] = value;
			}
		}

		public int Add(T resource)
		{
			int index = this.startIndex + this.indexToResource.Count;
			return this.Add(resource, index);
		}

		public int Add(T resource, int index)
		{
			int result;
			if (this.uniqueResources)
			{
				if (!this.resourceToIndex.TryGetValue(resource, out result))
				{
					this.resourceToIndex.Add(resource, index);
					this.indexToResource.Add(index, resource);
					result = index;
				}
			}
			else
			{
				result = index;
				this.indexToResource.Add(index, resource);
			}
			return result;
		}

		public void AddRange(IEnumerable<T> resources)
		{
			foreach (T resource in resources)
			{
				this.Add(resource);
			}
		}

		public bool Contains(T resource)
		{
			return this.resourceToIndex.ContainsKey(resource);
		}

		public int GetIndex(T resource)
		{
			return this.resourceToIndex[resource];
		}

		public IEnumerator<T> GetEnumerator()
		{
			foreach (KeyValuePair<int, T> keyValuePair in this.indexToResource)
			{
				KeyValuePair<int, T> keyValuePair2 = keyValuePair;
				yield return keyValuePair2.Value;
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		readonly bool uniqueResources;

		readonly int startIndex;

		readonly QueueDictionary<T, int> resourceToIndex;

		readonly QueueDictionary<int, T> indexToResource;
	}
}
