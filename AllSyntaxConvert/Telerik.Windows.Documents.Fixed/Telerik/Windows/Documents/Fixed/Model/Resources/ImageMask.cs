using System;
using System.Windows;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Extensions;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class ImageMask : IMask
	{
		public ImageMask(ImageSource image)
			: this()
		{
			this.Image.Value = image;
		}

		public ImageMask(int[] colorMaskArray)
			: this()
		{
			this.ColorMask.Value = colorMaskArray;
		}

		ImageMask()
		{
			this.image = new PdfProperty<ImageSource>();
			this.colorMask = new PdfProperty<int[]>();
		}

		public PdfProperty<ImageSource> Image
		{
			get
			{
				return this.image;
			}
		}

		public PdfProperty<int[]> ColorMask
		{
			get
			{
				return this.colorMask;
			}
		}

		public bool IsColorKeyMask
		{
			get
			{
				return this.colorMask.HasValue;
			}
		}

		public byte[] Data
		{
			get
			{
				if (this.IsColorKeyMask)
				{
					return null;
				}
				IImageDescriptor value = this.Image.Value;
				return value.GetDecodedData();
			}
		}

		public SizeI GetMaskedImageSize(IImageDescriptor image)
		{
			if (this.IsColorKeyMask)
			{
				return new SizeI(image.Width, image.Height);
			}
			IImageDescriptor value = this.Image.Value;
			return ImageMask.GetMaskedImageSize(value, image);
		}

		public PixelContainer MaskImage(IImageDescriptor image, PixelContainer pixels)
		{
			if (this.IsColorKeyMask)
			{
				return pixels;
			}
			return ImageMask.MaskImage(this.Image.Value, image, pixels);
		}

		public bool ShouldMaskColorComponents(int[] components)
		{
			if (this.IsColorKeyMask)
			{
				int[] value = this.ColorMask.Value;
				for (int i = 0; i < value.Length; i += 2)
				{
					int num = components[i / 2];
					if (num < value[i] || value[i + 1] < num)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		internal static SizeI GetMaskedImageSize(IImageDescriptor mask, IImageDescriptor image)
		{
			int width = Math.Max(image.Width, mask.Width);
			int height = Math.Max(image.Height, mask.Height);
			return new SizeI(width, height);
		}

		internal static PixelContainer MaskImage(IImageDescriptor mask, IImageDescriptor image, PixelContainer pixels)
		{
			ImageMask.ImageMaskingContext imageMaskingContext = new ImageMask.ImageMaskingContext();
			imageMaskingContext.Image = image;
			imageMaskingContext.MaskedSize = ImageMask.GetMaskedImageSize(mask, image);
			imageMaskingContext.MaskScale = new Size((double)mask.Width / (double)imageMaskingContext.MaskedSize.Width, (double)mask.Height / (double)imageMaskingContext.MaskedSize.Height);
			imageMaskingContext.ImageScale = new Size((double)image.Width / (double)imageMaskingContext.MaskedSize.Width, (double)image.Height / (double)imageMaskingContext.MaskedSize.Height);
			imageMaskingContext.Pixels = pixels;
			imageMaskingContext.UnmaskedColor = ImageMask.GetUnmaskedColor(mask);
			imageMaskingContext.BitReader = new ImageBitReader(mask.GetDecodedData(), 1, mask.Width);
			if (imageMaskingContext.MaskedSize.Width == image.Width && imageMaskingContext.MaskedSize.Height == image.Height)
			{
				return ImageMask.GetMaskImageWhenSameImageSize(imageMaskingContext);
			}
			return ImageMask.GetMaskImageWhenDifferentImageSize(imageMaskingContext);
		}

		internal static int GetUnmaskedColor(IImageDescriptor image)
		{
			int result = 0;
			double[] decode = image.Decode;
			if (decode != null && decode.Length > 0)
			{
				result = (int)decode[0];
			}
			return result;
		}

		internal static bool IsMasked(ImageBitReader bitReader, int unmaskedColor, int row, int column)
		{
			int bitAt = bitReader.GetBitAt(row, column);
			return bitAt != unmaskedColor;
		}

		static PixelContainer GetMaskImageWhenSameImageSize(ImageMask.ImageMaskingContext c)
		{
			for (int i = 0; i < c.MaskedSize.Height; i++)
			{
				int num = i * c.MaskedSize.Width;
				for (int j = 0; j < c.MaskedSize.Width; j++)
				{
					int column = (int)((double)j * c.MaskScale.Width);
					int row = (int)((double)i * c.MaskScale.Height);
					int index = num + j;
					if (ImageMask.IsMasked(c.BitReader, c.UnmaskedColor, row, column))
					{
						c.Pixels.SetColorToIndex(Color.Transparent, index);
					}
				}
			}
			return c.Pixels;
		}

		static PixelContainer GetMaskImageWhenDifferentImageSize(ImageMask.ImageMaskingContext c)
		{
			PixelContainer maskedImagePixelContainer = PixelContainerFactory.GetMaskedImagePixelContainer(c.MaskedSize.Width, c.MaskedSize.Height);
			for (int i = 0; i < c.MaskedSize.Height; i++)
			{
				int num = i * c.MaskedSize.Width;
				for (int j = 0; j < c.MaskedSize.Width; j++)
				{
					int column = (int)((double)j * c.MaskScale.Width);
					int row = (int)((double)i * c.MaskScale.Height);
					int index = num + j;
					if (ImageMask.IsMasked(c.BitReader, c.UnmaskedColor, row, column))
					{
						maskedImagePixelContainer.SetColorToIndex(Color.Transparent, index);
					}
					else
					{
						int scaledPixelIndex = c.Image.GetScaledPixelIndex(j, i, c.ImageScale.Width, c.ImageScale.Height);
						Color colorFromIndex = c.Pixels.GetColorFromIndex(scaledPixelIndex);
						maskedImagePixelContainer.SetColorToIndex(colorFromIndex, index);
					}
				}
			}
			return maskedImagePixelContainer;
		}

		readonly PdfProperty<ImageSource> image;

		readonly PdfProperty<int[]> colorMask;

		class ImageMaskingContext
		{
			public int UnmaskedColor;

			public SizeI MaskedSize;

			public Size ImageScale;

			public Size MaskScale;

			public PixelContainer Pixels;

			public ImageBitReader BitReader;

			public IImageDescriptor Image;
		}
	}
}
