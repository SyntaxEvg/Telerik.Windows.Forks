using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class FirstChild : PseudoSelectorChild
	{
		public override bool Matches(IDomObject element)
		{
			return element.ParentNode.FirstElementChild == element;
		}

		public override IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			IDomObject child = element.FirstChild;
			if (child != null)
			{
				yield return element.FirstElementChild;
			}
			yield break;
		}
	}
}
