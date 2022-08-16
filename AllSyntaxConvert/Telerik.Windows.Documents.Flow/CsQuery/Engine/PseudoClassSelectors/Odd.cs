using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Odd : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			int index = 0;
			foreach (IDomObject child in selection)
			{
				if (index % 2 != 0)
				{
					yield return child;
				}
				index++;
			}
			yield break;
		}
	}
}
