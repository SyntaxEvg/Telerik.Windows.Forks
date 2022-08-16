using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.Model.Resources;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Extensions
{
	public static class ImageSourceExtensions
	{
		public static Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource ToImageSource(this BitmapSource bitmapSource)
		{
			Guard.ThrowExceptionIfNull<BitmapSource>(bitmapSource, "bitmapSource");
			return bitmapSource.ToImageSource(FixedDocumentDefaults.ImageQuality);
		}

		public static Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource ToImageSource(this BitmapSource bitmapSource, ImageQuality imageQuality)
		{
			Guard.ThrowExceptionIfNull<BitmapSource>(bitmapSource, "bitmapSource");
			return new Telerik.Windows.Documents.Fixed.Model.Resources.ImageSource(bitmapSource, imageQuality);
		}

		internal static BitmapSource ToBitmapSource(this ImageDataSource dataSource, SizeI? modifiedSize = null)
		{
			BitmapSource result;
			if (dataSource.IsEmpty)
			{
				result = null;
			}
			else if (dataSource.ImageDataType == ImageDataType.Jpeg)
			{
				JpegImageDataSource source = (JpegImageDataSource)dataSource;
				result = ImageSourceExtensions.CreateImageSourceFromEncodedJpeg(source, modifiedSize);
			}
			else
			{
				if (dataSource.ImageDataType != ImageDataType.Decoded)
				{
					throw new NotSupportedException();
				}
				DecodedImageDataSource source2 = (DecodedImageDataSource)dataSource;
				result = ImageSourceExtensions.CreateImageSourceFromDecodedData(source2, modifiedSize);
			}
			return result;
		}

		internal static BitmapSource CreateBitmapSource(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			stream.Seek(0L, SeekOrigin.Begin);
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = stream;
			bitmapImage.CreateOptions = BitmapCreateOptions.None;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		internal static Stream CreateImageStream(BitmapSource bitmapSource)
		{
			PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
			pngBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
			MemoryStream memoryStream = new MemoryStream();
			pngBitmapEncoder.Save(memoryStream);
			return memoryStream;
		}

		internal static IEnumerable<int> GetCmykPixels(BitmapSource source)
		{
			int pixelWidth = source.PixelWidth;
			int pixelHeight = source.PixelHeight;
			BitmapSource source2 = ((source.Format == PixelFormats.Cmyk32) ? source : new FormatConvertedBitmap(source, PixelFormats.Cmyk32, null, 0.0));
			return ImageSourceExtensions.EnumerateBitmapPixels(source2);
		}

		internal static IEnumerable<int> GetPixels(BitmapSource source)
		{
			if (source.Format == PixelFormats.Bgr32 || source.Format == PixelFormats.Bgra32 || source.Format == PixelFormats.Pbgra32)
			{
				return ImageSourceExtensions.EnumerateBitmapPixels(source);
			}
			if (source.Format == PixelFormats.Indexed8)
			{
				return ImageSourceExtensions.EnumeratePixelsFromIndexedBitmap(source);
			}
			FormatConvertedBitmap source2 = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0.0);
			return ImageSourceExtensions.EnumerateBitmapPixels(source2);
		}

		internal static BitmapSource GetBitmapSourceFromPixels(int width, int height, int[] pixels)
		{
			double dpiX = 96.0;
			double dpiY = 96.0;
			PixelFormat bgra = PixelFormats.Bgra32;
			int num = (bgra.BitsPerPixel + 7) / 8;
			int stride = num * width;
			return BitmapSource.Create(width, height, dpiX, dpiY, bgra, null, pixels, stride);
		}

		static BitmapSource CreateImageSourceFromEncodedJpeg(JpegImageDataSource source, SizeI? modifiedSize = null)
		{
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = new MemoryStream(source.Data);
			if (modifiedSize != null)
			{
				bitmapImage.DecodePixelWidth = modifiedSize.Value.Width;
				bitmapImage.DecodePixelHeight = modifiedSize.Value.Height;
			}
			bitmapImage.EndInit();
			bitmapImage.Freeze();
			return bitmapImage;
		}

		private static global::System.Windows.Media.Imaging.BitmapSource CreateImageSourceFromDecodedData(global::Telerik.Windows.Documents.Fixed.Model.Resources.DecodedImageDataSource source, global::Telerik.Windows.Documents.Primitives.SizeI? modifiedSize = null)
		{
			global::System.Windows.Media.Imaging.BitmapSource bitmapSource;
			if (source.Data.DataType == global::Telerik.Windows.Documents.Fixed.Model.Resources.ImageDecodedDataType.Monochrome)
			{
				global::Telerik.Windows.Documents.Fixed.Model.Resources.MonochromePixelContainer monochromePixelContainer = (global::Telerik.Windows.Documents.Fixed.Model.Resources.MonochromePixelContainer)source.Data;
				global::System.Collections.Generic.List<global::System.Windows.Media.Color> colors = monochromePixelContainer.Palette.Select((global::Telerik.Windows.Documents.Core.Imaging.Color c) => global::System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B)).ToList<global::System.Windows.Media.Color>();
				global::System.Windows.Media.Imaging.BitmapPalette palette = new global::System.Windows.Media.Imaging.BitmapPalette(colors);
				bitmapSource = global::System.Windows.Media.Imaging.BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, global::System.Windows.Media.PixelFormats.Indexed1, palette, source.Data.Array, (source.Width + 7) / 8);
			}
			else if (source.Data.DataType == global::Telerik.Windows.Documents.Fixed.Model.Resources.ImageDecodedDataType.Indexed)
			{
				global::Telerik.Windows.Documents.Fixed.Model.Resources.IndexedPixelContainer indexedPixelContainer = (global::Telerik.Windows.Documents.Fixed.Model.Resources.IndexedPixelContainer)source.Data;
				global::System.Collections.Generic.List<global::System.Windows.Media.Color> colors2 = indexedPixelContainer.Palette.Select((global::Telerik.Windows.Documents.Core.Imaging.Color c) => global::System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B)).ToList<global::System.Windows.Media.Color>();
				global::System.Windows.Media.Imaging.BitmapPalette palette2 = new global::System.Windows.Media.Imaging.BitmapPalette(colors2);
				bitmapSource = global::System.Windows.Media.Imaging.BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, global::System.Windows.Media.PixelFormats.Indexed8, palette2, source.Data.Array, source.Width);
			}
			else if (source.Data.DataType == global::Telerik.Windows.Documents.Fixed.Model.Resources.ImageDecodedDataType.Gray)
			{
				bitmapSource = global::System.Windows.Media.Imaging.BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, global::System.Windows.Media.PixelFormats.Gray8, null, source.Data.Array, source.Width);
			}
			else
			{
				bitmapSource = global::System.Windows.Media.Imaging.BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, global::System.Windows.Media.PixelFormats.Bgra32, null, source.Data.Array, source.Width * 4);
			}
			global::System.Windows.Media.Imaging.BitmapSource bitmapSource2 = bitmapSource;
			if (modifiedSize != null)
			{
				global::System.Windows.Media.DrawingVisual drawingVisual = new global::System.Windows.Media.DrawingVisual();
				global::System.Windows.Media.DrawingContext drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawImage(bitmapSource2, new global::System.Windows.Rect(0.0, 0.0, (double)modifiedSize.Value.Width, (double)modifiedSize.Value.Height));
				drawingContext.Close();
				global::System.Windows.Media.Imaging.RenderTargetBitmap renderTargetBitmap = new global::System.Windows.Media.Imaging.RenderTargetBitmap(modifiedSize.Value.Width, modifiedSize.Value.Height, 96.0, 96.0, global::System.Windows.Media.PixelFormats.Pbgra32);
				renderTargetBitmap.Render(drawingVisual);
				bitmapSource2 = renderTargetBitmap;
			}
			bitmapSource2.Freeze();
			return bitmapSource2;
		}

		static IEnumerable<int> EnumerateBitmapPixels(BitmapSource source)
		{
			int[] row = new int[source.PixelWidth];
			for (int h = 0; h < source.PixelHeight; h++)
			{
				source.CopyPixels(new Int32Rect(0, h, source.PixelWidth, 1), row, source.PixelWidth * 4, 0);
				for (int w = 0; w < source.PixelWidth; w++)
				{
					yield return row[w];
				}
			}
			yield break;
		}

		private static global::System.Collections.Generic.IEnumerable<int> EnumeratePixelsFromIndexedBitmap(global::System.Windows.Media.Imaging.BitmapSource source)
		{
			byte[] indices = new byte[source.PixelWidth * source.PixelHeight];
			source.CopyPixels(indices, source.PixelWidth, 0);
			for (int i = 0; i < indices.Length; i++)
			{
				System.Windows.Media.Color c = source.Palette.Colors[(int)indices[i]];
				int pixel = ((int)c.A << 24) | ((int)c.R << 16) | ((int)c.G << 8) | (int)c.B;
				yield return pixel;
			}
			yield break;
		}
	}
}
