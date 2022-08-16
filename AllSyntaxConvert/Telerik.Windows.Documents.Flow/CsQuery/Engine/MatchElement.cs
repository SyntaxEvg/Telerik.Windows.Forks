using System;

namespace CsQuery.Engine
{
	class MatchElement
	{
		public MatchElement(IDomElement element)
		{
			this.Initialize(element, 0);
		}

		public MatchElement(IDomElement element, int depth)
		{
			this.Initialize(element, depth);
		}

		protected void Initialize(IDomElement element, int depth)
		{
			this.Depth = depth;
			this.Element = element;
		}

		public int Depth { get; protected set; }

		public IDomElement Element { get; protected set; }
	}
}
