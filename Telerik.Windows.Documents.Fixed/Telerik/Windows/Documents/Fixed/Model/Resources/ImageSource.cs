using System;
using System.IO;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Core.Imaging.Jpeg2000;
using Telerik.Windows.Documents.Core.Imaging.Jpeg2000.Decoder;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Extensions;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	public class ImageSource : IInstanceIdOwner, IImageDescriptor
	{
		public ImageSource(Stream stream)
			: this()
		{
			this.unknownImageData = BytesHelper.GetBytes(stream);
			this.isInitialized = false;
		}

		public ImageSource(Stream stream, ImageQuality imageQuality)
			: this()
		{
			byte[] bytes = BytesHelper.GetBytes(stream);
			this.CalculateEncodedDataFromUnknownData(bytes, imageQuality);
		}

		public ImageSource(EncodedImageData imageSourceInfo)
			: this()
		{
			Guard.ThrowExceptionIfNull<EncodedImageData>(imageSourceInfo, "imageSourceInfo");
			this.InitializeEncodedData(imageSourceInfo);
		}

		public ImageSource(BitmapSource bitmapSource)
			: this(ImageSourceExtensions.CreateImageStream(bitmapSource))
		{
			this.InitializeImageInfo(bitmapSource);
		}

		public ImageSource(BitmapSource bitmapSource, ImageQuality imageQuality)
			: this()
		{
			this.imageQuality = imageQuality;
			this.InitializeEncodedData(bitmapSource);
		}

		internal ImageSource(byte[] encodedImageData, int bitsPerComponent, int width, int height, ColorSpaceBase colorSpace, string[] filters, double[] decodeArray, bool imageMask)
			: this()
		{
			Guard.ThrowExceptionIfNull<byte[]>(encodedImageData, "encodedImageData");
			this.encodedImageData = encodedImageData;
			this.InitializeImageInfo(colorSpace, filters, width, height, bitsPerComponent, decodeArray, imageMask);
		}

		ImageSource()
		{
			this.id = InstanceIdGenerator.GetNextId();
			this.defaultDecodeArray = null;
			this.mask = new PdfProperty<ImageMask>();
			this.sMask = new PdfProperty<ImageSource>();
			this.colorSpace = new PdfProperty<ColorSpaceBase>();
			this.bitsPerComponent = new PdfProperty<int>();
			this.decodeArray = new PdfProperty<double[]>(() => this.defaultDecodeArray);
			this.imageQuality = FixedDocumentDefaults.ImageQuality;
			this.imageMask = new PdfProperty<bool>();
		}

		public int Height
		{
			get
			{
				this.EnsureImageInfo();
				return this.height;
			}
		}

		public int Width
		{
			get
			{
				this.EnsureImageInfo();
				return this.width;
			}
		}

		public double[] DecodeArray
		{
			get
			{
				return this.decodeArray.Value;
			}
			set
			{
				this.decodeArray.Value = value;
			}
		}

		internal DecodeParameters[] DecodeParameters { get; set; }

		internal string[] Filters
		{
			get
			{
				this.EnsureImageInfo();
				return this.filters;
			}
		}

		internal PdfProperty<int> BitsPerComponent
		{
			get
			{
				this.EnsureImageInfo();
				return this.bitsPerComponent;
			}
		}

		internal PdfProperty<ColorSpaceBase> ColorSpace
		{
			get
			{
				this.EnsureImageInfo();
				return this.colorSpace;
			}
		}

		internal PdfProperty<ImageMask> Mask
		{
			get
			{
				return this.mask;
			}
		}

		internal PdfProperty<ImageSource> SMask
		{
			get
			{
				return this.sMask;
			}
		}

		internal PdfProperty<bool> ImageMask
		{
			get
			{
				return this.imageMask;
			}
		}

		int IInstanceIdOwner.InstanceId
		{
			get
			{
				return this.id;
			}
		}

		int IImageDescriptor.BitsPerComponent
		{
			get
			{
				return this.BitsPerComponent.Value;
			}
		}

		double[] IImageDescriptor.Decode
		{
			get
			{
				return this.DecodeArray;
			}
		}

		IColorSpace IImageDescriptor.ColorSpace
		{
			get
			{
				if (!this.ColorSpace.HasValue && this.ImageMask.HasValue && this.ImageMask.Value)
				{
					return new DeviceGray();
				}
				return this.ColorSpace.Value;
			}
		}

		IMask IImageDescriptor.Mask
		{
			get
			{
				return this.Mask.Value;
			}
		}

		IImageDescriptor IImageDescriptor.SMask
		{
			get
			{
				return this.SMask.Value;
			}
		}

		string[] IImageDescriptor.Filters
		{
			get
			{
				return this.Filters;
			}
		}

		byte[] IImageDescriptor.GetDecodedData()
		{
			this.EnsureImageInfo();
			ColorSpace colorSpace = Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.ColorSpace.Undefined;
			if (this.ColorSpace.HasValue)
			{
				ColorSpaceObject colorSpaceObject = ColorSpaceManager.CreateColorSpaceObject(this.ColorSpace.Value);
				colorSpace = colorSpaceObject.Public;
			}
			PdfObject obj = new PdfObject(this.Width, this.Height, colorSpace);
			return FiltersManager.Decode(obj, this.encodedImageData, this.Filters, this.DecodeParameters);
		}

		byte[] IImageDescriptor.GetEncodedData()
		{
			return this.GetEncodedImageData(FixedDocumentDefaults.ImageQuality);
		}

		public EncodedImageData GetEncodedImageData()
		{
			byte[] data = this.GetEncodedImageData(FixedDocumentDefaults.ImageQuality);
			string text = (this.ColorSpace.HasValue ? this.ColorSpace.Value.Name : null);
			return new EncodedImageData(data, this.BitsPerComponent.Value, this.Width, this.Height, text, this.Filters);
		}

		public BitmapSource GetBitmapSource()
		{
			ImageDataSource dataSource = this.ToImageDataSource();
			return dataSource.ToBitmapSource(null);
		}

		internal BitmapSource GetBitmapSource(ColorBase stencilColor)
		{
			Guard.ThrowExceptionIfNull<ColorBase>(stencilColor, "stencilColor");
			Guard.ThrowExceptionIfFalse(this.ImageMask.Value, "ImageMask.Value");
			Color stencilColor2 = stencilColor.ToColor();
			ImageDataSource dataSource = this.ToDecodedImageDataSource(stencilColor2);
			return dataSource.ToBitmapSource(null);
		}

		internal byte[] GetEncodedImageData(ImageQuality imageQuality)
		{
			if (this.unknownImageData != null)
			{
				this.EnsureEncodedDataFromUnknownImageData(imageQuality);
			}
			return this.encodedImageData;
		}

		static PdfArray CreateFilters(string[] filters)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (string initialValue in filters)
			{
				pdfArray.Add(new PdfName(initialValue));
			}
			return pdfArray;
		}

		static bool CheckIfJpeg2000(byte[] imageData)
		{
			Jpeg2000HeadersDecoder jpeg2000HeadersDecoder = new Jpeg2000HeadersDecoder(imageData);
			Jpeg2000ImageInfo jpeg2000ImageInfo;
			return jpeg2000HeadersDecoder.TryDecodeJpeg2000Image(out jpeg2000ImageInfo);
		}

		void EnsureEncodedDataFromUnknownImageData(ImageQuality imageQuality)
		{
			bool flag = this.encodedImageData == null || this.imageQuality != imageQuality;
			if (flag)
			{
				this.CalculateEncodedDataFromUnknownData(this.unknownImageData, imageQuality);
			}
		}

		void InitializeEncodedData(EncodedImageData encodedData)
		{
			this.encodedImageData = encodedData.Data;
			this.InitializeImageInfo(encodedData);
			if (encodedData.AlphaChannel == null)
			{
				this.SMask.ClearValue();
				return;
			}
			EncodedImageData imageSourceInfo = new EncodedImageData(encodedData.AlphaChannel, encodedData.BitsPerComponent, encodedData.Width, encodedData.Height, "DeviceGray", encodedData.Filters);
			this.SMask.Value = new ImageSource(imageSourceInfo);
		}

		void InitializeEncodedData(BitmapSource bitmapSource)
		{
			this.InitializeImageInfo(bitmapSource);
			byte[] inputData;
			byte[] array;
			ImageSource.GetData(bitmapSource, out inputData, out array);
			this.encodedImageData = DctDecode.Encode(this.Width, this.Height, (float)this.imageQuality, FixedDocumentDefaults.DefaultImageColorspace, inputData);
			if (array != null)
			{
				byte[] array2 = DctDecode.Encode(this.Width, this.Height, (float)this.imageQuality, FixedDocumentDefaults.DefaultSMaskColorspace, array);
				ColorSpaceBase colorSpaceBase = new DeviceGray();
				this.SMask.Value = new ImageSource(array2, 8, this.Width, this.Height, colorSpaceBase, FixedDocumentDefaults.DefaultImageFilters, null, false);
				return;
			}
			this.SMask.ClearValue();
		}

		static void GetData(BitmapSource bitmapSource, out byte[] data, out byte[] alpha)
		{
			Guard.ThrowExceptionIfNull<BitmapSource>(bitmapSource, "bitmapSource");
			int num = bitmapSource.PixelHeight * bitmapSource.PixelWidth;
			data = new byte[num * 3];
			alpha = new byte[num];
			bool flag = false;
			int num2 = 0;
			foreach (int pixel in ImageSourceExtensions.GetPixels(bitmapSource))
			{
				byte b;
				byte b2;
				byte b3;
				byte b4;
				Color.GetComponentsFromPixel(pixel, out b, out b2, out b3, out b4);
				data[3 * num2] = b2;
				data[3 * num2 + 1] = b3;
				data[3 * num2 + 2] = b4;
				alpha[num2] = b;
				num2++;
				if (b != 255)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				alpha = null;
			}
		}

		void EnsureImageInfo()
		{
			if (!this.isInitialized)
			{
				this.InitializeImageInfoFromUnknownData(this.unknownImageData, this.imageQuality);
			}
		}

		void InitializeImageInfo(EncodedImageData encodedImageData)
		{
			this.InitializeImageInfo(ImageSource.CreateColorSpace(encodedImageData.ColorSpace), encodedImageData.Filters, encodedImageData.Width, encodedImageData.Height, encodedImageData.BitsPerComponent, encodedImageData.DecodeArray, false);
		}

		void InitializeImageInfo(BitmapSource bitmapSource)
		{
			if (bitmapSource.PixelWidth == 0 || bitmapSource.PixelHeight == 0)
			{
				throw new NotSupportedException("Not supported image format. PixelWidth and PixelHeight must be positive numbers!");
			}
			this.InitializeImageInfo(ImageSource.DefaultColorSpace, FixedDocumentDefaults.DefaultImageFilters, bitmapSource.PixelWidth, bitmapSource.PixelHeight, 8, null, false);
		}

		void InitializeImageInfo(ColorSpaceBase colorSpace, string[] filters, int width, int height, int bitsPerComponent, double[] decodeArray, bool imageMask = false)
		{
			this.filters = filters ?? new string[0];
			this.width = width;
			this.height = height;
			if (bitsPerComponent > 0)
			{
				this.bitsPerComponent.Value = bitsPerComponent;
			}
			else
			{
				this.bitsPerComponent.ClearValue();
			}
			if (colorSpace != null && !imageMask)
			{
				this.colorSpace.Value = colorSpace;
			}
			else
			{
				this.colorSpace.ClearValue();
			}
			this.defaultDecodeArray = decodeArray;
			this.imageMask.Value = imageMask;
			if (imageMask)
			{
				this.bitsPerComponent.Value = 1;
			}
			this.isInitialized = true;
		}

		void InitializeImageInfoFromUnknownData(byte[] unknownData, ImageQuality imageQuality)
		{
			this.DoOnUnknownData(unknownData, imageQuality, new Action<EncodedImageData>(this.InitializeImageInfo), new Action<BitmapSource>(this.InitializeImageInfo));
		}

		void CalculateEncodedDataFromUnknownData(byte[] unknownData, ImageQuality imageQuality)
		{
			this.DoOnUnknownData(unknownData, imageQuality, new Action<EncodedImageData>(this.InitializeEncodedData), new Action<BitmapSource>(this.InitializeEncodedData));
		}

		void DoOnUnknownData(byte[] unknownData, ImageQuality imageQuality, Action<EncodedImageData> doOnEncodedData, Action<BitmapSource> doOnBitmapSource)
		{
			this.imageQuality = imageQuality;
			EncodedImageData obj;
			if (imageQuality == ImageQuality.High && EncodedImageData.TryCreateFromUnknownImageData(unknownData, out obj))
			{
				doOnEncodedData(obj);
				return;
			}
			if (ImageSource.CheckIfJpeg2000(unknownData))
			{
				throw new NotSupportedException("Cannot export Jpeg2000 image with ImageQuality different than High!");
			}
			BitmapSource obj2 = ImageSourceExtensions.CreateBitmapSource(new MemoryStream(unknownData));
			doOnBitmapSource(obj2);
		}

		static ColorSpaceBase CreateColorSpace(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			if (name != null)
			{
				if (name == "DeviceRGB")
				{
					return new DeviceRgb();
				}
				if (name == "DeviceCMYK")
				{
					return new DeviceCmyk();
				}
				if (name == "DeviceGray")
				{
					return new DeviceGray();
				}
			}
			throw new ArgumentException(string.Format("The color space with name {0} is not supported.", name));
		}

		const int OpaqueAlpha = 255;

		internal const int DefaultBitsPerComponent = 8;

		internal static readonly ColorSpaceBase DefaultColorSpace = new DeviceRgb();

		readonly int id;

		readonly byte[] unknownImageData;

		readonly PdfProperty<ImageMask> mask;

		readonly PdfProperty<ImageSource> sMask;

		readonly PdfProperty<ColorSpaceBase> colorSpace;

		readonly PdfProperty<int> bitsPerComponent;

		readonly PdfProperty<double[]> decodeArray;

		readonly PdfProperty<bool> imageMask;

		double[] defaultDecodeArray;

		int width;

		int height;

		string[] filters;

		byte[] encodedImageData;

		bool isInitialized;

		ImageQuality imageQuality;
	}
}
