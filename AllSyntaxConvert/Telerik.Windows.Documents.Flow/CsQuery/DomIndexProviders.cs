using System;
using CsQuery.Engine;

namespace CsQuery
{
	static class DomIndexProviders
	{
		public static IDomIndexProvider Simple
		{
			get
			{
				return DomIndexProviders._SimpleDomIndexProvider;
			}
		}

		public static IDomIndexProvider Ranged
		{
			get
			{
				return DomIndexProviders._RangedDomIndexProvider;
			}
		}

		public static IDomIndexProvider None
		{
			get
			{
				return DomIndexProviders._NoDomIndexProvider;
			}
		}

		static IDomIndexProvider _RangedDomIndexProvider = new RangedDomIndexProvider();

		static IDomIndexProvider _SimpleDomIndexProvider = new SimpleDomIndexProvider();

		static IDomIndexProvider _NoDomIndexProvider = new NoDomIndexProvider();
	}
}
