using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class DeviceGrayOld : ColorSpaceOld
	{
		public DeviceGrayOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override int ComponentCount
		{
			get
			{
				return 1;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return ColorSpace.Gray;
			}
		}

		public override Color GetColor(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return ColorSpaceOld.GetGrayColor(pars);
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return ColorSpaceOld.GetGrayColor(bytes, offset);
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceGray.GetDefaultDecodeArray();
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return new SolidColorBrush(this.GetColor(pars));
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			return DeviceGray.GetGrayPixels(image, applyMask);
		}
	}
}
