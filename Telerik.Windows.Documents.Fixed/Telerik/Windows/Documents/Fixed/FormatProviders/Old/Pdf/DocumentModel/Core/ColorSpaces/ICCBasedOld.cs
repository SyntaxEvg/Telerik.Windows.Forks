using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters;
using Telerik.Windows.Documents.Fixed.Model.Internal;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class ICCBasedOld : CachedColorSpaceOld
	{
		public ICCBasedOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.profile = base.CreateLoadOnDemandProperty<ICCProfileOld>(new PdfPropertyDescriptor
			{
				Name = "Profile"
			});
		}

		public ICCProfileOld Profile
		{
			get
			{
				return this.profile.GetValue();
			}
			set
			{
				this.profile.SetValue(value);
			}
		}

		public override int ComponentCount
		{
			get
			{
				return this.Profile.Alternate.ComponentCount;
			}
		}

		public override ColorSpace Type
		{
			get
			{
				return this.Profile.Alternate.Type;
			}
		}

		protected override Color GetColorOverride(object[] pars)
		{
			return this.Profile.Alternate.GetColor(pars);
		}

		public override Color GetColor(byte[] bytes, int index)
		{
			Guard.ThrowExceptionIfNull<byte[]>(bytes, "bytes");
			return this.Profile.Alternate.GetColor(bytes, index);
		}

		protected override PixelContainer GetPixelsOverride(IImageDescriptor image, bool applyMask)
		{
			PixelContainer pixels = this.Profile.Alternate.GetPixels(image, applyMask);
			this.Profile.Alternate.Clear();
			return pixels;
		}

		public override Brush GetBrush(PdfResourceOld resources, object[] pars)
		{
			Guard.ThrowExceptionIfNull<object[]>(pars, "pars");
			return this.Profile.Alternate.GetBrush(resources, pars);
		}

		public override double[] GetDefaultDecodeArray(int bitsPerComponent)
		{
			return this.Profile.Alternate.GetDefaultDecodeArray(bitsPerComponent);
		}

		readonly LoadOnDemandProperty<ICCProfileOld> profile;
	}
}
