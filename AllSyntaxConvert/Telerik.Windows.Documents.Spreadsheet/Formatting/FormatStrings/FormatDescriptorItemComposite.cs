using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings
{
	class FormatDescriptorItemComposite : FormatDescriptorItem
	{
		public FormatDescriptorItemComposite(Func<double?, string, IEnumerable<FormatDescriptorItem>> getItems, string startSequence)
			: base(string.Empty, false, false, true)
		{
			this.getCompositeItems = getItems;
			this.startSequence = startSequence;
		}

		public override IEnumerable<FormatDescriptorItem> GetItems(double? doubleValue)
		{
			return this.getCompositeItems(doubleValue, this.startSequence);
		}

		readonly Func<double?, string, IEnumerable<FormatDescriptorItem>> getCompositeItems;

		readonly string startSequence;
	}
}
