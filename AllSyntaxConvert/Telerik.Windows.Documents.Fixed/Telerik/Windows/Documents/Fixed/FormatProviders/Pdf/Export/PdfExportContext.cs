// Decompiled with JetBrains decompiler
// Type: Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export.PdfExportContext
// Assembly: Telerik.Windows.Documents.Fixed, Version=2019.2.503.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// MVID: 6454440F-C021-4D95-99F0-3BFB2485AF25
// Assembly location: C:\Users\user\Downloads\FromFileToFileCore\FromFileToFileCore\Export_Word_Excel_PDF_CSV_HTML-master\ExportDemo1\ConsoleApp1\bin\x64\Debug\Telerik.Windows.Documents.Fixed.dll

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
    internal class PdfExportContext : IPdfExportContext
    {
        private const string FontResourcePrefix = "F";
        private const string FormXObjectResourcePrefix = "X";
        private const string ImageResourcePrefix = "I";
        private const string PatternResourcePrefix = "P";
        private const string GraphicResourcePrefix = "G";
        private const string ColorResourcePrefix = "C";
        private const int ResourcesInitialIndex = 1;
        private static readonly double dipToPoint = Unit.DipToPoint(1.0);
        private readonly CrossReferenceCollection crossReferenceCollection;
        private readonly Queue<IndirectObject> indirectObjects;
        private readonly InstanceIdCache<PdfPrimitive, IndirectObject> registeredIndirectObjects;
        private readonly Dictionary<FontBase, ResourceEntry> fontResources;
        private readonly InstanceIdCache<FormSource, ResourceEntry> formResources;
        private readonly InstanceIdCache<Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource, ResourceEntry> imageResources;
        private readonly InstanceIdCache<PatternColor, ResourceEntry> patternResources;
        private readonly InstanceIdCache<PatternColor, ResourceEntry> uncoloredTilingResources;
        private readonly InstanceIdCache<ExtGState, ResourceEntry> extGStateResources;
        private readonly Dictionary<string, ResourceEntry> colorspaceResources;
        private readonly Dictionary<FontBase, HashSet<CharInfo>> usedCharacters;
        private readonly Dictionary<double, Matrix> contentRootHeightToDipToPdfPointTransformations;
        private readonly InstanceIdCache<RadFixedPage, Page> fixedPageToPage;
        private readonly InstanceIdCache<Widget, WidgetObject> widgetModelToWidgetObject;
        private readonly InstanceIdCache<FormField, FormFieldNode> fieldToNode;
        private readonly byte[] documentId;
        private readonly PdfExportSettings settings;
        private readonly RadFixedDocumentInfo documentInfo;
        private int fontResourcesCount;
        private int formXObjectResourcesCount;
        private int imageResourcesCount;
        private int patternResourcesCount;
        private int graphicResourcesCount;
        private int colorResourcesCount;
        private IndirectObject currentIndirectObject;
        private SignatureExportInfo signatureExportInfo;

        public PdfExportContext(RadFixedDocumentInfo documentInfo, PdfExportSettings settings)
        {
            Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, nameof(settings));
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
            this.crossReferenceCollection.AddCrossReferenceEntry(0, new CrossReferenceEntry(0L, (int)ushort.MaxValue, CrossReferenceEntryType.Free));
            this.contentRootHeightToDipToPdfPointTransformations = new Dictionary<double, Matrix>();
            this.fixedPageToPage = new InstanceIdCache<RadFixedPage, Page>();
            this.widgetModelToWidgetObject = new InstanceIdCache<Widget, WidgetObject>();
            this.fieldToNode = new InstanceIdCache<FormField, FormFieldNode>();
            this.signatureExportInfo = new SignatureExportInfo();
        }

        public bool CanWriteColors { get; set; }

        public Encrypt Encryption { get; set; }

        public PdfExportSettings Settings => this.settings;

        public byte[] DocumentId => this.documentId;

        public IEnumerable<FontBase> FontResources => (IEnumerable<FontBase>)this.fontResources.Keys;

        public Queue<IndirectObject> IndirectObjectsQueue => this.indirectObjects;

        public CrossReferenceCollection CrossReferenceCollection => this.crossReferenceCollection;

        public RadFixedDocumentInfo DocumentInfo => this.documentInfo;

        public IPdfContentExportContext AcroFormContentExportContext { get; set; }

        public SignatureExportInfo SignatureExportInfo => this.signatureExportInfo;

        public IndirectObject CreateIndirectObject(PdfPrimitive primitive)
        {
            Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, nameof(primitive));
            return this.CreateIndirectObject(primitive, true);
        }

        public void BeginExportIndirectObject(IndirectObject indirectObject, long offset)
        {
            Guard.ThrowExceptionIfNull<IndirectObject>(indirectObject, nameof(indirectObject));
            Guard.ThrowExceptionIfNotNull<IndirectObject>(this.currentIndirectObject, "currentIndirectObject");
            this.currentIndirectObject = indirectObject;
            CrossReferenceEntryType type = indirectObject.Content.Type == PdfElementType.Null ? CrossReferenceEntryType.Free : CrossReferenceEntryType.Used;
            this.crossReferenceCollection.UpdateCrossReferenceEntry(indirectObject.Reference.ObjectNumber, offset, 0, type);
        }

        public void EndExportIndirectObject()
        {
            this.currentIndirectObject.ReleaseContent();
            this.currentIndirectObject = (IndirectObject)null;
        }

        public FormFieldNode CreateFormFieldObject(
          FormField field,
          bool includeInIndirectObjectQueue)
        {
            FormFieldNode formFieldNode;
            if (!this.fieldToNode.TryGetValue(field, out formFieldNode))
            {
                formFieldNode = new FormFieldNode();
                this.fieldToNode.Add(field, formFieldNode);
            }
            IndirectObject indirectObject = this.CreateIndirectObject((PdfPrimitive)formFieldNode, false);
            if (includeInIndirectObjectQueue)
                this.IndirectObjectsQueue.Enqueue(indirectObject);
            return formFieldNode;
        }

        public ResourceEntry GetResource(FontBase resource)
        {
            ResourceEntry resourceEntry;
            if (!this.fontResources.TryGetValue(resource, out resourceEntry))
            {
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)PdfFontsFactory.CreateFont(resource.Type), "F");
                this.fontResources[resource] = resourceEntry;
            }
            return resourceEntry;
        }

        public ResourceEntry GetResource(FormSource resource)
        {
            ResourceEntry resourceEntry;
            if (!this.formResources.TryGetValue(resource, out resourceEntry))
            {
                FormXObject formXobject = new FormXObject();
                formXobject.CopyPropertiesFrom((IPdfExportContext)this, resource);
                if (!resource.Matrix.IsIdentity)
                    formXobject.Matrix = resource.Matrix.ToPdfArray();
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)formXobject, "X", true);
                this.formResources[resource] = resourceEntry;
            }
            return resourceEntry;
        }

        public ResourceEntry GetResource(Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource resource)
        {
            ResourceEntry resourceEntry;
            if (!this.imageResources.TryGetValue(resource, out resourceEntry))
            {
                ImageXObject imageXobject = new ImageXObject();
                imageXobject.CopyPropertiesFrom((IPdfExportContext)this, resource);
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)imageXobject, "I", true);
                this.imageResources[resource] = resourceEntry;
            }
            return resourceEntry;
        }

        public ResourceEntry GetResource(ExtGState resource)
        {
            ResourceEntry resourceEntry;
            if (!this.extGStateResources.TryGetValue(resource, out resourceEntry))
            {
                ExtGStateObject extGstateObject = new ExtGStateObject();
                extGstateObject.CopyPropertiesFrom((IPdfExportContext)this, resource);
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)extGstateObject, "G", true);
                this.extGStateResources[resource] = resourceEntry;
            }
            return resourceEntry;
        }

        public ResourceEntry GetResource(ColorSpaceBase first, ColorSpaceBase second)
        {
            Guard.ThrowExceptionIfNull<ColorSpaceBase>(first, nameof(first));
            Guard.ThrowExceptionIfNull<ColorSpaceBase>(second, nameof(second));
            string key = string.Format("{0}{1}", (object)first.Name, (object)second.Name);
            ResourceEntry resourceEntry;
            if (!this.colorspaceResources.TryGetValue(key, out resourceEntry))
            {
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)new PdfArray(new PdfPrimitive[0])
        {
          (PdfPrimitive) new PdfName(first.Name),
          (PdfPrimitive) new PdfName(second.Name)
        }, "C", true);
                this.colorspaceResources[key] = resourceEntry;
            }
            return resourceEntry;
        }

        public ResourceEntry GetResource(
          PatternColor resource,
          IPdfContentExportContext context)
        {
            Guard.ThrowExceptionIfNull<PatternColor>(resource, nameof(resource));
            Guard.ThrowExceptionIfNull<IPdfContentExportContext>(context, nameof(context));
            PatternColor key = resource;
            InstanceIdCache<PatternColor, ResourceEntry> instanceIdCache = this.patternResources;
            if (resource.PatternType == PatternType.Tiling && resource is UncoloredTiling uncoloredTiling)
            {
                key = (PatternColor)uncoloredTiling.Tiling;
                instanceIdCache = this.uncoloredTilingResources;
            }
            ResourceEntry resourceEntry;
            if (!instanceIdCache.TryGetValue(key, out resourceEntry))
            {
                PatternColorObject instance = PatternColorObject.CreateInstance((int)resource.PatternType);
                instance.CopyPropertiesFrom(context, (ColorBase)resource);
                resourceEntry = this.CreateResourceEntry((PdfPrimitive)instance, "P", true);
                instanceIdCache[key] = resourceEntry;
            }
            return resourceEntry;
        }

        public byte[] EncryptStream(byte[] data)
        {
            Guard.ThrowExceptionIfNull<byte[]>(data, nameof(data));
            return this.Encryption == null || this.currentIndirectObject == null || this.Encryption == this.currentIndirectObject.Content ? data : this.Encryption.EncryptStream(this.currentIndirectObject.Reference.ObjectNumber, this.currentIndirectObject.Reference.GenerationNumber, data);
        }

        public byte[] EncryptString(byte[] data)
        {
            Guard.ThrowExceptionIfNull<byte[]>(data, nameof(data));
            return this.Encryption == null || this.currentIndirectObject == null || this.Encryption == this.currentIndirectObject.Content ? data : this.Encryption.EncryptString(this.currentIndirectObject.Reference.ObjectNumber, this.currentIndirectObject.Reference.GenerationNumber, data);
        }

        public IPdfContentExportContext CreateContentExportContext(
          IResourceHolder resourceHolder,
          IContentRootElement contentRoot)
        {
            Guard.ThrowExceptionIfNull<IResourceHolder>(resourceHolder, nameof(resourceHolder));
            Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, nameof(contentRoot));
            return (IPdfContentExportContext)new PdfContentExportContext((IPdfExportContext)this, resourceHolder, contentRoot);
        }

        public void SetUsedCharacters(FontBase font, TextCollection glyphs)
        {
            Guard.ThrowExceptionIfNull<FontBase>(font, nameof(font));
            Guard.ThrowExceptionIfNull<TextCollection>(glyphs, nameof(glyphs));
            HashSet<CharInfo> charInfoSet;
            if (!this.usedCharacters.TryGetValue(font, out charInfoSet))
            {
                charInfoSet = new HashSet<CharInfo>();
                this.usedCharacters[font] = charInfoSet;
            }
            foreach (CharInfo character in glyphs.Characters)
                charInfoSet.Add(character);
        }

        public IEnumerable<CharInfo> GetUsedCharacters(FontBase font)
        {
            Guard.ThrowExceptionIfNull<FontBase>(font, nameof(font));
            HashSet<CharInfo> charInfoSet;
            return !this.usedCharacters.TryGetValue(font, out charInfoSet) ? Enumerable.Empty<CharInfo>() : (IEnumerable<CharInfo>)charInfoSet;
        }

        public Matrix GetDipToPdfPointTransformation(IContentRootElement contentRoot)
        {
            Matrix pointTransformation;
            if (!this.contentRootHeightToDipToPdfPointTransformations.TryGetValue(contentRoot.Size.Height, out pointTransformation))
            {
                pointTransformation = PdfExportContext.CalculateDipToPdfPointTransformation(contentRoot);
                this.contentRootHeightToDipToPdfPointTransformations.Add(contentRoot.Size.Height, pointTransformation);
            }
            return pointTransformation;
        }

        public static Matrix CalculateDipToPdfPointTransformation(IContentRootElement contentRoot) => new Matrix(PdfExportContext.dipToPoint, 0.0, 0.0, -PdfExportContext.dipToPoint, 0.0, Unit.DipToPoint(contentRoot.Size.Height));

        public void MapPages(RadFixedPage fixedPage, Page page)
        {
            Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, nameof(fixedPage));
            Guard.ThrowExceptionIfNull<Page>(page, nameof(page));
            this.fixedPageToPage[fixedPage] = page;
        }

        public bool TryGetPage(RadFixedPage fixedPage, out Page page)
        {
            Guard.ThrowExceptionIfNull<RadFixedPage>(fixedPage, nameof(fixedPage));
            return this.fixedPageToPage.TryGetValue(fixedPage, out page);
        }

        public void MapWidgets(Widget widgetModel, WidgetObject widgetObject)
        {
            Guard.ThrowExceptionIfNull<Widget>(widgetModel, nameof(widgetModel));
            Guard.ThrowExceptionIfNull<WidgetObject>(widgetObject, nameof(widgetObject));
            this.widgetModelToWidgetObject[widgetModel] = widgetObject;
        }

        public bool TryGetWidget(Widget widgetModel, out WidgetObject widgetObject)
        {
            Guard.ThrowExceptionIfNull<Widget>(widgetModel, nameof(widgetModel));
            return this.widgetModelToWidgetObject.TryGetValue(widgetModel, out widgetObject);
        }

        protected IndirectObject CreateIndirectObject(
          PdfPrimitive primitive,
          bool includeInIndirectObjectsQueue)
        {
            int nextObjectNumber = this.GetNextObjectNumber();
            return this.CreateIndirectObject(primitive, nextObjectNumber, includeInIndirectObjectsQueue, true);
        }

        protected IndirectObject CreateIndirectObject(
          PdfPrimitive primitive,
          int objectNumber,
          bool includeInIndirectObjectsQueue,
          bool addToCrossReferences)
        {
            Guard.ThrowExceptionIfNull<PdfPrimitive>(primitive, nameof(primitive));
            IndirectObject indirectObject;
            if (!this.registeredIndirectObjects.TryGetValue(primitive, out indirectObject))
            {
                indirectObject = new IndirectObject(new IndirectReference(objectNumber, 0), primitive);
                this.registeredIndirectObjects[primitive] = indirectObject;
                if (includeInIndirectObjectsQueue)
                    this.IndirectObjectsQueue.Enqueue(indirectObject);
                if (addToCrossReferences)
                    this.crossReferenceCollection.AddCrossReferenceEntry(objectNumber, new CrossReferenceEntry(0L, 0, CrossReferenceEntryType.Free));
            }
            return indirectObject;
        }

        protected virtual int GetNextObjectNumber() => this.CrossReferenceCollection.MaxObjectNumber + 1;

        private ResourceEntry CreateResourceEntry(
          PdfPrimitive primitive,
          string keyPrefix,
          bool includeInQueue = false)
        {
            IndirectObject indirectObject = this.CreateIndirectObject(primitive, includeInQueue);
            return new ResourceEntry(this.GetNextResourceKey(keyPrefix), indirectObject);
        }

        private string GetNextResourceKey(string keyPrefix)
        {
            switch (keyPrefix)
            {
                case "F":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.fontResourcesCount++);
                case "X":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.formXObjectResourcesCount++);
                case "I":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.imageResourcesCount++);
                case "P":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.patternResourcesCount++);
                case "G":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.graphicResourcesCount++);
                case "C":
                    return string.Format("{0}{1}", (object)keyPrefix, (object)this.colorResourcesCount++);
                default:
                    throw new ArgumentException("Invalid prefix argument: {0}", keyPrefix);
            }
        }

        private static byte[] CreateDocumentId() => MD5Core.GetHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString("o", (IFormatProvider)CultureInfo.InvariantCulture)));
    }
}
