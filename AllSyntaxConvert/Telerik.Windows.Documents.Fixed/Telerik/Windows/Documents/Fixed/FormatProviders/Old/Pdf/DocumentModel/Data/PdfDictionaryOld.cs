using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfDictionaryOld : PdfCollectionBaseOld<string>
	{
		public PdfDictionaryOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.store = new Dictionary<string, object>();
		}

		public int Count
		{
			get
			{
				return this.store.Count;
			}
		}

		public IEnumerable<string> Keys
		{
			get
			{
				return this.store.Keys;
			}
		}

		public object this[string key]
		{
			get
			{
				return this.store[key];
			}
			set
			{
				this.store[key] = value;
			}
		}

		public void Add(string key, object value)
		{
			this.store.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return this.store.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return this.store.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return this.store.TryGetValue(key, out value);
		}

		public void Load(object[] content)
		{
			for (int i = 0; i < content.Length; i += 2)
			{
				this.store[(content[i] as PdfNameOld).Value] = content[i + 1];
			}
			base.IsLoaded = true;
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this.store.GetEnumerator();
		}

		public override void Load(IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(indirectObject, "indirectObject");
			if (indirectObject.Value is PdfDictionaryOld)
			{
				PdfDictionaryOld pdfDictionaryOld = indirectObject.Value as PdfDictionaryOld;
				this.store = pdfDictionaryOld.store;
			}
			base.Load(indirectObject);
		}

		protected override object GetElementAtIndex(string index)
		{
			return this.store[index];
		}

		protected override bool ContainsElementAtIndex(string index)
		{
			return this.store.ContainsKey(index);
		}

		Dictionary<string, object> store;
	}
}
