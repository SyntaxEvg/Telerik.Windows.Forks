using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	class PdfDictionary : PdfCollectionBase<string>, IEnumerable<KeyValuePair<string, PdfPrimitive>>, IEnumerable
	{
		public PdfDictionary()
		{
			this.store = new Dictionary<string, PdfPrimitive>();
		}

		public override PdfElementType Type
		{
			get
			{
				return PdfElementType.Dictionary;
			}
		}

		public override int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public override PdfPrimitive this[string index]
		{
			get
			{
				return this.store[index];
			}
			set
			{
				this.store[index] = value;
			}
		}

		public IEnumerable<string> Keys
		{
			get
			{
				return this.store.Keys;
			}
		}

		public bool Remove(string key)
		{
			return this.store.Remove(key);
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			writer.Write(PdfNames.PdfDictionaryStart);
			foreach (KeyValuePair<string, PdfPrimitive> keyValuePair in this.store)
			{
				writer.WritePdfName(keyValuePair.Key);
				writer.WriteSeparator();
				PdfPrimitive value = keyValuePair.Value;
				if (value.Type == PdfElementType.PdfStreamObject)
				{
					PdfPrimitive.WriteIndirectReference(writer, context, value);
				}
				else
				{
					value.Write(writer, context);
				}
				writer.WriteSeparator();
			}
			writer.Write(PdfNames.PdfDictionaryEnd);
		}

		public bool ContainsKey(string key)
		{
			Guard.ThrowExceptionIfNullOrEmpty(key, "key");
			return this.store.ContainsKey(key);
		}

		public IEnumerator<KeyValuePair<string, PdfPrimitive>> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		protected override bool TryGetElementOverride(string index, out PdfPrimitive element)
		{
			return this.store.TryGetValue(index, out element);
		}

		readonly Dictionary<string, PdfPrimitive> store;
	}
}
