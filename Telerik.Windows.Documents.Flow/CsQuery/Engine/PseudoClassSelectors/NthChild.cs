using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class NthChild : NthChildSelector
	{
		public override bool Matches(IDomObject element)
		{
			return element.NodeType == NodeType.ELEMENT_NODE && base.NthC.IsNthChild((IDomElement)element, this.Parameters[0], false);
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			return base.NthC.NthChilds(element, this.Parameters[0], false);
		}
	}
}
