using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Actions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.CMaps;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Destinations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Functions;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Metadata;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Patterns;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model
{
	static class PdfObjectDescriptors
	{
		static PdfObjectDescriptors()
		{
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(DocumentCatalog), new PdfObjectDescriptor("Catalog"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(PageTreeNode), new PdfObjectDescriptor("Pages", new PageTypeAwareConverter(new Func<PdfName, object>(PageTreeNode.CreateInstance), "Type")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Page), new PdfObjectDescriptor("Page", new PageTypeAwareConverter(new Func<PdfName, object>(PageTreeNode.CreateInstance), "Type")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(PdfReal), new PdfObjectDescriptor(new PdfRealConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(ContentStream), new PdfObjectDescriptor(new ContentStreamConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(XFAStream), new PdfObjectDescriptor(new XFAStreamConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SimpleFontEncoding), new PdfObjectDescriptor("Encoding", new SimpleFontEncodingConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(FontObject), new PdfObjectDescriptor(new TypeAwareConverter<PdfName>(new Func<PdfName, object>(PdfFontsFactory.CreateInstance), "Subtype")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Type1FontObject), new PdfObjectDescriptor("Font", new PdfName("Type1"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(TrueTypeFontObject), new PdfObjectDescriptor("Font", new PdfName("TrueType"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Type0FontObject), new PdfObjectDescriptor("Font", new PdfName("Type0"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(CidFontObject), new PdfObjectDescriptor(new TypeAwareConverter<PdfName>(new Func<PdfName, object>(CidFontObject.CreateInstance), "Subtype")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(CidFontType0Object), new PdfObjectDescriptor("Font", new PdfName("CIDFontType0"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(CidFontType2Object), new PdfObjectDescriptor("Font", new PdfName("CIDFontType2"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(PrimitiveWrapper), new PdfObjectDescriptor(new PrimitiveWrapperConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(ImageXObject), new PdfObjectDescriptor("XObject", new PdfName("Image"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(FormXObject), new PdfObjectDescriptor("XObject", new PdfName("Form"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(XObjectBase), new PdfObjectDescriptor(new TypeAwareConverterWithParameter<PdfName, PdfBool>(new Func<PdfName, PdfBool, object>(XObjectFactory.CreateInstance), "Subtype", "ImageMask")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Mask), new PdfObjectDescriptor(new MaskConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(AnnotationObject), new PdfObjectDescriptor(new TypeAwareConverter<PdfName>(new Func<PdfName, object>(AnnotationFactory.CreateInstance), "Subtype")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(ActionObject), new PdfObjectDescriptor(new TypeAwareConverter<PdfName>(new Func<PdfName, object>(ActionObject.CreateInstance), "S")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(LinkObject), new PdfObjectDescriptor("Annot", new PdfName("Link"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(WidgetObject), new PdfObjectDescriptor("Annot", new PdfName("Widget"), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(DestinationObject), new PdfObjectDescriptor("Dest", new DestinationConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(GoToActionObject), new PdfObjectDescriptor("Action"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(UriActionObject), new PdfObjectDescriptor("Action"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Encrypt), new PdfObjectDescriptor(new TypeAwareConverter<PdfName>(new Func<PdfName, object>(Encrypt.CreateInstance), "Filter")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(EncodingBaseObject), new PdfObjectDescriptor(new EncodingBaseConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(CharIdToGlyphIdMappingBase), new PdfObjectDescriptor(new CharCodeToGlyphIdMappingBaseConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(ColorSpaceObject), new PdfObjectDescriptor(new ColorSpaceConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(ShadingPatternObject), new PdfObjectDescriptor(new PdfInt(2), "PatternType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(AxialShading), new PdfObjectDescriptor(new PdfInt(2), "ShadingType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(RadialShading), new PdfObjectDescriptor(new PdfInt(3), "ShadingType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SampledFunctionObject), new PdfObjectDescriptor(new PdfInt(0), "FunctionType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(StitchingFunctionObject), new PdfObjectDescriptor(new PdfInt(3), "FunctionType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Tiling), new PdfObjectDescriptor(new PdfInt(1), "PatternType"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(PatternColorObject), new PdfObjectDescriptor(new PatternColorConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Shading), new PdfObjectDescriptor(new TypeAwareConverter<PdfInt>(new Func<PdfInt, object>(Shading.CreateInstance), "ShadingType")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(FunctionObject), new PdfObjectDescriptor(new TypeAwareConverter<PdfInt>(new Func<PdfInt, object>(FunctionObject.CreateFunction), "FunctionType")));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(PdfArray), new PdfObjectDescriptor(new PdfArrayConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(FontDescriptor), new PdfObjectDescriptor("FontDescriptor"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(DocumentInfo), new PdfObjectDescriptor("Metadata", "XML".ToPdfName(), "Subtype"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(Appearances), new PdfObjectDescriptor(new AppearancesConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(FormFieldsTree), new PdfObjectDescriptor(new FieldsConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(CrossReferenceStream), new PdfObjectDescriptor("XRef"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SignatureReferenceObject), new PdfObjectDescriptor("SigRef"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SignatureObject), new PdfObjectDescriptor("Sig"));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SignatureByteRange), new PdfObjectDescriptor(new SignatureByteRangeConverter()));
			PdfObjectDescriptors.RegisterTypeDescriptor(typeof(SignatureContents), new PdfObjectDescriptor(new SignatureContentsConverter()));
		}

		public static PdfObjectDescriptor GetPdfObjectDescriptor(PdfPrimitive pdfObject)
		{
			return PdfObjectDescriptors.GetPdfObjectDescriptor(pdfObject.GetType());
		}

		public static PdfObjectDescriptor GetPdfObjectDescriptor<T>()
		{
			return PdfObjectDescriptors.GetPdfObjectDescriptor(typeof(T));
		}

		public static PdfObjectDescriptor GetPdfObjectDescriptor(Type type)
		{
			PdfObjectDescriptor result;
			if (PdfObjectDescriptors.pdfObjectDescriptors.TryGetValue(type, out result))
			{
				return result;
			}
			return PdfObjectDescriptors.CreateDefaultPdfObjectDescriptor(type);
		}

		static PdfObjectDescriptor CreateDefaultPdfObjectDescriptor(Type type)
		{
			PdfObjectDescriptor pdfObjectDescriptor = new PdfObjectDescriptor();
			PdfObjectDescriptors.RegisterTypeDescriptor(type, pdfObjectDescriptor);
			return pdfObjectDescriptor;
		}

		static void RegisterTypeDescriptor(Type type, PdfObjectDescriptor descriptor)
		{
			PdfObjectDescriptors.pdfObjectDescriptors[type] = descriptor;
		}

		static readonly Dictionary<Type, PdfObjectDescriptor> pdfObjectDescriptors = new Dictionary<Type, PdfObjectDescriptor>();
	}
}
