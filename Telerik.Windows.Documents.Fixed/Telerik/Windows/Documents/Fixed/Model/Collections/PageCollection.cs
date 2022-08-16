using System;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class PageCollection : DocumentElementCollection<RadFixedPage, RadFixedDocument>
	{
		internal PageCollection(RadFixedDocument parent)
			: base(parent)
		{
		}

		public RadFixedPage AddPage()
		{
			RadFixedPage radFixedPage = new RadFixedPage();
			base.Add(radFixedPage);
			return radFixedPage;
		}
	}
}
