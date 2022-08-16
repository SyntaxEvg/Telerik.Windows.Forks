using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.GraphicsState;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class PdfExportContext : IPdfExportContext
	{
		public PdfExportContext(RadFixedDocumentInfo documentInfo, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
			this.settings = settings;
			this.documentInfo = documentInfo;
			this.documentId = PdfExportContext.CreateDocumentId();
			this.registeredIndirectObjects = new InstanceIdCache<PdfPrimitive, IndirectObject>();
			this.fontResources = new Dictionary<FontBase, ResourceEntry>();
			this.formResources = new InstanceIdCache<FormSource, ResourceEntry>();
			this.imageResources = new InstanceIdCache<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource, ResourceEntry>();
			this.patternResources = new InstanceIdCache<PatternColor, ResourceEntry>();
			this.uncoloredTilingResources = new InstanceIdCache<PatternColor, ResourceEntry>();
			this.extGStateResources = new InstanceIdCache<ExtGState, ResourceEntry>();
			this.colorspaceResources = new Dictionary<string, ResourceEntry>();
			this.fontResourcesCount = 1;
			this.formXObjectResourcesCount = 1;
			this.imageResourcesCount = 1;
			this.patternResourcesCount = 1;
			this.graphicResourcesCount = 1;
			this.colorResourcesCount = 1;
			this.CanWriteColors = true;
			this.indirectObjects = new Queue<IndirectObject>();
			this.usedCharacters = new Dictionary<FontBase, HashSet<CharInfo>>();
			this.crossReferenceCollection = new CrossReferenceCollection();
			this.crossReferenceCollection.AddCrossReferenceEntry(0, new CrossReferenceEntry(0L, 65535, CrossReferenceEntryType.Free));
			this.contentRootHeightToDipToPdfPointTransformations = new Dictionary<double, Matrix>();
			this.fixedPageToPage = new InstanceIdCache<RadFixedPage, Page>();
			this.widgetModelToWidgetObject = new InstanceIdCache<Widget, WidgetObject>();
			this.fieldToNode = new InstanceIdCache<FormField, FormFieldNode>();
			this.signatureExportInfo = new SignatureExportInfo();
		}

		public bool CanWriteColors { get; set; }

		public Encrypt Encryption { get; set; }

		public PdfExportSettings Settings
		{
			get
			{
				return this.settings;
			}
		}

		public byte[] DocumentId
		{
			get
			{
				return this.documentId;
			}
		}

		public IEnumerable<FontBase> FontResources
		{
			get
			{
				return this.fontResources.Keys;
			}
		}

		public Queue<IndirectObject> IndirectObjectsQueue
		{
			get
			{
				return this.indirectObjects;
			}
		}

		public CrossReferenceCollection CrossReferenceCollection
		{
			get
			{
				return this.crossReferenceCollection;
			}
		}

		public RadFixedDocumentInfo DocumentInfo
		{
			get
			{
				return this.documentInfo;
			}
		}

		public IPdfContentExportContext AcroFormContentExportContext { get; set; }

		public SignatureExportInfo SignatureExportInfo
		{
			get
			{
				return this.signatureExportInfo;
			}
		}

		public IndirectObject CreateIndirectObject(PdfPrimitive primitive)
		{
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			return this.CreateIndirectObject(primitive, true);
		}

		public void BeginExportIndirectObject(IndirectObject indirectObject, long offset)
		{
			Guard.ThrowExceptionIfNull<IndirectObject>(indirectObject, "indirectObject");
			Guard.ThrowExceptionIfNotNull<IndirectObject>(this.currentIndirectObject, "currentIndirectObject");
			this.currentIndirectObject = indirectObject;
			CrossReferenceEntryType type = ((indirectObject.Content.Type == PdfElementType.Null) ? CrossReferenceEntryType.Free : CrossReferenceEntryType.Used);
			this.crossReferenceCollection.UpdateCrossReferenceEntry(indirectObject.Reference.ObjectNumber, offset, 0, type);
		}

		public void EndExportIndirectObject()
		{
			this.currentIndirectObject.ReleaseContent();
			this.currentIndirectObject = null;
		}

		public FormFieldNode CreateFormFieldObject(FormField field, bool includeInIndirectObjectQueue)
		{
			FormFieldNode formFieldNode;
			if (!this.fieldToNode.TryGetValue(field, out formFieldNode))
			{
				formFieldNode = new FormFieldNode();
				this.fieldToNode.Add(field, formFieldNode);
			}
			IndirectObject item = this.CreateIndirectObject(formFieldNode, false);
			if (includeInIndirectObjectQueue)
			{
				this.IndirectObjectsQueue.Enqueue(item);
			}
			return formFieldNode;
		}

		public ResourceEntry GetResource(FontBase resource)
		{
			ResourceEntry resourceEntry;
			if (!this.fontResources.TryGetValue(resource, out resourceEntry))
			{
				FontObject primitive = PdfFontsFactory.CreateFont(resource.Type);
				resourceEntry = this.CreateResourceEntry(primitive, "F", false);
				this.fontResources[resource] = resourceEntry;
			}
			return resourceEntry;
		}

		public ResourceEntry GetResource(FormSource resource)
		{
			ResourceEntry resourceEntry;
			if (!this.formResources.TryGetValue(resource, out resourceEntry))
			{
				FormXObject formXObject = new FormXObject();
				formXObject.CopyPropertiesFrom(this, resource);
				if (!resource.Matrix.IsIdentity)
				{
					formXObject.Matrix = resource.Matrix.ToPdfArray();
				}
				resourceEntry = this.CreateResourceEntry(formXObject, "X", true);
				this.formResources[resource] = resourceEntry;
			}
			return resourceEntry;
		}

		public ResourceEntry GetResource(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource resource)
		{
			ResourceEntry resourceEntry;
			if (!this.imageResources.TryGetValue(resource, out resourceEntry))
			{
				ImageXObject imageXObject = new ImageXObject();
				imageXObject.CopyPropertiesFrom(this, resource);
				resourceEntry = this.CreateResourceEntry(imageXObject, "I", true);
				this.imageResources[resource] = resourceEntry;
			}
			return resourceEntry;
		}

		public ResourceEntry GetResource(ExtGState resource)
		{
			ResourceEntry resourceEntry;
			if (!this.extGStateResources.TryGetValue(resource, out resourceEntry))
			{
				ExtGStateObject extGStateObject = new ExtGStateObject();
				extGStateObject.CopyPropertiesFrom(this, resource);
				resourceEntry = this.CreateResourceEntry(extGStateObject, "G", true);
				this.extGStateResources[resource] = resourceEntry;
			}
			return resourceEntry;
		}

		public ResourceEntry GetResource(ColorSpaceBase first, ColorSpaceBase second)
		{
			Guard.ThrowExceptionIfNull<ColorSpaceBase>(first, "first");
			Guard.ThrowExceptionIfNull<ColorSpaceBase>(second, "second");
			string key = string.Format("{0}{1}", first.Name, second.Name);
			ResourceEntry resourceEntry;
			if (!this.colorspaceResources.TryGetValue(key, out resourceEntry))
			{
				PdfArray primitive = new PdfArray(new PdfPrimitive[0])
				{
					new PdfName(first.Name),
					new PdfName(second.Name)
				};
				resourceEntry = this.CreateResourceEntry(primitive, "C", true);
				this.colorspaceResources[key] = resourceEntry;
			}
			return resourceEntry;
		}

		public ResourceEntry GetResource(PatternColor resource, IPdfContentExportContext context)
		{
			Guard.ThrowExceptionIfNull<PatternColor>(resource, "resource");
			Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, "context");
			PatternColor key = resource;
			InstanceIdCache<PatternColor, ResourceEntry> instanceIdCache = this.patternResources;
			if (resource.PatternType == PatternType.Tiling)
			{
				UncoloredTiling uncoloredTiling = resource as UncoloredTiling;
				if (uncoloredTiling != null)
				{
					key = uncoloredTiling.Tiling;
					instanceIdCache = this.uncoloredTilingResources;
				}
			}
			ResourceEntry resourceEntry;
			if (!instanceIdCache.TryGetValue(key, out resourceEntry))
			{
				PatternColorObject patternColorObject = PatternColorObject.CreateInstance((int)resource.PatternType);
				patternColorObject.CopyPropertiesFrom(context, resource);
				resourceEntry = this.CreateResourceEntry(patternColorObject, "P", true);
				instanceIdCache[key] = resourceEntry;
			}
			return resourceEntry;
		}

		public byte[] EncryptStream(byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			if (this.Encryption == null || this.currentIndirectObject == null || this.Encryption == this.currentIndirectObject.Content)
			{
				return data;
			}
			return this.Encryption.EncryptStream(this.currentIndirectObject.Reference.ObjectNumber, this.currentIndirectObject.Reference.GenerationNumber, data);
		}

		public byte[] EncryptString(byte[] data)
		{
			Guard.ThrowExceptionIfNull<byte[]>(data, "data");
			if (this.Encryption == null || this.currentIndirectObject == null || this.Encryption == this.currentIndirectObject.Content)
			{
				return data;
			}
			return this.Encryption.EncryptString(this.currentIndirectObject.Reference.ObjectNumber, this.currentIndirectObject.Reference.GenerationNumber, data);
		}

		public IPdfContentExportContext CreateContentExportContext(IResourceHolder resourceHolder, IContentRootElement contentRoot)
		{
			Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, "resourceHolder");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, "contentRoot");
			return new PdfContentExportContext(this, resourceHolder, contentRoot);
		}

		public void SetUsedCharacters(FontBase font, TextCollection glyphs)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			Guard.ThrowExceptionIfNull<TextCollection>(glyphs, "glyphs");
			HashSet<CharInfo> hashSet;
			if (!this.usedCharacters.TryGetValue(font, out hashSet))
			{
				hashSet = new HashSet<CharInfo>();
				this.usedCharacters[font] = hashSet;
			}
			foreach (CharInfo item in glyphs.Characters)
			{
				hashSet.Add(item);
			}
		}

		public IEnumerable<CharInfo> GetUsedCharacters(FontBase font)
		{
			Guard.ThrowExceptionIfNull<FontBase>(font, "font");
			HashSet<CharInfo> result;
			if (!this.usedCharacters.TryGetValue(font, out result))
			{
				return Enumerable.Empty<CharInfo>();
			}
			return result;
		}

		public Matrix GetDipToPdfPointTransformation(IContentRootElement contentRoot)
		{
			Matrix matrix;
			if (!this.contentRootHeightToDipToPdfPointTransformations.TryGetValue(contentRoot.Size.Height, out matrix))
			{
				matrix = PdfExportContext.CalculateDipToPdfPointTransformation(contentRoot);
				this.contentRootHeightToDipToPdfPointTransformations.Add(contentRoot.Size.Height, matrix);
			}
			return matrix;
		}

		public static Matrix CalculateDipToPdfPointTransformation(IContentRootElement contentRoot)
		{
			return new Matrix(PdfExportContext.dipToPoint, 0.0, 0.0, -PdfExportContext.dipToPoint, 0.0, Unit.DipToPoint(contentRoot.Size.Height));
		}

		public void MapPages(RadFixedPage fixedPage, Page page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			Guard.ThrowExceptionIfNull<Page>(page, "page");
			this.fixedPageToPage[fixedPage] = page;
		}

		public bool TryGetPage(RadFixedPage fixedPage, out Page page)
		{
			Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, "fixedPage");
			return this.fixedPageToPage.TryGetValue(fixedPage, out page);
		}

		public void MapWidgets(Widget widgetModel, WidgetObject widgetObject)
		{
			Guard.ThrowExceptionIfNull<Widget>(widgetModel, "widgetModel");
			Guard.ThrowExceptionIfNull<WidgetObject>(widgetObject, "widgetObject");
			this.widgetModelToWidgetObject[widgetModel] = widgetObject;
		}

		public bool TryGetWidget(Widget widgetModel, out WidgetObject widgetObject)
		{
			Guard.ThrowExceptionIfNull<Widget>(widgetModel, "widgetModel");
			return this.widgetModelToWidgetObject.TryGetValue(widgetModel, out widgetObject);
		}

		protected IndirectObject CreateIndirectObject(PdfPrimitive primitive, bool includeInIndirectObjectsQueue)
		{
			int nextObjectNumber = this.GetNextObjectNumber();
			return this.CreateIndirectObject(primitive, nextObjectNumber, includeInIndirectObjectsQueue, true);
		}

		protected IndirectObject CreateIndirectObject(PdfPrimitive primitive, int objectNumber, bool includeInIndirectObjectsQueue, bool addToCrossReferences)
		{
			Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, "primitive");
			IndirectObject indirectObject;
			if (!this.registeredIndirectObjects.TryGetValue(primitive, out indirectObject))
			{
				IndirectReference reference = new IndirectReference(objectNumber, 0);
				indirectObject = new IndirectObject(reference, primitive);
				this.registeredIndirectObjects[primitive] = indirectObject;
				if (includeInIndirectObjectsQueue)
				{
					this.IndirectObjectsQueue.Enqueue(indirectObject);
				}
				if (addToCrossReferences)
				{
					this.crossReferenceCollection.AddCrossReferenceEntry(objectNumber, new CrossReferenceEntry(0L, 0, CrossReferenceEntryType.Free));
				}
			}
			return indirectObject;
		}

		protected virtual int GetNextObjectNumber()
		{
			return this.CrossReferenceCollection.MaxObjectNumber + 1;
		}

		ResourceEntry CreateResourceEntry(PdfPrimitive primitive, string keyPrefix, bool includeInQueue = false)
		{
			IndirectObject resource = this.CreateIndirectObject(primitive, includeInQueue);
			string nextResourceKey = this.GetNextResourceKey(keyPrefix);
			return new ResourceEntry(nextResourceKey, resource);
		}

		string GetNextResourceKey(string keyPrefix)
		{
			switch (keyPrefix)
			{
			case "F":
				return string.Format("{0}{1}", keyPrefix, this.fontResourcesCount++);
			case "X":
				return string.Format("{0}{1}", keyPrefix, this.formXObjectResourcesCount++);
			case "I":
				return string.Format("{0}{1}", keyPrefix, this.imageResourcesCount++);
			case "P":
				return string.Format("{0}{1}", keyPrefix, this.patternResourcesCount++);
			case "G":
				return string.Format("{0}{1}", keyPrefix, this.graphicResourcesCount++);
			case "C":
				return string.Format("{0}{1}", keyPrefix, this.colorResourcesCount++);
			}
			throw new ArgumentException("Invalid prefix argument: {0}", keyPrefix);
		}

		static byte[] CreateDocumentId()
		{
			string s = DateTime.Now.ToString("o", CultureInfo.InvariantCulture);
			return MD5Core.GetHash(Encoding.UTF8.GetBytes(s));
		}

		const string FontResourcePrefix = "F";

		const string FormXObjectResourcePrefix = "X";

		const string ImageResourcePrefix = "I";

		const string PatternResourcePrefix = "P";

		const string GraphicResourcePrefix = "G";

		const string ColorResourcePrefix = "C";

		const int ResourcesInitialIndex = 1;

		static readonly double dipToPoint = Unit.DipToPoint(1.0);

		readonly CrossReferenceCollection crossReferenceCollection;

		readonly Queue<IndirectObject> indirectObjects;

		readonly InstanceIdCache<PdfPrimitive, IndirectObject> registeredIndirectObjects;

		readonly Dictionary<FontBase, ResourceEntry> fontResources;

		readonly InstanceIdCache<FormSource, ResourceEntry> formResources;

		readonly InstanceIdCache<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource, ResourceEntry> imageResources;

		readonly InstanceIdCache<PatternColor, ResourceEntry> patternResources;

		readonly InstanceIdCache<PatternColor, ResourceEntry> uncoloredTilingResources;

		readonly InstanceIdCache<ExtGState, ResourceEntry> extGStateResources;

		readonly Dictionary<string, ResourceEntry> colorspaceResources;

		readonly Dictionary<FontBase, HashSet<CharInfo>> usedCharacters;

		readonly Dictionary<double, Matrix> contentRootHeightToDipToPdfPointTransformations;

		readonly InstanceIdCache<RadFixedPage, Page> fixedPageToPage;

		readonly InstanceIdCache<Widget, WidgetObject> widgetModelToWidgetObject;

		readonly InstanceIdCache<FormField, FormFieldNode> fieldToNode;

		readonly byte[] documentId;

		readonly PdfExportSettings settings;

		readonly RadFixedDocumentInfo documentInfo;

		int fontResourcesCount;

		int formXObjectResourcesCount;

		int imageResourcesCount;

		int patternResourcesCount;

		int graphicResourcesCount;

		int colorResourcesCount;

		IndirectObject currentIndirectObject;

		SignatureExportInfo signatureExportInfo;
	}
}
