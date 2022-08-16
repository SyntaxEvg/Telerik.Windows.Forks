using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	class ImageMaskOld : XImage, IMask
	{
		internal ImageMaskOld(PdfContentManager contentManager, byte[] decodedMaskData)
			: base(contentManager)
		{
			this.decodedMaskData = decodedMaskData;
		}

		public bool IsColorKeyMask
		{
			get
			{
				return false;
			}
		}

		public SizeI GetMaskedImageSize(IImageDescriptor image)
		{
			return Telerik.Windows.Documents.Fixed.Model.Resources.ImageMask.GetMaskedImageSize(this, image);
		}

		public PixelContainer MaskImage(IImageDescriptor image, PixelContainer pixels)
		{
			return Telerik.Windows.Documents.Fixed.Model.Resources.ImageMask.MaskImage(this, image, pixels);
		}

		public bool ShouldMaskColorComponents(int[] components)
		{
			return false;
		}

		public override void Load(PdfDictionaryOld dictionary)
		{
			base.Load(dictionary);
		}

		protected override byte[] ReadData(bool shouldDecode)
		{
			if (shouldDecode)
			{
				return this.decodedMaskData;
			}
			return base.ReadData(shouldDecode);
		}

		readonly byte[] decodedMaskData;
	}
}
