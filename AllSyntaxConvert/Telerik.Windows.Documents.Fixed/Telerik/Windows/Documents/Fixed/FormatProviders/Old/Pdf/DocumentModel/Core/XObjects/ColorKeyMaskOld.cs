using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.XObjects
{
	class ColorKeyMaskOld : PdfObjectOld, IMask
	{
		public ColorKeyMaskOld(PdfContentManager contentManager, PdfArrayOld componentRangesArray)
			: base(contentManager)
		{
			Guard.ThrowExceptionIfNull<PdfArrayOld>(componentRangesArray, "componentRangesArray");
			int[] array = new int[componentRangesArray.Count];
			for (int i = 0; i < componentRangesArray.Count; i++)
			{
				componentRangesArray.TryGetInt(i, out array[i]);
			}
			this.mask = new ImageMask(array);
		}

		public bool IsColorKeyMask
		{
			get
			{
				return true;
			}
		}

		public SizeI GetMaskedImageSize(IImageDescriptor image)
		{
			return new SizeI(image.Width, image.Height);
		}

		public PixelContainer MaskImage(IImageDescriptor image, PixelContainer pixels)
		{
			return pixels;
		}

		public bool ShouldMaskColorComponents(int[] components)
		{
			return this.mask.ShouldMaskColorComponents(components);
		}

		readonly ImageMask mask;
	}
}
