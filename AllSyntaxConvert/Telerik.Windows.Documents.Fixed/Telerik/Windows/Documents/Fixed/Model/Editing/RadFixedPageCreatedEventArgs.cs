using System;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	class RadFixedPageCreatedEventArgs : EventArgs
	{
		public RadFixedPageCreatedEventArgs(SectionInfo section)
		{
			Guard.ThrowExceptionIfNull<SectionInfo>(section, "section");
			this.section = section;
		}

		public SectionInfo Section
		{
			get
			{
				return this.section;
			}
		}

		readonly SectionInfo section;
	}
}
