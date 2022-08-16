using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects
{
	class FormXObject : XObjectBase
	{
		public FormXObject(PdfStreamBase importedStream, PdfFileSource fileSource, PdfFileStreamExportContext exportContext)
			: this()
		{
			this.encodedContentData = importedStream.ReadRawPdfData();
			base.Load(fileSource.Context.Reader, fileSource.Context, importedStream.Dictionary);
			if (base.Filters != null)
			{
				base.Filters = ResourceRenamer.GetArrayWithRenamedResources(base.Filters, fileSource, exportContext);
			}
			if (base.DecodeParms != null)
			{
				switch (base.DecodeParms.Type)
				{
				case PdfElementType.Dictionary:
				{
					PdfDictionary original = (PdfDictionary)base.DecodeParms.Primitive;
					base.DecodeParms = new PrimitiveWrapper(ResourceRenamer.GetDictionaryWithRenamedResources(original, fileSource, exportContext));
					return;
				}
				case PdfElementType.Array:
				{
					PdfArray original2 = (PdfArray)base.DecodeParms.Primitive;
					base.DecodeParms = new PrimitiveWrapper(ResourceRenamer.GetArrayWithRenamedResources(original2, fileSource, exportContext));
					break;
				}
				default:
					return;
				}
			}
		}

		public FormXObject(byte[] decodedContentData)
			: this()
		{
			this.decodedContentData = decodedContentData;
		}

		public FormXObject()
		{
			this.boundingBox = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("BBox", true));
			this.resources = base.RegisterDirectProperty<PdfDictionary>(new PdfPropertyDescriptor("Resources"));
			this.matrix = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Matrix"));
		}

		public PdfArray BoundingBox
		{
			get
			{
				return this.boundingBox.GetValue();
			}
			set
			{
				this.boundingBox.SetValue(value);
			}
		}

		public PdfArray Matrix
		{
			get
			{
				return this.matrix.GetValue();
			}
			set
			{
				this.matrix.SetValue(value);
			}
		}

		public PdfDictionary Resources
		{
			get
			{
				return this.resources.GetValue();
			}
			set
			{
				this.resources.SetValue(value);
			}
		}

		public override XObjectType XObjectType
		{
			get
			{
				return XObjectType.Form;
			}
		}

		internal byte[] DecodedContentData
		{
			get
			{
				return this.decodedContentData;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			if (this.decodedContentData == null)
			{
				base.WriteDictionaryAndEncodedData(writer, context, this.encodedContentData);
				return;
			}
			base.Write(writer, context);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, FormSource formSource)
		{
			this.CopyPropertiesFrom(context, formSource, formSource.BoundingBox);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, IContentRootElement contentRoot, Rect boundingBox)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IContentRootElement>(contentRoot, "contentRoot");
			ResourceHolder resourceHolder = new ResourceHolder();
			this.decodedContentData = ContentStream.BuildContentData(context, resourceHolder, contentRoot);
			this.BoundingBox = contentRoot.ToBottomLeftCoordinateSystem(boundingBox);
			this.CopyResourcesFrom(resourceHolder);
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			if (this.decodedContentData == null)
			{
				return base.GetData(context);
			}
			return this.decodedContentData;
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			this.decodedContentData = stream.ReadDecodedPdfData();
		}

		internal FormSource ToFormSource(PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			FormSource formSource = new FormSource();
			Rect rect = this.BoundingBox.ToRect(reader, context);
			formSource.BoundingBox = rect.ToTopLeftCoordinateSystem(rect.Height);
			ResourceHolder resourceHolder = new ResourceHolder();
			if (this.Resources != null)
			{
				resourceHolder.Resources.Load(reader, context, this.Resources);
			}
			if (this.Matrix != null)
			{
				formSource.Matrix = this.Matrix.ToMatrix(reader, context);
			}
			ContentStream.ParseContentData(this.decodedContentData, context, resourceHolder, formSource);
			return formSource;
		}

		internal void CopyResourcesFrom(IResourceHolder resourceHolder)
		{
			this.CopyResourcesFrom(resourceHolder, (IPdfProperty property) => property.GetValue());
		}

		internal void CopyResourcesFrom(IResourceHolder resourceHolder, PdfFileSource fileSource, PdfFileStreamExportContext exportContext)
		{
			this.CopyResourcesFrom(resourceHolder, delegate(IPdfProperty property)
			{
				PdfPrimitive value = property.GetValue();
				PdfPrimitive result;
				switch (value.Type)
				{
				case PdfElementType.Dictionary:
				{
					PdfDictionary original = (PdfDictionary)value;
					result = ResourceRenamer.GetDictionaryWithRenamedResources(original, fileSource, exportContext);
					break;
				}
				case PdfElementType.Array:
				{
					PdfArray original2 = (PdfArray)value;
					result = ResourceRenamer.GetArrayWithRenamedResources(original2, fileSource, exportContext);
					break;
				}
				default:
					throw new InvalidOperationException(string.Format("Invalid resource primitive type: {0}", value.Type));
				}
				return result;
			});
		}

		void CopyResourcesFrom(IResourceHolder resourceHolder, Func<IPdfProperty, PdfPrimitive> getValueFromProperty)
		{
			foreach (IPdfProperty pdfProperty in resourceHolder.Resources.Properties.Values)
			{
				if (pdfProperty.HasValue && pdfProperty.HasNonDefaultValue)
				{
					if (this.Resources == null)
					{
						this.Resources = new PdfDictionary();
					}
					this.Resources[pdfProperty.Descriptor.Name] = getValueFromProperty(pdfProperty);
				}
			}
		}

		readonly DirectProperty<PdfArray> boundingBox;

		readonly DirectProperty<PdfDictionary> resources;

		readonly DirectProperty<PdfArray> matrix;

		readonly byte[] encodedContentData;

		byte[] decodedContentData;
	}
}
