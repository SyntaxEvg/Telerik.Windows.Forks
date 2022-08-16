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
	class CalRgbOld : CIEBasedOld
	{
		public CalRgbOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override int ComponentCount
		{
			get
			{
				return 3;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return ColorSpace.RGB;
			}
		}

		public override Color GetColor(object[] pars)
		{
			return ColorSpaceOld.GetRgbColor(pars);
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return ColorSpaceOld.GetRgbColor(bytes, offset);
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return DeviceRgb.GetDefaultDecodeArray();
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return new SolidColorBrush(this.GetColor(pars));
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			return DeviceRgb.GetRgbPixels(image, applyMask);
		}
	}
}
