using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery.Engine;

namespace CsQuery.Utility
{
	class SelectorCache
	{
		public SelectorCache(CQ cqSource)
		{
			this.CqSource = cqSource;
		}

		protected IDictionary<Selector, IList<IDomObject>> SelectionCache
		{
			get
			{
				if (this._SelectionCache == null)
				{
					this._SelectionCache = new Dictionary<Selector, IList<IDomObject>>();
				}
				return this._SelectionCache;
			}
		}

		public CQ Select(string selector)
		{
			Selector selector2 = new Selector(selector);
			IList<IDomObject> elements;
			if (this.SelectionCache.TryGetValue(selector2, out elements))
			{
				return new CQ(elements);
			}
			CQ cq = this.CqSource.Select(selector2);
			this.SelectionCache.Add(selector2, cq.Selection.ToList<IDomObject>());
			return cq;
		}

		CQ CqSource;

		IDictionary<Selector, IList<IDomObject>> _SelectionCache;
	}
}
