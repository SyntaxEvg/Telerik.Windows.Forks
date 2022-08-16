using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class CrossReferenceSection
	{
		public int First { get; set; }

		public CrossReferenceEntryOld[] Entries { get; set; }
	}
}
