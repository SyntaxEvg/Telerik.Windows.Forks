using System;

namespace CsQuery.Implementation
{
	class NodeEventArgs : EventArgs
	{
		public NodeEventArgs(IDomObject node)
		{
			this.Node = node;
		}

		public IDomObject Node { get; protected set; }
	}
}
