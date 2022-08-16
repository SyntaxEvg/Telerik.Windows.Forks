using System;
using System.Collections.Generic;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Lt : Indexed
	{
		public override IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			return Lt.IndexLessThan(selection, base.Index);
		}

		static IEnumerable<IDomObject> IndexLessThan(IEnumerable<IDomObject> list, int position)
		{
			int index = 0;
			foreach (IDomObject obj in list)
			{
				int num;
				index = (num = index) + 1;
				if (num < position)
				{
					yield return obj;
				}
			}
			yield break;
		}
	}
}
