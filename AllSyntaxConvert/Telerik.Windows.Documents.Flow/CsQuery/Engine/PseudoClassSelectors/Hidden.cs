using System;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Hidden : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			return !Visible.IsVisible(element);
		}
	}
}
