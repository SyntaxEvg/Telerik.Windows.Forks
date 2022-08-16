using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class NthLastChild : NthChildSelector
	{
		public override bool Matches(IDomObject element)
		{
			return element.NodeType == NodeType.ELEMENT_NODE && base.NthC.IsNthChild((IDomElement)element, this.Parameters[0], true);
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			return base.NthC.NthChilds(element, this.Parameters[0], true);
		}
	}
}
