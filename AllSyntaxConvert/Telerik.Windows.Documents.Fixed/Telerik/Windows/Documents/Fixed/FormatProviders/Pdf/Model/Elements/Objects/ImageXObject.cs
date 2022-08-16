using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects
{
	class ImageXObject : XObjectBase
	{
		byte[] EncodedImageData
		{
			get
			{
				if (this.encodedImageData == null && this.stream != null)
				{
					this.encodedImageData = this.stream.ReadRawPdfData();
				}
				return this.encodedImageData;
			}
			set
			{
				this.encodedImageData = value;
			}
		}

		public ImageXObject()
		{
			this.width = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Width", true));
			this.height = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("Height", true));
			this.colorSpace = base.RegisterDirectProperty<ColorSpaceObject>(new PdfPropertyDescriptor("ColorSpace"));
			this.bitsPerComponent = base.RegisterDirectProperty<PdfInt>(new PdfPropertyDescriptor("BitsPerComponent"));
			this.decode = base.RegisterDirectProperty<PdfArray>(new PdfPropertyDescriptor("Decode"));
			this.imageMask = base.RegisterDirectProperty<PdfBool>(new PdfPropertyDescriptor("ImageMask"));
			this.mask = base.RegisterReferenceProperty<Mask>(new PdfPropertyDescriptor("Mask"));
			this.sMask = base.RegisterReferenceProperty<ImageXObject>(new PdfPropertyDescriptor("SMask"));
			this.filters = FixedDocumentDefaults.DefaultImageFilters.ToPdfArray();
		}

		public PdfInt Width
		{
			get
			{
				return this.width.GetValue();
			}
			set
			{
				this.width.SetValue(value);
			}
		}

		public PdfInt Height
		{
			get
			{
				return this.height.GetValue();
			}
			set
			{
				this.height.SetValue(value);
			}
		}

		public ColorSpaceObject ColorSpace
		{
			get
			{
				return this.colorSpace.GetValue();
			}
			set
			{
				this.colorSpace.SetValue(value);
			}
		}

		public PdfInt BitsPerComponent
		{
			get
			{
				return this.bitsPerComponent.GetValue();
			}
			set
			{
				this.bitsPerComponent.SetValue(value);
			}
		}

		public PdfArray Decode
		{
			get
			{
				return this.decode.GetValue();
			}
			set
			{
				this.decode.SetValue(value);
			}
		}

		public PdfBool ImageMask
		{
			get
			{
				return this.imageMask.GetValue();
			}
			set
			{
				this.imageMask.SetValue(value);
			}
		}

		public Mask Mask
		{
			get
			{
				return this.mask.GetValue();
			}
			set
			{
				this.mask.SetValue(value);
			}
		}

		public ImageXObject SMask
		{
			get
			{
				return this.sMask.GetValue();
			}
			set
			{
				this.sMask.SetValue(value);
			}
		}

		public override XObjectType XObjectType
		{
			get
			{
				return XObjectType.Image;
			}
		}

		public override void Write(PdfWriter writer, IPdfExportContext context)
		{
			Guard.ThrowExceptionIfNull<PdfWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			if (this.ExportAs == PdfElementType.PdfStreamObject)
			{
				byte[] array = this.GetData(context);
				base.Filters = this.GetExportFilters(context);
				if (this.ShouldEncryptData)
				{
					array = context.EncryptStream(array);
				}
				base.Length = new PdfInt(array.Length);
				Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject.WritePdfPropertiesDictionary(this, writer, context, true);
				writer.WriteLine();
				writer.WriteLine("stream");
				writer.Write(array);
				writer.WriteLine();
				writer.Write("endstream");
				return;
			}
			Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common.PdfObject.WritePdfPropertiesDictionary(this, writer, context, true);
		}

		public void CopyPropertiesFrom(IPdfExportContext context, ImageSource imageSource)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<ImageSource>(imageSource, "imageSource");
			this.Width = new PdfInt(imageSource.Width);
			this.Height = new PdfInt(imageSource.Height);
			this.EncodedImageData = imageSource.GetEncodedImageData(context.Settings.ImageQuality);
			this.filters = imageSource.Filters.ToPdfArray();
			this.ColorSpace = imageSource.ColorSpace.ToPrimitive((ColorSpaceBase colorSpace) => ColorSpaceManager.CreateColorSpaceObject(colorSpace), null);
			this.ImageMask = imageSource.ImageMask.ToPrimitive((bool imageMask) => new PdfBool(imageMask), null);
			this.BitsPerComponent = imageSource.BitsPerComponent.ToPrimitive((int bits) => new PdfInt(bits), null);
			this.Mask = imageSource.Mask.ToPrimitive((ImageMask imageMask) => ImageXObject.CalculateMask(imageMask, context), null);
			this.SMask = imageSource.SMask.ToPrimitive((ImageSource sMask) => ImageXObject.CalculateSMask(sMask, context), null);
			base.DecodeParms = DecodeParametersConverter.CreateDecodeParms(imageSource.DecodeParameters);
			if (imageSource.DecodeArray != null)
			{
				this.Decode = imageSource.DecodeArray.ToPdfArray();
			}
		}

		internal ImageSource ToImageSource()
		{
			int num = ((this.BitsPerComponent != null) ? this.BitsPerComponent.Value : 0);
			ColorSpaceBase colorSpaceBase = ((this.ColorSpace != null) ? this.ColorSpace.ToColorSpace() : null);
			string[] array = ((base.Filters != null) ? base.Filters.ToStringArray() : new string[0]);
			double[] decodeArray = ((this.Decode != null) ? this.Decode.ToDoubleArray() : null);
			bool flag = this.ImageMask != null && this.ImageMask.Value;
			ImageSource imageSource = new ImageSource(this.EncodedImageData, num, this.Width.Value, this.Height.Value, colorSpaceBase, array, decodeArray, flag);
			imageSource.DecodeParameters = this.stream.DecodeParameters;
			this.Mask.CopyToProperty(imageSource.Mask, new Func<Mask, ImageMask>(Mask.CreateImageMask));
			this.SMask.CopyToProperty(imageSource.SMask, (ImageXObject smask) => smask.ToImageSource());
			return imageSource;
		}

		protected override byte[] GetData(IPdfExportContext context)
		{
			byte[] result = this.EncodedImageData;
			this.EncodedImageData = null;
			return result;
		}

		protected override void InterpretData(PostScriptReader reader, IPdfImportContext context, PdfStreamBase stream)
		{
			Guard.ThrowExceptionIfNull<PdfStreamBase>(stream, "stream");
			this.stream = stream;
		}

		protected override PdfArray GetExportFilters(IPdfExportContext context)
		{
			if (this.stream != null)
			{
				return (PdfArray)this.stream.Filters;
			}
			return this.filters;
		}

		protected override Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.PdfObject CreateEncodeObject(IPdfExportContext context)
		{
			ColorSpace colorSpace = ((this.ColorSpace != null) ? this.ColorSpace.Public : Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.ColorSpace.Undefined);
			return new Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters.PdfObject(context, this.Width.Value, this.Height.Value, colorSpace);
		}

		static Mask CalculateMask(ImageMask imageMask, IPdfExportContext context)
		{
			Mask mask;
			if (imageMask == null)
			{
				mask = null;
			}
			else if (imageMask.IsColorKeyMask)
			{
				mask = new Mask(imageMask.ColorMask.Value.ToPdfArray());
			}
			else
			{
				mask = new Mask();
				mask.CopyPropertiesFrom(context, imageMask.Image.Value);
				mask.ImageMask = new PdfBool(true);
			}
			return mask;
		}

		static ImageXObject CalculateSMask(ImageSource sMask, IPdfExportContext context)
		{
			ImageXObject result;
			if (sMask == null)
			{
				result = null;
			}
			else
			{
				ResourceEntry resource = context.GetResource(sMask);
				result = (ImageXObject)resource.Resource.Content;
			}
			return result;
		}

		public const string ImageMaskPropertyName = "ImageMask";

		readonly DirectProperty<PdfInt> width;

		readonly DirectProperty<PdfInt> height;

		readonly DirectProperty<ColorSpaceObject> colorSpace;

		readonly DirectProperty<PdfInt> bitsPerComponent;

		readonly DirectProperty<PdfArray> decode;

		readonly DirectProperty<PdfBool> imageMask;

		readonly ReferenceProperty<Mask> mask;

		readonly ReferenceProperty<ImageXObject> sMask;

		PdfStreamBase stream;

		byte[] encodedImageData;

		PdfArray filters;
	}
}
