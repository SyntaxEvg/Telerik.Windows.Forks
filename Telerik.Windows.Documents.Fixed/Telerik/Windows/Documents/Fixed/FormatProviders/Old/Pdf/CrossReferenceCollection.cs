using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf
{
	class CrossReferenceCollection
	{
		public CrossReferenceCollection()
		{
			this.crossReferences = new Dictionary<int, CrossReferenceEntryOld>();
		}

		public CrossReferenceEntryOld this[int objNumber]
		{
			get
			{
				return this.crossReferences[objNumber];
			}
		}

		public void AddCrossReferenceEntry(int objNumber, CrossReferenceEntryOld entry)
		{
			if (!this.crossReferences.ContainsKey(objNumber))
			{
				this.crossReferences[objNumber] = entry;
			}
		}

		public bool TryGetCrossReferenceEntry(int objNumber, out CrossReferenceEntryOld entry)
		{
			return this.crossReferences.TryGetValue(objNumber, out entry);
		}

		readonly Dictionary<int, CrossReferenceEntryOld> crossReferences;
	}
}
