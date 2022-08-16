using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	public class PdfObject
	{
		internal PdfObject(IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			this.exportContext = context;
		}

		internal PdfObject(IPdfExportContext context, int width, int height, ColorSpace colorSpace)
			: this(context)
		{
			this.width = width;
			this.height = height;
			this.colorSpace = colorSpace;
		}

		internal PdfObject(int width, int height, ColorSpace colorSpace)
		{
			this.width = width;
			this.height = height;
			this.colorSpace = colorSpace;
		}

		internal PdfObject(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			Guard.ThrowExceptionIfNull<PostScriptReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IPdfImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfDictionary>(dictionary, "dictionary");
			this.reader = reader;
			this.importContext = context;
			this.dictionary = dictionary;
		}

		internal PdfObject(PdfDictionaryOld dictionary)
		{
			Guard.ThrowExceptionIfNull<PdfDictionaryOld>(dictionary, "dictionary");
			this.oldDictionary = dictionary;
		}

		public ColorSpace ColorSpace
		{
			get
			{
				return this.GetColorSpace();
			}
			set
			{
				this.SetColorSpace(value);
			}
		}

		public int BitsPerComponent
		{
			get
			{
				int result;
				if (this.TryGetIntValue("BitsPerComponent", out result))
				{
					return result;
				}
				return 0;
			}
			internal set
			{
				this.SetIntValue("BitsPerComponent", value);
			}
		}

		public int Width
		{
			get
			{
				int result;
				if (!this.TryGetIntValue("Width", out result))
				{
					result = this.width;
				}
				return result;
			}
			internal set
			{
				this.SetIntValue("Width", value);
			}
		}

		public int Height
		{
			get
			{
				int result;
				if (!this.TryGetIntValue("Height", out result))
				{
					result = this.height;
				}
				return result;
			}
			internal set
			{
				this.SetIntValue("Height", value);
			}
		}

		internal IPdfExportContext ExportContext
		{
			get
			{
				return this.exportContext;
			}
		}

		internal PdfContentManager ContentManager
		{
			get
			{
				if (this.oldDictionary != null)
				{
					return this.oldDictionary.ContentManager;
				}
				return null;
			}
		}

		internal T GetColorSpaceInternal<T>() where T : class
		{
			ColorSpaceObject colorSpaceObject;
			if (this.dictionary != null && this.dictionary.TryGetElement<ColorSpaceObject>(this.reader, this.importContext, "ColorSpace", out colorSpaceObject))
			{
				return colorSpaceObject as T;
			}
			if (this.oldDictionary != null && this.oldDictionary.ContainsKey("ColorSpace"))
			{
				return this.oldDictionary.GetElement<ColorSpaceOld>("ColorSpace", Converters.ColorSpaceConverter) as T;
			}
			return default(T);
		}

		internal ColorSpaceObject GetColorSpaceObject()
		{
			ColorSpaceObject result;
			if (this.dictionary != null && this.dictionary.TryGetElement<ColorSpaceObject>(this.reader, this.importContext, "ColorSpace", out result))
			{
				return result;
			}
			if (this.oldDictionary != null && this.oldDictionary.ContainsKey("ColorSpace"))
			{
				ColorSpaceOld element = this.oldDictionary.GetElement<ColorSpaceOld>("ColorSpace", Converters.ColorSpaceConverter);
				return OldToProcessingTranslator.GetColorSpaceObjectFromColorSpaceOld(element);
			}
			return null;
		}

		bool TryGetIntValue(string index, out int value)
		{
			value = 0;
			if (this.dictionary != null)
			{
				PdfInt pdfInt;
				this.dictionary.TryGetElement<PdfInt>(this.reader, this.importContext, index, out pdfInt);
				value = pdfInt.Value;
				return true;
			}
			return this.oldDictionary != null && this.oldDictionary.TryGetInt(index, out value);
		}

		void SetIntValue(string index, int value)
		{
			if (this.dictionary != null)
			{
				this.dictionary[index] = new PdfInt(value);
			}
			if (this.oldDictionary != null)
			{
				this.oldDictionary[index] = new PdfIntOld(this.oldDictionary.ContentManager, value);
			}
		}

		ColorSpace GetColorSpace()
		{
			if (this.dictionary != null)
			{
				ColorSpaceObject colorSpaceInternal = this.GetColorSpaceInternal<ColorSpaceObject>();
				if (colorSpaceInternal != null)
				{
					return colorSpaceInternal.Public;
				}
			}
			if (this.oldDictionary != null)
			{
				ColorSpaceOld colorSpaceInternal2 = this.GetColorSpaceInternal<ColorSpaceOld>();
				if (colorSpaceInternal2 != null)
				{
					return colorSpaceInternal2.Type;
				}
			}
			return this.colorSpace;
		}

		void SetColorSpace(ColorSpace value)
		{
			if (this.dictionary != null)
			{
				this.dictionary["ColorSpace"] = ColorSpaceManager.CreateColorSpaceObject(value);
			}
			if (this.oldDictionary != null)
			{
				this.oldDictionary["ColorSpace"] = ColorSpaceOld.CreateColorSpace(this.oldDictionary.ContentManager, value);
			}
		}

		const string BitsPerComponentName = "BitsPerComponent";

		const string WidthName = "Width";

		const string HeightName = "Height";

		readonly PdfDictionaryOld oldDictionary;

		readonly PdfDictionary dictionary;

		readonly PostScriptReader reader;

		readonly IPdfImportContext importContext;

		readonly IPdfExportContext exportContext;

		readonly ColorSpace colorSpace;

		readonly int width;

		readonly int height;
	}
}
