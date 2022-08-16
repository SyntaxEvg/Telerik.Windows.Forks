using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class CrossReferenceEntry
	{
		public CrossReferenceEntry()
		{
		}

		public CrossReferenceEntry(long field1, int field2, CrossReferenceEntryType type)
		{
			this.Field1 = field1;
			this.Field2 = field2;
			this.Type = type;
		}

		public int Field2 { get; set; }

		public long Field1 { get; set; }

		public CrossReferenceEntryType Type { get; set; }

		public override string ToString()
		{
			string text = ((this.Type == CrossReferenceEntryType.Used) ? "n" : ((this.Type == CrossReferenceEntryType.Compressed) ? "c" : "f"));
			char c = ' ';
			return string.Format("{0:0000000000}{3}{1:00000}{3}{2}", new object[] { this.Field1, this.Field2, text, c });
		}

		public string ToCrossReferenceTableString()
		{
			string text = ((this.Type == CrossReferenceEntryType.Free) ? "f" : "n");
			char c = ' ';
			return string.Format("{0:0000000000}{3}{1:00000}{3}{2}", new object[] { this.Field1, this.Field2, text, c });
		}

		public void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.WriteLine(this.ToCrossReferenceTableString());
		}

		public void Read(PostScriptReader reader)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			string[] array;
			do
			{
				array = reader.Reader.ReadSplitLine();
			}
			while (array.Length == 0);
			long field;
			long.TryParse(array[0], out field);
			int field2;
			int.TryParse(array[1], out field2);
			this.Field1 = field;
			this.Field2 = field2;
			this.Type = ((array[2] == "f") ? CrossReferenceEntryType.Free : CrossReferenceEntryType.Used);
		}
	}
}
