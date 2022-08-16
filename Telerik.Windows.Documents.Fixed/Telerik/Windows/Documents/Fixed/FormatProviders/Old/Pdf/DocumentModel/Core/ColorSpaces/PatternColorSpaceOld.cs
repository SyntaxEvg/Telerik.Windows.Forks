using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class PatternColorSpaceOld : ColorSpaceOld
	{
		public PatternColorSpaceOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public override int ComponentCount
		{
			get
			{
				return this.UnderlyingColorSpace.ComponentCount;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return ColorSpace.Pattern;
			}
		}

		public ColorSpaceOld UnderlyingColorSpace { get; set; }

		public override Color GetColor(object[] pars)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return null;
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor xImage, bool applyMask)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<PdfResourceOld>(resources, "resources");
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			int num = pars.Length - 1;
			PdfNameOld key = (PdfNameOld)pars[num];
			PatternOld pattern = resources.GetPattern(key);
			pattern.UnderlyingColorSpace = this.UnderlyingColorSpace;
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = pars[i];
			}
			return pattern.CreateBrush(array);
		}
	}
}
