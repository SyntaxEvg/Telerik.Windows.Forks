using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	class InlineImageInfo : IImageDescriptor
	{
		public InlineImageInfo(XImage xImage, PdfDictionaryOld dictionary, byte[] rawData, long endOfImagePosition)
		{
			this.imageDescriptor = xImage;
			this.xImage = xImage;
			this.dictionary = dictionary;
			this.rawData = rawData;
			this.endOfImagePosition = endOfImagePosition;
		}

		public XImage XImage
		{
			get
			{
				return this.xImage;
			}
		}

		public PdfDictionaryOld Dictionary
		{
			get
			{
				return this.dictionary;
			}
		}

		public byte[] RawData
		{
			get
			{
				return this.rawData;
			}
		}

		public long EndOfImagePosition
		{
			get
			{
				return this.endOfImagePosition;
			}
		}

		int IImageDescriptor.Width
		{
			get
			{
				return this.imageDescriptor.Width;
			}
		}

		int IImageDescriptor.Height
		{
			get
			{
				return this.imageDescriptor.Height;
			}
		}

		int IImageDescriptor.BitsPerComponent
		{
			get
			{
				return this.imageDescriptor.BitsPerComponent;
			}
		}

		double[] IImageDescriptor.Decode
		{
			get
			{
				return this.imageDescriptor.Decode;
			}
		}

		IColorSpace IImageDescriptor.ColorSpace
		{
			get
			{
				return this.imageDescriptor.ColorSpace;
			}
		}

		IMask IImageDescriptor.Mask
		{
			get
			{
				return this.imageDescriptor.Mask;
			}
		}

		IImageDescriptor IImageDescriptor.SMask
		{
			get
			{
				return this.imageDescriptor.SMask;
			}
		}

		string[] IImageDescriptor.Filters
		{
			get
			{
				return FiltersManagerOld.GetFilterValues(this.XImage.ContentManager, this.Dictionary);
			}
		}

		byte[] IImageDescriptor.GetDecodedData()
		{
			byte[] array = this.RawData;
			if (this.Dictionary.ContainsKey("Filter"))
			{
				PdfNameOld[] filters = FiltersManagerOld.GetFilters(this.XImage.ContentManager, this.Dictionary);
				DecodeParameters[] decodeParameters = FiltersManagerOld.GetDecodeParameters(this.XImage.ContentManager, this.Dictionary);
				array = FiltersManagerOld.Decode(this.dictionary, filters, array, decodeParameters);
			}
			return array;
		}

		byte[] IImageDescriptor.GetEncodedData()
		{
			return this.RawData;
		}

		readonly IImageDescriptor imageDescriptor;

		readonly XImage xImage;

		readonly byte[] rawData;

		readonly long endOfImagePosition;

		readonly PdfDictionaryOld dictionary;
	}
}
