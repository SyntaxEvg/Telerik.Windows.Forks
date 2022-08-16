﻿using System;
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

		static BitmapSource CreateImageSourceFromDecodedData(DecodedImageDataSource source, SizeI? modifiedSize = null)
		{
			BitmapSource bitmapSource;
			if (source.Data.DataType == ImageDecodedDataType.Monochrome)
			{
				MonochromePixelContainer monochromePixelContainer = (MonochromePixelContainer)source.Data;
				List<Color> colors = (from c in monochromePixelContainer.Palette
					select Color.FromArgb(c.A, c.R, c.G, c.B)).ToList<Color>();
				BitmapPalette palette = new BitmapPalette(colors);
				bitmapSource = BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, PixelFormats.Indexed1, palette, source.Data.Array, (source.Width + 7) / 8);
			}
			else if (source.Data.DataType == ImageDecodedDataType.Indexed)
			{
				IndexedPixelContainer indexedPixelContainer = (IndexedPixelContainer)source.Data;
				List<Color> colors2 = (from c in indexedPixelContainer.Palette
					select Color.FromArgb(c.A, c.R, c.G, c.B)).ToList<Color>();
				BitmapPalette palette2 = new BitmapPalette(colors2);
				bitmapSource = BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, PixelFormats.Indexed8, palette2, source.Data.Array, source.Width);
			}
			else if (source.Data.DataType == ImageDecodedDataType.Gray)
			{
				bitmapSource = BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, PixelFormats.Gray8, null, source.Data.Array, source.Width);
			}
			else
			{
				bitmapSource = BitmapSource.Create(source.Width, source.Height, 96.0, 96.0, PixelFormats.Bgra32, null, source.Data.Array, source.Width * 4);
			}
			BitmapSource bitmapSource2 = bitmapSource;
			if (modifiedSize != null)
			{
				DrawingVisual drawingVisual = new DrawingVisual();
				DrawingContext drawingContext = drawingVisual.RenderOpen();
				drawingContext.DrawImage(bitmapSource2, new Rect(0.0, 0.0, (double)modifiedSize.Value.Width, (double)modifiedSize.Value.Height));
				drawingContext.Close();
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(modifiedSize.Value.Width, modifiedSize.Value.Height, 96.0, 96.0, PixelFormats.Pbgra32);
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

		static IEnumerable<int> EnumeratePixelsFromIndexedBitmap(BitmapSource source)
		{
			byte[] indices = new byte[source.PixelWidth * source.PixelHeight];
			source.CopyPixels(indices, source.PixelWidth, 0);
			for (int i = 0; i < indices.Length; i++)
			{
				Color c = source.Palette.Colors[(int)indices[i]];
				int pixel = ((int)c.A << 24) | ((int)c.R << 16) | ((int)c.G << 8) | (int)c.B;
				yield return pixel;
			}
			yield break;
		}
	}
}
