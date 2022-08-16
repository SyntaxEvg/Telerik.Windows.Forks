using System;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Header : PseudoSelectorFilter
	{
		public override bool Matches(IDomObject element)
		{
			string nodeName = element.NodeName;
			return nodeName[0] == 'H' && nodeName.Length == 2 && nodeName[1] >= '0' && nodeName[1] <= '6';
		}
	}
}
