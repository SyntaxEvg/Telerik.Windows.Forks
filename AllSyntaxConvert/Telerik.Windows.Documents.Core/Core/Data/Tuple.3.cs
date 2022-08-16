using System;

namespace Telerik.Windows.Documents.Core.Data
{
	class Tuple<T1, T2, T3>
	{
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
		}

		public T3 Item3
		{
			get
			{
				return this.item3;
			}
		}

		public Tuple()
		{
		}

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			this.item1 = item1;
			this.item2 = item2;
			this.item3 = item3;
		}

		readonly T1 item1;

		readonly T2 item2;

		readonly T3 item3;
	}
}
