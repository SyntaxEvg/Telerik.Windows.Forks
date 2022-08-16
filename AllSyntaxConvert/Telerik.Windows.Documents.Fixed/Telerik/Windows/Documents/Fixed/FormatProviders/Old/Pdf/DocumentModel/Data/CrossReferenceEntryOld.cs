using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class CrossReferenceEntryOld
	{
		public int Field3 { get; set; }

		public long Field2 { get; set; }

		public CrossReferenceEntryTypeOld Type { get; set; }

		public string ToCrossReferenceTableString()
		{
			string arg = ((this.Type == CrossReferenceEntryTypeOld.Free) ? "f" : "n");
			return string.Format("{0:0000000000} {1:00000} {2}", this.Field2, this.Field3, arg);
		}

		public override string ToString()
		{
			string text = ((this.Type == CrossReferenceEntryTypeOld.Used) ? "n" : ((this.Type == CrossReferenceEntryTypeOld.Compressed) ? "c" : "f"));
			char c = ' ';
			return string.Format("{0:0000000000}{3}{1:00000}{3}{2}", new object[] { this.Field2, this.Field3, text, c });
		}
	}
}
