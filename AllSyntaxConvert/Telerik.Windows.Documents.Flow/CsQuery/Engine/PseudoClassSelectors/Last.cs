using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Last : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			IDomObject last = selection.LastOrDefault<IDomObject>();
			if (last != null)
			{
				yield return last;
			}
			yield break;
		}
	}
}
