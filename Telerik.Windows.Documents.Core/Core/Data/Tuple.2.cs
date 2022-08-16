using System;

namespace Telerik.Windows.Documents.Core.Data
{
	class Tuple<T1, T2>
	{
		public T1 Item1
		{
			get
			{
				return this.item1;
			}
			set
			{
				this.item1 = value;
				this.isEmpty = false;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.item2;
			}
			set
			{
				this.item2 = value;
				this.isEmpty = false;
			}
		}

		public Tuple()
		{
			this.item1 = default(T1);
			this.item2 = default(T2);
			this.isEmpty = true;
		}

		public Tuple(T1 item1, T2 item2)
		{
			this.item1 = item1;
			this.item2 = item2;
		}

		public bool IsEmpty()
		{
			return this.isEmpty;
		}

		bool isEmpty;

		T1 item1;

		T2 item2;
	}
}
