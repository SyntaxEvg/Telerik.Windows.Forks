using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	public class SectionProperties
	{
		public SectionProperties()
		{
			this.PageSize = FixedDocumentDefaults.PageSize;
			this.PageMargins = FixedDocumentDefaults.PageMargins;
			this.PageRotation = FixedDocumentDefaults.PageRotation;
		}

		public Size PageSize { get; set; }

		public Padding PageMargins { get; set; }

		public Rotation PageRotation { get; set; }

		public void CopyPropertiesFrom(SectionProperties fromProperties)
		{
			this.PageSize = fromProperties.PageSize;
			this.PageMargins = fromProperties.PageMargins;
			this.PageRotation = fromProperties.PageRotation;
		}

		internal void CopyTo(RadFixedPage page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPage>(page, "page");
			page.Size = this.PageSize;
			page.Rotation = this.PageRotation;
		}
	}
}
