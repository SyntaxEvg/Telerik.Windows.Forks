using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Not : PseudoSelector, IPseudoSelectorFilter, IPseudoSelector
	{
		public IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			IDomObject domObject = selection.FirstOrDefault<IDomObject>();
			if (domObject != null)
			{
				IList<IDomObject> second = this.SubSelector().Select(domObject.Document, selection);
				return selection.Except(second);
			}
			return Enumerable.Empty<IDomObject>();
		}

		Selector SubSelector()
		{
			return new Selector(string.Join(",", this.Parameters)).ToFilterSelector();
		}

		public override int MaximumParameterCount
		{
			get
			{
				return 1;
			}
		}

		public override int MinimumParameterCount
		{
			get
			{
				return 1;
			}
		}
	}
}
