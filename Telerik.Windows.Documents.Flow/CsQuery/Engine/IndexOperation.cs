using System;

namespace CsQuery.Engine
{
	struct IndexOperation
	{
		public IndexOperationType IndexOperationType;

		public ushort[] Key;

		public IDomObject Value;
	}
}
