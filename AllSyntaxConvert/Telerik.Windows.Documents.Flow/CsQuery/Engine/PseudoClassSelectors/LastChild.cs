using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class LastChild : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			return element.ParentNode.LastElementChild == element;
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			IDomElement child = element.LastElementChild;
			if (child != null)
			{
				yield return child;
			}
			yield break;
		}
	}
}
