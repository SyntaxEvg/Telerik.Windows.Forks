using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine
{
	abstract class PseudoSelectorFilter : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public abstract bool Matches(IDomObject element);

		public virtual IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> elements)
		{
			return from item in elements
				where this.Matches(item)
				select item;
		}
	}
}
