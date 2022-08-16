using System;

namespace CsQuery.Engine
{
	interface IDomIndex
	{
		void AddToIndex(IDomIndexedNode element);

		void AddToIndex(ushort[] key, IDomIndexedNode element);

		void RemoveFromIndex(IDomIndexedNode element);

		void RemoveFromIndex(ushort[] key, IDomIndexedNode element);

		void Clear();

		int Count { get; }
	}
}
