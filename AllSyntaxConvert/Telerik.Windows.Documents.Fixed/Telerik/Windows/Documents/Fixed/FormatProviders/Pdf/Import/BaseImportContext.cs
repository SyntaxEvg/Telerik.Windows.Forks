using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	abstract class BaseImportContext : IPdfImportContext
	{
		protected BaseImportContext(PdfImportSettings settings)
		{
			Guard.ThrowExceptionIfNull<PdfImportSettings>(settings, "settings");
			this.settings = settings;
			this.nodeToField = new Dictionary<FormFieldNode, FormField>();
			this.nodeToWidgets = new Dictionary<FormFieldNode, List<WidgetObject>>();
			this.widgetObjectToWidget = new Dictionary<WidgetObject, Widget>();
			this.crossReferences = new CrossReferenceCollection();
			this.indirectObjects = new Dictionary<IndirectReference, PdfPrimitive>();
			this.pagesToPdfDictionaries = new Dictionary<PageTreeNode, PdfDictionary>();
			this.indirectReferencesStack = new Stack<IndirectReference>();
			this.innerStreamCounter = new BeginEndCounter();
			this.pages = new List<Page>();
		}

		public CrossReferenceCollection CrossReferences
		{
			get
			{
				return this.crossReferences;
			}
		}

		public PdfImportSettings ImportSettings
		{
			get
			{
				return this.settings;
			}
		}

		public ReferenceProperty<DocumentCatalog> Root { get; set; }

		public byte[] DocumentId { get; set; }

		public Encrypt Encryption { get; set; }

		public List<Page> Pages
		{
			get
			{
				return this.pages;
			}
		}

		public IndirectReference CurrentIndirectReference
		{
			get
			{
				if (this.indirectReferencesStack.Count == 0)
				{
					return null;
				}
				return this.indirectReferencesStack.Peek();
			}
		}

		public int StartXRefOffset { get; set; }

		public bool StartsWithCrossReferenceTable { get; set; }

		protected bool IsReadingStreamInnerContent
		{
			get
			{
				return this.innerStreamCounter.IsUpdateInProgress;
			}
		}

		public PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		public void RegisterIndirectObject(IndirectReference reference, PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			this.indirectObjects[reference] = primitive;
		}

		public bool TryGetIndirectObject(IndirectReference reference, out PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			return this.indirectObjects.TryGetValue(reference, out primitive);
		}

		public IndirectObject ReadIndirectObject(IndirectReference reference)
		{
			Guard.ThrowExceptionIfNull<IndirectReference>(reference, "reference");
			IndirectObject result;
			using (this.reader.Reader.BeginReadingBlock())
			{
				this.indirectReferencesStack.Push(reference);
				CrossReferenceEntry crossReferenceEntry;
				IndirectObject indirectObject;
				if (this.CrossReferences.TryGetCrossReferenceEntry(reference, out crossReferenceEntry))
				{
					switch (crossReferenceEntry.Type)
					{
					case CrossReferenceEntryType.Used:
						indirectObject = this.ReadUsedIndirectObject(crossReferenceEntry);
						break;
					case CrossReferenceEntryType.Compressed:
						indirectObject = this.ReadCompressedIndirectObject(reference, crossReferenceEntry);
						break;
					default:
						indirectObject = this.ReadFreeIndirectObject(reference);
						break;
					}
					if (indirectObject.Reference != reference)
					{
						indirectObject = this.ReadFreeIndirectObject(reference);
					}
				}
				else
				{
					indirectObject = this.ReadFreeIndirectObject(reference);
				}
				this.indirectReferencesStack.Pop();
				result = indirectObject;
			}
			return result;
		}

		public void MapWidgets(WidgetObject widgetObject, Widget widget)
		{
			this.widgetObjectToWidget[widgetObject] = widget;
		}

		public bool TryGetWidget(WidgetObject widgetObject, out Widget widget)
		{
			return this.widgetObjectToWidget.TryGetValue(widgetObject, out widget);
		}

		public void MapFields(FormFieldNode node, FormField field)
		{
			this.nodeToField[node] = field;
		}

		public bool TryGetField(FormFieldNode node, out FormField field)
		{
			return this.nodeToField.TryGetValue(node, out field);
		}

		public void AddWidgetParent(WidgetObject widgetObject, FormFieldNode parent)
		{
			widgetObject.FormField = parent;
			List<WidgetObject> list;
			if (!this.nodeToWidgets.TryGetValue(parent, out list))
			{
				list = new List<WidgetObject>();
				this.nodeToWidgets.Add(parent, list);
			}
			list.Add(widgetObject);
		}

		public IEnumerable<WidgetObject> GetChildWidgets(FormFieldNode parent)
		{
			List<WidgetObject> childWidgets;
			if (this.nodeToWidgets.TryGetValue(parent, out childWidgets))
			{
				foreach (WidgetObject widget in childWidgets)
				{
					yield return widget;
				}
			}
			yield break;
		}

		public void BeginImport(Stream pdfFileStream)
		{
			Guard.ThrowExceptionIfNull<Stream>(pdfFileStream, "pdfFileStream");
			Guard.ThrowExceptionIfNotNull<PostScriptReader>(this.reader, "this.reader");
			PdfImporter.ValidateInputStreamCanReadAndSeek(pdfFileStream);
			if (!PdfImporter.SeekToStartXRef(pdfFileStream))
			{
				throw new NotSupportedException("StartXRef keyword cannot be found.");
			}
			this.reader = new PostScriptReader(pdfFileStream, new KeywordCollection());
			this.PrepareCrossReferencesForImport();
			this.BeginImportOverride();
		}

		public IDisposable BeginImportOfStreamInnerContent()
		{
			return this.innerStreamCounter.Begin();
		}

		public byte[] DecryptStream(IndirectReference reference, byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			if (!this.ShouldDecryptStream(reference))
			{
				return data;
			}
			return this.Encryption.DecryptStream(reference.ObjectNumber, reference.GenerationNumber, data);
		}

		public byte[] DecryptString(byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			if (!this.ShouldDecryptString())
			{
				return data;
			}
			return this.Encryption.DecryptString(this.CurrentIndirectReference.ObjectNumber, this.CurrentIndirectReference.GenerationNumber, data);
		}

		public bool TryGetNestedPrimitive<T>(PdfPrimitive primitive, IEnumerable<string> dictionaryKeys, out T result) where T : PdfPrimitive
		{
			PdfPrimitive pdfPrimitive = primitive;
			if (pdfPrimitive.Type == PdfElementType.IndirectReference)
			{
				IndirectReference reference = (IndirectReference)pdfPrimitive;
				IndirectObject indirectObject = this.ReadIndirectObject(reference);
				pdfPrimitive = indirectObject.Content;
			}
			foreach (string text in dictionaryKeys)
			{
				if (pdfPrimitive.Type != PdfElementType.Dictionary)
				{
					result = default(T);
					return false;
				}
				PdfDictionary pdfDictionary = (PdfDictionary)pdfPrimitive;
				if (!pdfDictionary.ContainsKey(text))
				{
					result = default(T);
					return false;
				}
				pdfPrimitive = pdfDictionary[text];
				if (pdfPrimitive.Type == PdfElementType.IndirectReference)
				{
					IndirectReference reference2 = (IndirectReference)pdfPrimitive;
					IndirectObject indirectObject2 = this.ReadIndirectObject(reference2);
					pdfPrimitive = indirectObject2.Content;
				}
			}
			result = pdfPrimitive as T;
			return result != null;
		}

		public void MapPageToPdfDictionary(PageTreeNode page, PdfDictionary dictionary)
		{
			this.pagesToPdfDictionaries[page] = dictionary;
		}

		public void UnmapPdfDictionary(PageTreeNode page)
		{
			this.pagesToPdfDictionaries.Remove(page);
		}

		public bool TryGetPdfDictionary(PageTreeNode page, out PdfDictionary dictionary)
		{
			return this.pagesToPdfDictionaries.TryGetValue(page, out dictionary);
		}

		protected abstract void BeginImportOverride();

		internal static int ReadStartXRefOffset(PostScriptReader reader, IPdfImportContext context)
		{
			reader.Reader.ReadLine();
			PdfInt pdfInt = reader.Read<PdfInt>(context, PdfElementType.PdfInt);
			if (pdfInt == null)
			{
				throw new NotSupportedException("Offset to XRef cannot be found.");
			}
			return pdfInt.Value;
		}

		void PrepareCrossReferencesForImport()
		{
			this.StartXRefOffset = BaseImportContext.ReadStartXRefOffset(this.Reader, this);
			this.Reader.Reader.Seek((long)this.StartXRefOffset, SeekOrigin.Begin);
			this.StartsWithCrossReferenceTable = this.CrossReferences.PeekCrossReferenceTableMarker(this.Reader, this);
			this.CrossReferences.Read(this.Reader, this);
			if (this.Encryption != null)
			{
				Encrypt.ValidateEncryptionAlgorithmValue(this.Encryption.Algorithm.Value);
				if (!this.Encryption.AuthenticateUserPassword(this.DocumentId, this.settings.GetUserPassword()))
				{
					throw new InvalidOperationException("Password is not correct.");
				}
			}
		}

		bool ShouldDecryptStream(IndirectReference reference)
		{
			return !this.IsReadingStreamInnerContent && this.Encryption != null && reference != null;
		}

		bool ShouldDecryptString()
		{
			return !this.IsReadingStreamInnerContent && this.Encryption != null && this.CurrentIndirectReference != null;
		}

		IndirectObject ReadCompressedIndirectObject(IndirectReference reference, CrossReferenceEntry entry)
		{
			int objectNumber = (int)entry.Field1;
			IndirectReference value = new IndirectReference(objectNumber, 0);
			PdfObjectDescriptor pdfObjectDescriptor = PdfObjectDescriptors.GetPdfObjectDescriptor<ObjectStream>();
			ObjectStream objectStream = (ObjectStream)pdfObjectDescriptor.Converter.Convert(typeof(ObjectStream), this.reader, this, value);
			PdfPrimitive content = objectStream.ReadObjectContent(entry, this);
			return new IndirectObject(reference, content);
		}

		IndirectObject ReadUsedIndirectObject(CrossReferenceEntry entry)
		{
			long field = entry.Field1;
			this.reader.Reader.Seek(field, SeekOrigin.Begin);
			return this.reader.Read<IndirectObject>(this, PdfElementType.IndirectObject);
		}

		IndirectObject ReadFreeIndirectObject(IndirectReference reference)
		{
			return new IndirectObject(reference, PdfNull.Instance);
		}

		readonly Dictionary<FormFieldNode, FormField> nodeToField;

		readonly Dictionary<FormFieldNode, List<WidgetObject>> nodeToWidgets;

		readonly Dictionary<WidgetObject, Widget> widgetObjectToWidget;

		readonly Dictionary<IndirectReference, PdfPrimitive> indirectObjects;

		readonly Dictionary<PageTreeNode, PdfDictionary> pagesToPdfDictionaries;

		readonly Stack<IndirectReference> indirectReferencesStack;

		readonly CrossReferenceCollection crossReferences;

		readonly BeginEndCounter innerStreamCounter;

		readonly PdfImportSettings settings;

		readonly List<Page> pages;

		PostScriptReader reader;
	}
}
