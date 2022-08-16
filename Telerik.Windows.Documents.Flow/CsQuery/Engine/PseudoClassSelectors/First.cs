using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class First : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			IDomObject first = selection.FirstOrDefault<IDomObject>();
			if (first != null)
			{
				yield return first;
			}
			yield break;
		}
	}
}
