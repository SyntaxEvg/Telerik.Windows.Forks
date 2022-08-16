using System;
using System.Collections.Generic;

namespace CsQuery.Engine
{
	class DomIndexNone : IDomIndex
	{
		public void AddToIndex(IDomIndexedNode element)
		{
		}

		public void AddToIndex(ushort[] key, IDomIndexedNode element)
		{
		}

		public void RemoveFromIndex(ushort[] key, IDomIndexedNode element)
		{
		}

		public void RemoveFromIndex(IDomIndexedNode element)
		{
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] subKey)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<IDomObject> QueryIndex(ushort[] subKey, int depth, bool includeDescendants)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
		}

		public int Count
		{
			get
			{
				return 0;
			}
		}

		public DomIndexFeatures Features
		{
			get
			{
				return (DomIndexFeatures)0;
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
	}
}
