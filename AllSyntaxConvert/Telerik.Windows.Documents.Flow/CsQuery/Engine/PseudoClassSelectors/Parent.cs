using System;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Parent : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			return element.HasChildren && !Empty.IsEmpty(element);
		}
	}
}
