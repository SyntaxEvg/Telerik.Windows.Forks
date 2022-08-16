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
	class DeviceCmykOld : ColorSpaceOld
	{
		public DeviceCmykOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override Brush DefaultBrush
		{
			get
			{
				return new SolidColorBrush(Color.Black);
			}
		}

		public override int ComponentCount
		{
			get
			{
				return 4;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return ColorSpace.CMYK;
			}
		}

		public override Color GetColor(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return ColorSpaceOld.GetCmykColor(pars);
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return ColorSpaceOld.GetCmykColor(bytes, offset);
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return new SolidColorBrush(this.GetColor(pars));
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceCmyk.GetDefaultDecodeArray();
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			return DeviceCmyk.GetCmykPixels(image, applyMask);
		}
	}
}
