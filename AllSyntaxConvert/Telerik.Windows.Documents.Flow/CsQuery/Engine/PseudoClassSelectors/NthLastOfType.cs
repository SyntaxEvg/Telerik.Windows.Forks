using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class NthLastOfType : NthChildSelector
	{
		public override bool Matches(IDomObject element)
		{
			return element.NodeType == NodeType.ELEMENT_NODE && base.NthC.IsNthChildOfType((IDomElement)element, this.Parameters[0], true);
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			return base.NthC.NthChildsOfType(element, this.Parameters[0], true);
		}
	}
}
