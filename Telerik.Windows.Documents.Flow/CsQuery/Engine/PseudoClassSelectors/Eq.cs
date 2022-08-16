using System;
using System.Collections.Generic;
using System.Linq;

namespace CsQuery.Engine.PseudoClassSelectors
{
	class Eq : Indexed
	{
		public override IEnumerable<IDomObject> Filter(IEnumerable<IDomObject> selection)
		{
			IDomObject el = this.ElementAtIndex(selection, base.Index);
			if (el != null)
			{
				yield return el;
			}
			yield break;
		}

		IDomObject ElementAtIndex(IEnumerable<IDomObject> list, int index)
		{
			if (index < 0)
			{
				index = list.Count<IDomObject>() + index;
			}
			bool flag = true;
			IEnumerator<IDomObject> enumerator = list.GetEnumerator();
			int num = 0;
			while (num <= index && flag)
			{
				flag = enumerator.MoveNext();
				num++;
			}
			if (flag)
			{
				return enumerator.Current;
			}
			return null;
		}
	}
}
