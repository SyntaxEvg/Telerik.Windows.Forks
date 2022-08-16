using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class CrossReferenceCollection
	{
		public CrossReferenceCollection()
		{
			this.crossReferenceEntries = new Dictionary<int, CrossReferenceEntry>();
			this.MaxObjectNumber = int.MinValue;
		}

		public int Count
		{
			get
			{
				return this.crossReferenceEntries.Count;
			}
		}

		public int MaxObjectNumber { get; set; }

		internal IEnumerable<KeyValuePair<int, CrossReferenceEntry>> Entries
		{
			get
			{
				return this.crossReferenceEntries;
			}
		}

		public void AddCrossReferenceEntry(int objectNumber, CrossReferenceEntry entry)
		{
			Guard.ThrowExceptionIfNull<CrossReferenceEntry>(entry, "entry");
			if (!this.crossReferenceEntries.ContainsKey(objectNumber))
			{
				this.crossReferenceEntries[objectNumber] = entry;
				this.MaxObjectNumber = Math.Max(this.MaxObjectNumber, objectNumber);
			}
		}

		public CrossReferenceEntry GetCrossReferenceEntry(int objectNumber)
		{
			return this.crossReferenceEntries[objectNumber];
		}

		public void UpdateCrossReferenceEntry(int objectNumber, long field1, int field2, CrossReferenceEntryType type)
		{
			CrossReferenceEntry crossReferenceEntry = this.crossReferenceEntries[objectNumber];
			crossReferenceEntry.Field1 = field1;
			crossReferenceEntry.Field2 = field2;
			crossReferenceEntry.Type = type;
		}

		public bool TryGetCrossReferenceEntry(IndirectReference reference, out CrossReferenceEntry entry)
		{
			return this.crossReferenceEntries.TryGetValue(reference.ObjectNumber, out entry);
		}

		public void Read(PostScriptReader reader, IPdfImportContext context)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			bool flag = this.PeekCrossReferenceTableMarker(reader, context);
			if (flag)
			{
				reader.Reader.ReadLine();
				this.ReadCrossReferenceTable(reader, context);
				return;
			}
			this.ReadCrossReferenceStream(reader, context);
		}

		internal bool PeekCrossReferenceTableMarker(PostScriptReader reader, IPdfImportContext context)
		{
			bool result;
			using (reader.Reader.BeginReadingBlock())
			{
				string a = reader.Reader.ReadLine().Trim();
				bool flag = a == "xref";
				result = flag;
			}
			return result;
		}

		void ReadCrossReferenceTable(PostScriptReader reader, IPdfImportContext context)
		{
			bool flag = true;
			string[] array = reader.Reader.ReadSplitLine();
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			while (flag)
			{
				for (int i = 0; i < num2; i++)
				{
					CrossReferenceEntry crossReferenceEntry = new CrossReferenceEntry();
					crossReferenceEntry.Read(reader);
					this.AddCrossReferenceEntry(num + i, crossReferenceEntry);
				}
				using (reader.Reader.BeginReadingBlock())
				{
					array = reader.Reader.ReadSplitLine();
					flag = array.Length >= 2 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2);
				}
				if (flag)
				{
					reader.Reader.ReadLine();
				}
			}
			Trailer trailer = new Trailer();
			trailer.Read(reader, context);
			this.InterpretTrailer(trailer, reader, context);
		}

		void ReadCrossReferenceStream(PostScriptReader reader, IPdfImportContext context)
		{
			IndirectObject indirectObject = reader.Read<IndirectObject>(context, PdfElementType.IndirectObject);
			CrossReferenceStream crossReferenceStream = new CrossReferenceStream();
			crossReferenceStream.Load(reader, context, indirectObject.Content);
			this.InterpretTrailer(crossReferenceStream, reader, context);
		}

		void InterpretTrailer(Trailer trailer, PostScriptReader reader, IPdfImportContext context)
		{
			trailer.CopyPropertiesTo(reader, context);
			if (trailer.XRefStm != null)
			{
				reader.Reader.Seek((long)trailer.XRefStm.Value, SeekOrigin.Begin);
				this.Read(reader, context);
			}
			if (trailer.Prev != null)
			{
				reader.Reader.Seek((long)trailer.Prev.Value, SeekOrigin.Begin);
				this.Read(reader, context);
			}
		}

		readonly Dictionary<int, CrossReferenceEntry> crossReferenceEntries;
	}
}
