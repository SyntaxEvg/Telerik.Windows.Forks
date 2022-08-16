using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.Internal.Collections
{
	sealed class PagesCollection : IEnumerable<RadFixedPageInternal>, IEnumerable
	{
		internal PagesCollection(IEnumerable<RadFixedPageInternal> pages)
		{
			this.pages = new List<RadFixedPageInternal>(pages);
		}

		public RadFixedPageInternal this[int index]
		{
			get
			{
				return this.pages[index];
			}
		}

		public int Count
		{
			get
			{
				return this.pages.Count;
			}
		}

		public IEnumerator<RadFixedPageInternal> GetEnumerator()
		{
			return this.pages.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.pages.GetEnumerator();
		}

		readonly List<RadFixedPageInternal> pages;
	}
}
