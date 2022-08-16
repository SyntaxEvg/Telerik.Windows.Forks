using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	static class Converters
	{
		static Converters()
		{
			Converters.actionConverter = new ActionConverter();
			Converters.encryptConverter = new EncryptConverter();
			Converters.cidFontConverter = new CIDFontConverter();
			Converters.cidToGidMapConverter = new CIDToGIDMapConverter();
			Converters.pdfNameConverter = new PdfNameConverter();
			Converters.patternConverter = new PatternConverter();
			Converters.shadingConverter = new ShadingConverter();
			Converters.functionConverter = new FunctionConverter();
			Converters.fontConverter = new FontConverter();
			Converters.xobjectConverter = new XObjectConverter();
			Converters.lookupConverter = new LookupConverter();
			Converters.filtersConverter = new FiltersConverter();
			Converters.decodeParametersConverter = new DecodeParametersConverter();
			Converters.pdfStringConverter = new PdfStringConverter();
			Converters.maskConverter = new MaskConverter();
			Converters.xfaConverter = new XFAStreamConverterOld();
			Converters.fieldsConverter = new FieldsConverterOld();
			Converters.arrayConverter = new PdfArrayConverterOld();
		}

		public static MaskConverter MaskConverter
		{
			get
			{
				return Converters.maskConverter;
			}
		}

		public static PdfStringConverter PdfStringConverter
		{
			get
			{
				return Converters.pdfStringConverter;
			}
		}

		public static PdfArrayConverterOld ArrayConverter
		{
			get
			{
				return Converters.arrayConverter;
			}
		}

		public static FunctionConverter FunctionConverter
		{
			get
			{
				return Converters.functionConverter;
			}
		}

		public static AnnotConverter AnnotConverter
		{
			get
			{
				return Converters.annotConverter;
			}
		}

		public static FieldsConverterOld FieldsConverter
		{
			get
			{
				return Converters.fieldsConverter;
			}
		}

		public static XFAStreamConverterOld XFAConverter
		{
			get
			{
				return Converters.xfaConverter;
			}
		}

		public static ActionConverter ActionConverter
		{
			get
			{
				return Converters.actionConverter;
			}
		}

		public static DestinationConverter DestinationConverter
		{
			get
			{
				return Converters.destinationConverter;
			}
		}

		public static PdfNameConverter PdfNameConverter
		{
			get
			{
				return Converters.pdfNameConverter;
			}
		}

		public static PdfBoolConverter PdfBoolConverter
		{
			get
			{
				return Converters.pdfBoolConverter;
			}
		}

		public static PdfIntConverter PdfIntConverter
		{
			get
			{
				return Converters.pdfIntConverter;
			}
		}

		public static PdfRealConverter PdfRealConverter
		{
			get
			{
				return Converters.pdfRealConverter;
			}
		}

		public static CIDToGIDMapConverter CidToGidMapConverter
		{
			get
			{
				return Converters.cidToGidMapConverter;
			}
		}

		public static CMapStreamConverter CMapStreamConverter
		{
			get
			{
				return Converters.cmapConverter;
			}
		}

		public static EncodingConverter EncodingConverter
		{
			get
			{
				return Converters.encodingConverter;
			}
		}

		public static CIDFontConverter CIDFontConverter
		{
			get
			{
				return Converters.cidFontConverter;
			}
		}

		public static ContentsConverter ContentsConverter
		{
			get
			{
				return Converters.contentsConverter;
			}
		}

		public static AppearanceConverterOld AppearanceConverter
		{
			get
			{
				return Converters.appearanceConverter;
			}
		}

		public static ShadingConverter ShadingConverter
		{
			get
			{
				return Converters.shadingConverter;
			}
		}

		public static PatternConverter PatternConverter
		{
			get
			{
				return Converters.patternConverter;
			}
		}

		public static ColorSpaceConverter ColorSpaceConverter
		{
			get
			{
				return Converters.colorSpaceConverter;
			}
		}

		public static PdfDictionaryToPdfObjectConverter PdfDictionaryToPdfObjectConverter
		{
			get
			{
				return Converters.pdfDictionaryToPdfObjectConverter;
			}
		}

		public static EncryptConverter EncryptConverter
		{
			get
			{
				return Converters.encryptConverter;
			}
		}

		public static FontConverter FontConverter
		{
			get
			{
				return Converters.fontConverter;
			}
		}

		public static XObjectConverter XObjectConverter
		{
			get
			{
				return Converters.xobjectConverter;
			}
		}

		public static LookupConverter LookupConverter
		{
			get
			{
				return Converters.lookupConverter;
			}
		}

		public static FiltersConverter FiltersConverter
		{
			get
			{
				return Converters.filtersConverter;
			}
		}

		public static DecodeParametersConverter DecodeParametersConverter
		{
			get
			{
				return Converters.decodeParametersConverter;
			}
		}

		public static IndirectReferenceToPdfObjectConverter IndirectReferenceToPdfObjectConverter
		{
			get
			{
				return Converters.indirectReferenceToPdfObjectConverter;
			}
		}

		static readonly IndirectReferenceToPdfObjectConverter indirectReferenceToPdfObjectConverter = new IndirectReferenceToPdfObjectConverter();

		static readonly EncryptConverter encryptConverter;

		static readonly PdfDictionaryToPdfObjectConverter pdfDictionaryToPdfObjectConverter = new PdfDictionaryToPdfObjectConverter();

		static readonly ColorSpaceConverter colorSpaceConverter = new ColorSpaceConverter();

		static readonly PatternConverter patternConverter;

		static readonly ShadingConverter shadingConverter;

		static readonly ContentsConverter contentsConverter = new ContentsConverter();

		static readonly AppearanceConverterOld appearanceConverter = new AppearanceConverterOld();

		static readonly CIDFontConverter cidFontConverter;

		static readonly CIDToGIDMapConverter cidToGidMapConverter;

		static readonly EncodingConverter encodingConverter = new EncodingConverter();

		static readonly CMapStreamConverter cmapConverter = new CMapStreamConverter();

		static readonly PdfRealConverter pdfRealConverter = new PdfRealConverter();

		static readonly PdfIntConverter pdfIntConverter = new PdfIntConverter();

		static readonly PdfBoolConverter pdfBoolConverter = new PdfBoolConverter();

		static readonly PdfNameConverter pdfNameConverter;

		static readonly DestinationConverter destinationConverter = new DestinationConverter();

		static readonly ActionConverter actionConverter;

		static readonly AnnotConverter annotConverter = new AnnotConverter();

		static readonly FunctionConverter functionConverter;

		static readonly FontConverter fontConverter;

		static readonly XObjectConverter xobjectConverter;

		static readonly LookupConverter lookupConverter;

		static readonly FiltersConverter filtersConverter;

		static readonly DecodeParametersConverter decodeParametersConverter;

		static readonly PdfStringConverter pdfStringConverter;

		static readonly MaskConverter maskConverter;

		static readonly XFAStreamConverterOld xfaConverter;

		static readonly FieldsConverterOld fieldsConverter;

		static readonly PdfArrayConverterOld arrayConverter;
	}
}
