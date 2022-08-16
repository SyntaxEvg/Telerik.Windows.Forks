using System;
using Telerik.Windows.Documents.Core.Fonts.OpenType.Utils;

namespace Telerik.Windows.Documents.Core.Fonts
{
	class FallbackRecord
	{
		public FallbackRecord(FallbackRange range, FontProperties descr)
		{
			this.Range = range;
			this.FontDescriptor = descr;
		}

		public FallbackRange Range { get; set; }

		public FontProperties FontDescriptor { get; set; }

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + ((this.Range != null) ? this.Range.GetHashCode() : 0);
			return num * 23 + ((this.FontDescriptor != null) ? this.FontDescriptor.GetHashCode() : 0);
		}

		public override bool Equals(object obj)
		{
			FallbackRecord fallbackRecord = obj as FallbackRecord;
			return fallbackRecord != null && this.Range == fallbackRecord.Range && this.FontDescriptor == fallbackRecord.FontDescriptor;
		}
	}
}
