using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	abstract class NthChildSelector : PseudoSelector, IPseudoSelectorChild, IPseudoSelector
	{
		protected NthChildMatcher NthC
		{
			get
			{
				if (this._NthC == null)
				{
					this._NthC = new NthChildMatcher();
				}
				return this._NthC;
			}
		}

		public abstract bool Matches(IDomObject element);

		public abstract IEnumerable<IDomObject> ChildMatches(IDomContainer element);

		public override int MinimumParameterCount
		{
			get
			{
				return 1;
			}
		}

		public override int MaximumParameterCount
		{
			get
			{
				return 1;
			}
		}

		NthChildMatcher _NthC;
	}
}
