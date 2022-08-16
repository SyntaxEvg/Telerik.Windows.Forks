using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Trees;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader.Parsers;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader
{
	class PdfContentManager
	{
		public PdfContentManager(PdfFormatProvider provider, Stream stream)
			: this()
		{
			Guard.ThrowExceptionIfNull<PdfFormatProvider>(provider, "provider");
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.provider = provider;
			this.pdfParser = new PdfParser(this, stream);
			this.trailer = this.pdfParser.InitializeCrossReferences();
			this.encrypt = this.trailer.Encrypt;
			this.id = this.trailer.ID;
			if (this.encrypt != null)
			{
				Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption.Encrypt.ValidateEncryptionAlgorithmValue((int)this.encrypt.Algorithm.Value);
				if (this.id != null)
				{
					this.encrypt.Initialize(this.id, string.Empty);
				}
			}
		}

		internal PdfContentManager()
		{
			this.crossReferences = new CrossReferenceCollection();
			this.objectStreams = new Dictionary<int, ObjectStreamOld>();
			this.contentCache = new Dictionary<IndirectReferenceOld, IndirectObjectOld>();
			this.fieldsCache = new Dictionary<IndirectReferenceOld, FormField>();
			this.nodeToWidgets = new Dictionary<FormFieldNodeOld, List<WidgetOld>>();
			this.indirectReferencesCache = new Dictionary<IndirectReferenceOld, object>();
			this.pages = new List<PageOld>();
			this.ResourceManager = new PdfResourceManager(this);
			this.FontsManager = new FontsManagerOld();
		}

		public DocumentCatalogOld DocumentCatalog
		{
			get
			{
				return this.trailer.Root;
			}
		}

		public CrossReferenceCollection CrossReferences
		{
			get
			{
				return this.crossReferences;
			}
		}

		public PdfResourceManager ResourceManager { get; set; }

		public FontsManagerOld FontsManager { get; set; }

		EncryptOld Encrypt
		{
			get
			{
				return this.encrypt;
			}
		}

		public void RegisterIndirectReference(IndirectReferenceOld reference, object obj)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			this.indirectReferencesCache[reference] = obj;
		}

		public bool TryGetPdfObject(IndirectReferenceOld reference, out object obj)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			return this.indirectReferencesCache.TryGetValue(reference, out obj);
		}

		public void RegisterFormField(IndirectReferenceOld reference, FormField field)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			this.fieldsCache[reference] = field;
		}

		public bool TryGetFormField(IndirectReferenceOld reference, out FormField field)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			return this.fieldsCache.TryGetValue(reference, out field);
		}

		public IEnumerable<WidgetOld> GetChildWidgets(FormFieldNodeOld parent)
		{
			List<WidgetOld> childWidgets;
			if (this.nodeToWidgets.TryGetValue(parent, out childWidgets))
			{
				foreach (WidgetOld widget in childWidgets)
				{
					yield return widget;
				}
			}
			yield break;
		}

		public void AddWidgetParent(WidgetOld widgetObject, FormFieldNodeOld parent)
		{
			widgetObject.FormField = parent;
			List<WidgetOld> list;
			if (!this.nodeToWidgets.TryGetValue(parent, out list))
			{
				list = new List<WidgetOld>();
				this.nodeToWidgets.Add(parent, list);
			}
			list.Add(widgetObject);
		}

		public byte[] DecryptStream(IndirectObjectOld containingObject, byte[] inputData)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(containingObject, "containingObject");
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			if (this.Encrypt != null)
			{
				return this.Encrypt.DecryptStream(containingObject.ObjectNumber, containingObject.GenerationNumber, inputData);
			}
			return inputData;
		}

		public byte[] DecryptString(IndirectObjectOld containingObject, byte[] inputData)
		{
			Guard.ThrowExceptionIfNull<IndirectObjectOld>(containingObject, "containingObject");
			Guard.ThrowExceptionIfNull<byte[]>(inputData, "inputData");
			if (this.Encrypt != null)
			{
				return this.Encrypt.DecryptString(containingObject.ObjectNumber, containingObject.GenerationNumber, inputData);
			}
			return inputData;
		}

		public byte[] ReadData(IndirectReferenceOld reference)
		{
			return this.ReadData(reference, true);
		}

		public byte[] ReadData(IndirectReferenceOld reference, bool shouldDecode)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			byte[] result;
			lock (this.provider)
			{
				IndirectObjectOld indirectObjectOld;
				if (this.TryReadIndirectObject(reference, true, out indirectObjectOld))
				{
					PdfDataStream pdfDataStream = (PdfDataStream)indirectObjectOld.Value;
					if (shouldDecode)
					{
						result = pdfDataStream.ReadData(this);
					}
					else
					{
						result = pdfDataStream.ReadDataWithoutDecoding(this);
					}
				}
				else
				{
					result = new byte[0];
				}
			}
			return result;
		}

		public bool TryGetIndirectObject(IndirectReferenceOld reference, out IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			return this.TryGetIndirectObject(reference, true, out indirectObject);
		}

		public bool TryGetIndirectObject(IndirectReferenceOld reference, bool isEncrypted, out IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			if (!this.contentCache.TryGetValue(reference, out indirectObject))
			{
				if (!this.TryReadIndirectObject(reference, isEncrypted, out indirectObject))
				{
					return false;
				}
				if (!(indirectObject.Value is PdfDataStream))
				{
					this.contentCache[reference] = indirectObject;
				}
			}
			return true;
		}

		public virtual bool TryReadIndirectObject(IndirectReferenceOld fromRef, bool isEncrypted, out IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(fromRef, "fromRef");
			CrossReferenceEntryOld crossReferenceEntryOld;
			if (!this.CrossReferences.TryGetCrossReferenceEntry(fromRef.ObjectNumber, out crossReferenceEntryOld) || crossReferenceEntryOld.Type == CrossReferenceEntryTypeOld.Free)
			{
				indirectObject = null;
				return false;
			}
			if (this.TryReadIndirectObject(crossReferenceEntryOld, isEncrypted, out indirectObject))
			{
				indirectObject.GenerationNumber = fromRef.GenerationNumber;
				indirectObject.ObjectNumber = fromRef.ObjectNumber;
				return true;
			}
			return false;
		}

		public void LoadIndirectObject<T>(T obj) where T : PdfObjectOld
		{
			Guard.ThrowExceptionIfNull<T>(obj, "obj");
			if (obj.IsLoaded || obj.Reference == null)
			{
				return;
			}
			IndirectObjectOld indirectObject;
			if (this.TryGetIndirectObject(obj.Reference, out indirectObject))
			{
				obj.Load(indirectObject);
			}
		}

		public bool TryGetSimpleType<T>(IndirectReferenceOld reference, out T simpleType) where T : class
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(reference, "reference");
			simpleType = default(T);
			IndirectObjectOld indirectObjectOld;
			if (this.TryGetIndirectObject(reference, out indirectObjectOld))
			{
				simpleType = indirectObjectOld.Value as T;
			}
			return simpleType != null;
		}

		public IEnumerable<PageOld> GetPages()
		{
			Guard.ThrowExceptionIfNull<DocumentCatalogOld>(this.DocumentCatalog, "Catalog");
			Guard.ThrowExceptionIfNull<PageTreeNodeOld>(this.DocumentCatalog.Pages, "Pages");
			this.LoadPages(this.DocumentCatalog.Pages.Reference);
			return this.pages;
		}

		public bool TryReadIndirectObject(long offset, bool isEncrypted, out IndirectObjectOld indirectObject)
		{
			return this.pdfParser.TryReadIndirectObject(offset, isEncrypted, out indirectObject);
		}

		public global::Telerik.Windows.Documents.Fixed.Model.DigitalSignatures.ISourceStream GetSourceStream()
		{
			return new global::Telerik.Windows.Documents.Fixed.Model.DigitalSignatures.FileSourceStream(this.pdfParser.Reader.Stream);
		}

		void LoadPages(IndirectReferenceOld root)
		{
			this.LoadPages(root, null);
		}

		void LoadPages(IndirectReferenceOld root, PageTreeNodeOld parent)
		{
			Guard.ThrowExceptionIfNull<IndirectReferenceOld>(root, "root");
			IndirectObjectOld indirectObjectOld;
			if (!this.TryGetIndirectObject(root, out indirectObjectOld))
			{
				return;
			}
			PdfNameOld objectType = indirectObjectOld.GetObjectType();
			if (objectType == null)
			{
				return;
			}
			string value;
			if ((value = objectType.Value) != null)
			{
				if (!(value == "Pages"))
				{
					if (!(value == "Page"))
					{
						return;
					}
				}
				else
				{
					PageTreeNodeOld pageTreeNodeOld = new PageTreeNodeOld(this);
					pageTreeNodeOld.Load(indirectObjectOld);
					pageTreeNodeOld.Parent = parent;
					using (IEnumerator<object> enumerator = pageTreeNodeOld.Children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							IndirectReferenceOld root2 = (IndirectReferenceOld)obj;
							this.LoadPages(root2, pageTreeNodeOld);
						}
						return;
					}
				}
				PageOld pageOld = new PageOld(this);
				pageOld.Load(indirectObjectOld);
				pageOld.Parent = parent;
				this.pages.Add(pageOld);
			}
		}

		bool TryReadIndirectObject(CrossReferenceEntryOld entry, bool isEncrypted, out IndirectObjectOld indirectObject)
		{
			Guard.ThrowExceptionIfNull<CrossReferenceEntryOld>(entry, "entry");
			bool result;
			lock (this.provider)
			{
				switch (entry.Type)
				{
				case CrossReferenceEntryTypeOld.Used:
					result = this.TryReadIndirectObject(entry.Field2, isEncrypted, out indirectObject);
					break;
				case CrossReferenceEntryTypeOld.Compressed:
				{
					ObjectStreamOld objectStreamOld;
					if (this.TryGetObjectStream((int)entry.Field2, isEncrypted, out objectStreamOld))
					{
						indirectObject = objectStreamOld.ReadIndirectObject(entry.Field3);
						result = true;
					}
					else
					{
						indirectObject = null;
						result = false;
					}
					break;
				}
				default:
					indirectObject = null;
					result = false;
					break;
				}
			}
			return result;
		}

		bool TryGetObjectStream(int objNum, bool isEncrypted, out ObjectStreamOld objectStream)
		{
			if (!this.objectStreams.TryGetValue(objNum, out objectStream))
			{
				IndirectObjectOld indirectObject;
				if (!this.TryReadIndirectObject(this.CrossReferences[objNum], isEncrypted, out indirectObject))
				{
					return false;
				}
				objectStream = new ObjectStreamOld(this);
				objectStream.Load(indirectObject);
				this.objectStreams[objNum] = objectStream;
			}
			return true;
		}

		readonly PdfFormatProvider provider;

		readonly CrossReferenceCollection crossReferences;

		readonly Dictionary<int, ObjectStreamOld> objectStreams;

		readonly Dictionary<IndirectReferenceOld, object> indirectReferencesCache;

		readonly Dictionary<IndirectReferenceOld, IndirectObjectOld> contentCache;

		readonly Dictionary<IndirectReferenceOld, FormField> fieldsCache;

		readonly Dictionary<FormFieldNodeOld, List<WidgetOld>> nodeToWidgets;

		readonly List<PageOld> pages;

		readonly PdfParser pdfParser;

		readonly TrailerOld trailer;

		readonly EncryptOld encrypt;

		readonly PdfArrayOld id;
	}
}
