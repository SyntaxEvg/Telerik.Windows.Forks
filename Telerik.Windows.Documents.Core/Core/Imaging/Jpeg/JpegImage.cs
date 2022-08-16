using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg
{
	class JpegImage
	{
		public JpegImage(int width, int height, JpegColorSpace colorTransform, byte[] byteRaster)
		{
			Guard.ThrowExceptionIfNull<byte[]>(byteRaster, "byteRaster");
			this.width = width;
			this.height = height;
			this.colorTransform = colorTransform;
			this.numberOfComponents = JpegImage.GetNumberOfComponents(colorTransform);
			this.raster = this.CreateEmptyRaster();
			Action<byte[], int, int> action;
			if (colorTransform != JpegColorSpace.YCbCr)
			{
				if (colorTransform != JpegColorSpace.Grayscale)
				{
					if (colorTransform != JpegColorSpace.Rgb)
					{
						throw new NotSupportedException(string.Format("Not supported colorspace: {0}", colorTransform));
					}
					action = new Action<byte[], int, int>(this.SetRgbPixel);
				}
				else
				{
					action = new Action<byte[], int, int>(this.SetGrayscalePixel);
				}
			}
			else
			{
				action = new Action<byte[], int, int>(this.SetYCbCrPixel);
			}
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					action(byteRaster, i, j);
				}
			}
		}

		internal JpegImage(int width, int height, JpegColorSpace colorTransform, byte[][,] raster)
		{
			Guard.ThrowExceptionIfNull<byte[][,]>(raster, "raster");
			this.width = width;
			this.height = height;
			this.colorTransform = colorTransform;
			this.raster = raster;
			this.numberOfComponents = this.raster.Length;
		}

		public JpegColorSpace ColorTransform
		{
			get
			{
				return this.colorTransform;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
		}

		public int NumberOfComponents
		{
			get
			{
				return this.numberOfComponents;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public byte[][,] Raster
		{
			get
			{
				return this.raster;
			}
		}

		public static int GetNumberOfComponents(JpegColorSpace colorSpace)
		{
			if (colorSpace <= JpegColorSpace.Grayscale)
			{
				switch (colorSpace)
				{
				case JpegColorSpace.Unknown:
					goto IL_37;
				case JpegColorSpace.YCbCr:
					break;
				case JpegColorSpace.YCCK:
					return 4;
				default:
					if (colorSpace != JpegColorSpace.Grayscale)
					{
						goto IL_37;
					}
					return 1;
				}
			}
			else if (colorSpace != JpegColorSpace.Rgb)
			{
				if (colorSpace == JpegColorSpace.Cmyk)
				{
					return 4;
				}
				if (colorSpace != JpegColorSpace.Undefined)
				{
					goto IL_37;
				}
				goto IL_37;
			}
			return 3;
			IL_37:
			throw new NotSupportedException("Color space is not supported.");
		}

		public int[] GetPixels(JpegColorSpace colorSpace)
		{
			if (colorSpace == JpegColorSpace.Grayscale)
			{
				return this.GetGrayscalePixels();
			}
			if (colorSpace == JpegColorSpace.Rgb)
			{
				return this.GetRgbPixels();
			}
			if (colorSpace != JpegColorSpace.Cmyk)
			{
				throw new NotSupportedException("Color space is not supported.");
			}
			return this.GetCmykPixels();
		}

		public byte[] GetData()
		{
			JpegColorSpace jpegColorSpace = this.ColorTransform;
			switch (jpegColorSpace)
			{
			case JpegColorSpace.Unknown:
				return this.GetDefaultData();
			case JpegColorSpace.YCbCr:
				return this.GetRgbData();
			case JpegColorSpace.YCCK:
				return this.GetCmykData();
			default:
				if (jpegColorSpace != JpegColorSpace.Undefined)
				{
					throw new NotSupportedException("Color space is not supported.");
				}
				switch (this.raster.Length)
				{
				case 1:
					return this.GetDefaultData();
				case 3:
					return this.GetRgbData();
				case 4:
					return this.GetCmykData();
				}
				throw new NotSupportedException("Color space is not supported.");
			}
		}

		int[] GetGrayscalePixels()
		{
			int[] array = new int[this.width * this.height];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					array[num++] = ColorsHelper.ToPixel(ColorsHelper.FromGray(this.raster[0][i, j]));
				}
			}
			return array;
		}

		int[] GetCmykPixels()
		{
			int[] array = new int[this.width * this.height];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					byte cyan = this.raster[0][i, j];
					byte magenta = this.raster[1][i, j];
					byte yellow = this.raster[2][i, j];
					byte black = this.raster[3][i, j];
					YCbCrK.ToCmyk(ref cyan, ref magenta, ref yellow, ref black);
					array[num++] = ColorsHelper.ToPixel(ColorsHelper.FromCmyk(cyan, magenta, yellow, black));
				}
			}
			return array;
		}

		int[] GetRgbPixels()
		{
			int[] array = new int[this.width * this.height];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					byte r = this.raster[0][i, j];
					byte g = this.raster[1][i, j];
					byte b = this.raster[2][i, j];
					YCbCr.ToRgb(ref r, ref g, ref b);
					array[num++] = ColorsHelper.ToPixel(System.Windows.Media.Color.FromArgb(byte.MaxValue, r, g, b));
				}
			}
			return array;
		}

		byte[] GetDefaultData()
		{
			byte[] array = new byte[this.width * this.height * this.raster.Length];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					for (int k = 0; k < this.raster.Length; k++)
					{
						array[num++] = this.raster[k][i, j];
					}
				}
			}
			return array;
		}

		byte[] GetCmykData()
		{
			byte[] array = new byte[this.width * this.height * 4];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					byte b = this.raster[0][i, j];
					byte b2 = this.raster[1][i, j];
					byte b3 = this.raster[2][i, j];
					byte b4 = this.raster[3][i, j];
					YCbCrK.ToCmyk(ref b, ref b2, ref b3, ref b4);
					array[num++] = b;
					array[num++] = b2;
					array[num++] = b3;
					array[num++] = b4;
				}
			}
			return array;
		}

		byte[] GetRgbData()
		{
			byte[] array = new byte[this.width * this.height * 3];
			int num = 0;
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					byte b = this.raster[0][i, j];
					byte b2 = this.raster[1][i, j];
					byte b3 = this.raster[2][i, j];
					YCbCr.ToRgb(ref b, ref b2, ref b3);
					array[num++] = b;
					array[num++] = b2;
					array[num++] = b3;
				}
			}
			return array;
		}

		void SetRgbPixel(byte[] fromByteRaster, int row, int column)
		{
			byte pixelComponent = this.GetPixelComponent(fromByteRaster, row, column, 0);
			byte pixelComponent2 = this.GetPixelComponent(fromByteRaster, row, column, 1);
			byte pixelComponent3 = this.GetPixelComponent(fromByteRaster, row, column, 2);
			YCbCr.FromRgb(ref pixelComponent, ref pixelComponent2, ref pixelComponent3);
			this.raster[0][row, column] = pixelComponent;
			this.raster[1][row, column] = pixelComponent2;
			this.raster[2][row, column] = pixelComponent3;
		}

		void SetYCbCrPixel(byte[] fromByteRaster, int row, int column)
		{
			this.raster[0][row, column] = this.GetPixelComponent(fromByteRaster, row, column, 0);
			this.raster[1][row, column] = this.GetPixelComponent(fromByteRaster, row, column, 1);
			this.raster[2][row, column] = this.GetPixelComponent(fromByteRaster, row, column, 2);
		}

		void SetGrayscalePixel(byte[] fromByteRaster, int row, int column)
		{
			this.raster[0][row, column] = this.GetPixelComponent(fromByteRaster, row, column, 0);
		}

		byte GetPixelComponent(byte[] byteRaster, int row, int column, int componentId)
		{
			return byteRaster[(row * this.width + column) * this.numberOfComponents + componentId];
		}

		byte[][,] CreateEmptyRaster()
		{
			byte[][,] array = new byte[this.numberOfComponents][,];
			for (int i = 0; i < this.numberOfComponents; i++)
			{
				array[i] = new byte[this.height, this.width];
			}
			return array;
		}

		readonly byte[][,] raster;

		readonly int width;

		readonly int height;

		readonly int numberOfComponents;

		readonly JpegColorSpace colorTransform;
	}
}
