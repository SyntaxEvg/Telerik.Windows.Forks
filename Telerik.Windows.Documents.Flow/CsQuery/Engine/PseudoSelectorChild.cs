using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine
{
	abstract class PseudoSelectorChild : PseudoSelector, IPseudoSelectorChild, IPseudoSelector
	{
		public abstract bool Matches(IDomObject element);

		public virtual IEnumerable<IDomObject> ChildMatches(IDomContainer element)
		{
			return from item in element.ChildElements
				where this.Matches(item)
				select item;
		}
	}
}
