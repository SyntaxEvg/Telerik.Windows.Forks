using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Extensions;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	[PdfClass(TypeName = "XObject", SubtypeProperty = "Subtype", SubtypeValue = "Image")]
	class XImage : XObject, IImageDescriptor
	{
		public XImage(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.width = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Width",
				IsRequired = true
			}, Converters.PdfIntConverter);
			this.height = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "Height",
				IsRequired = true
			}, Converters.PdfIntConverter);
			this.colorSpace = base.CreateInstantLoadProperty<ColorSpaceOld>(new PdfPropertyDescriptor
			{
				Name = "ColorSpace"
			}, Converters.ColorSpaceConverter);
			this.bitsPerComponent = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "BitsPerComponent"
			}, Converters.PdfIntConverter);
			this.imageMask = base.CreateInstantLoadProperty<PdfBoolOld>(new PdfPropertyDescriptor
			{
				Name = "ImageMask"
			}, new PdfBoolOld(contentManager, false), Converters.PdfBoolConverter);
			this.softMask = base.CreateInstantLoadProperty<XImage>(new PdfPropertyDescriptor
			{
				Name = "SMask"
			});
			this.decode = base.CreateInstantLoadProperty<PdfArrayOld>(new PdfPropertyDescriptor
			{
				Name = "Decode"
			});
			this.mask = base.CreateInstantLoadProperty<PdfObjectOld>(new PdfPropertyDescriptor
			{
				Name = "Mask"
			}, Converters.MaskConverter);
		}

		public PdfIntOld Width
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

		public PdfIntOld Height
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

		public ColorSpaceOld ColorSpace
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

		public PdfIntOld BitsPerComponent
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

		public PdfBoolOld ImageMask
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

		public PdfArrayOld Decode
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

		public XImage SMask
		{
			get
			{
				return this.softMask.GetValue();
			}
			set
			{
				this.softMask.SetValue(value);
			}
		}

		public IMask Mask
		{
			get
			{
				return (IMask)this.mask.GetValue();
			}
		}

		int IImageDescriptor.Width
		{
			get
			{
				return this.Width.Value;
			}
		}

		int IImageDescriptor.Height
		{
			get
			{
				return this.Height.Value;
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
				if (this.Decode == null)
				{
					return null;
				}
				return this.Decode.ToDoubleArray();
			}
		}

		IColorSpace IImageDescriptor.ColorSpace
		{
			get
			{
				return this.ColorSpace;
			}
		}

		IImageDescriptor IImageDescriptor.SMask
		{
			get
			{
				return this.SMask;
			}
		}

		IMask IImageDescriptor.Mask
		{
			get
			{
				return this.Mask;
			}
		}

		string[] IImageDescriptor.Filters
		{
			get
			{
				string[] result = null;
				IndirectObjectOld indirectObjectOld;
				if (base.Reference != null && base.ContentManager.TryReadIndirectObject(base.Reference, true, out indirectObjectOld))
				{
					PdfDataStream pdfDataStream = (PdfDataStream)indirectObjectOld.Value;
					PdfDictionaryOld dictionary = pdfDataStream.Dictionary;
					result = FiltersManagerOld.GetFilterValues(base.ContentManager, dictionary);
				}
				return result;
			}
		}

		byte[] IImageDescriptor.GetDecodedData()
		{
			return this.ReadData(true);
		}

		byte[] IImageDescriptor.GetEncodedData()
		{
			return this.ReadData(false);
		}

		public override void Load(PdfDataStream stream)
		{
			this.Load(stream.Dictionary);
			base.IsLoaded = true;
		}

		public ImageDataSource CreateImageSource(IImageDescriptor image)
		{
			if (this.ImageMask.Value)
			{
				throw new NotSupportedException();
			}
			return image.ToImageDataSource();
		}

		public DecodedImageDataSource CreateImageSourceWithStencilColor(IImageDescriptor image, Color stencilColor)
		{
			if (!this.ImageMask.Value)
			{
				throw new NotSupportedException();
			}
			return image.ToDecodedImageDataSource(stencilColor);
		}

		protected virtual byte[] ReadData(bool shouldDecode)
		{
			byte[] result = null;
			if (base.Reference != null)
			{
				result = base.ContentManager.ReadData(base.Reference, shouldDecode);
			}
			return result;
		}

		readonly InstantLoadProperty<PdfIntOld> width;

		readonly InstantLoadProperty<PdfIntOld> height;

		readonly InstantLoadProperty<ColorSpaceOld> colorSpace;

		readonly InstantLoadProperty<PdfIntOld> bitsPerComponent;

		readonly InstantLoadProperty<PdfBoolOld> imageMask;

		readonly InstantLoadProperty<XImage> softMask;

		readonly InstantLoadProperty<PdfArrayOld> decode;

		readonly InstantLoadProperty<PdfObjectOld> mask;
	}
}
