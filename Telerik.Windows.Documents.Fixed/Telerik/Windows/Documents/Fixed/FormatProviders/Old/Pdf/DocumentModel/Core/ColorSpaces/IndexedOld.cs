using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class IndexedOld : CachedColorSpaceOld, IIndexedColorSpace, IColorSpace
	{
		public IndexedOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public ColorSpaceOld Base { get; set; }

		public int HiVal { get; set; }

		public LookupOld Lookup { get; set; }

		public override Brush DefaultBrush
		{
			get
			{
				return this.Base.DefaultBrush;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return ColorSpace.Indexed;
			}
		}

		public override int ComponentCount
		{
			get
			{
				return 1;
			}
		}

		public Color GetColor(int index)
		{
			if (this.Base == null || this.Lookup == null)
			{
				return Color.Transparent;
			}
			if (index < 0)
			{
				index = 0;
			}
			if (index > this.HiVal)
			{
				index = this.HiVal;
			}
			ColorSpaceOld colorSpace = this.GetColorSpace();
			return colorSpace.GetColor(this.Lookup.Data, index * colorSpace.ComponentCount);
		}

		public Color[] GetAllColors()
		{
			return Indexed.GetAllColors(this);
		}

		public void Init(PdfArrayOld array)
		{
			this.Base = array.GetElement<ColorSpaceOld>(1, Converters.ColorSpaceConverter);
			int hiVal;
			array.TryGetInt(2, out hiVal);
			this.HiVal = hiVal;
			this.Lookup = array.GetElement<LookupOld>(3, Converters.LookupConverter);
			base.IsLoaded = true;
		}

		protected override Color GetColorOverride(object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			int index;
			Helper.UnboxInt(pars[pars.Length - 1], out index);
			return this.GetColor(index);
		}

		public override Color GetColor(byte[] bytes, int offset)
		{
			throw new NotSupportedException("This method is not supported.");
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return new SolidColorBrush(this.GetColor(pars));
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return Indexed.GetIndexedDecodeArray(bitsPerComponent);
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			return Indexed.GetIndexedPixels(image, this, applyMask);
		}

		ColorSpaceOld GetColorSpace()
		{
			ColorSpaceOld colorSpaceOld = this.Base;
			ICCBasedOld iccbasedOld = colorSpaceOld as ICCBasedOld;
			if (iccbasedOld != null)
			{
				colorSpaceOld = iccbasedOld.Profile.Alternate;
			}
			return colorSpaceOld;
		}
	}
}
