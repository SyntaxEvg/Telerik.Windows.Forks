using System;
using System.IO;
using BitMiracle.LibTiff.Classic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Filters
{
	class CCITTFaxDecode : IPdfFilter
	{
		public string Name
		{
			get
			{
				return "CCITTFaxDecode";
			}
		}

		public byte[] Encode(PdfObject encodedObject, byte[] data)
		{
			throw new NotSupportedException();
		}

		public byte[] Decode(PdfObject decodedObject, byte[] inputData, DecodeParameters parms)
		{
			int num = 1728;
			int num2 = 0;
			int num3 = 0;
			bool flag = false;
			if (parms != null)
			{
				if (parms.ContainsKey("Columns"))
				{
					num = (int)parms["Columns"];
				}
				if (num == 0)
				{
					num = decodedObject.Width;
				}
				if (parms.ContainsKey("Rows"))
				{
					num2 = (int)parms["Rows"];
				}
				if (num2 == 0)
				{
					num2 = decodedObject.Height;
				}
				if (parms.ContainsKey("K"))
				{
					num3 = (int)parms["K"];
				}
				if (parms.ContainsKey("BlackIs1"))
				{
					flag = (bool)parms["BlackIs1"];
				}
			}
			Compression compression;
			if (num3 < 0)
			{
				compression = Compression.CCITTFAX4;
			}
			else
			{
				if (num3 != 0)
				{
					return null;
				}
				compression = Compression.CCITTFAX3;
			}
			ColorSpaceObject colorSpaceObject = decodedObject.GetColorSpaceObject();
			if (colorSpaceObject == null)
			{
				colorSpaceObject = new DeviceGrayColorSpaceObject();
			}
			ColorSpaceBase colorSpaceBase = colorSpaceObject.ToColorSpace();
			TiffStream stream = new TiffStream();
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Tiff tiff = Tiff.ClientOpen("myTiff", "w", memoryStream, stream))
				{
					tiff.SetField(TiffTag.IMAGEWIDTH, new object[] { num });
					tiff.SetField(TiffTag.IMAGELENGTH, new object[] { num2 });
					tiff.SetField(TiffTag.COMPRESSION, new object[] { compression });
					tiff.SetField(TiffTag.BITSPERSAMPLE, new object[] { 1 });
					tiff.SetField(TiffTag.SAMPLESPERPIXEL, new object[] { 1 });
					tiff.WriteRawStrip(0, inputData, inputData.Length);
					tiff.Flush();
					tiff.FlushData();
					memoryStream.Position = 0L;
					using (Tiff tiff2 = Tiff.ClientOpen("myTiff", "r", memoryStream, stream))
					{
						int[] array = new int[num * num2];
						tiff2.ReadRGBAImage(num, num2, array);
						int num4 = (num + 7) / 8;
						byte[] array2 = new byte[num4 * num2];
						int num5 = 0;
						int num6 = 0;
						for (int i = num2 - 1; i >= 0; i--)
						{
							for (int j = 0; j < num; j++)
							{
								if (flag)
								{
									if (array[i * num + j] == -16777216)
									{
										ColorBase color = colorSpaceBase.GetColor(new double[] { 1.0 });
										RgbColor rgbColor = color as RgbColor;
										if (rgbColor != null && rgbColor.R != 0)
										{
											byte[] array3 = array2;
											int num7 = num5;
											array3[num7] |= (byte)(1 << 7 - num6);
										}
									}
									else
									{
										ColorSpaceBase colorSpaceBase2 = colorSpaceBase;
										double[] pars = new double[1];
										ColorBase color = colorSpaceBase2.GetColor(pars);
										RgbColor rgbColor2 = color as RgbColor;
										if (rgbColor2 != null && rgbColor2.R != 0)
										{
											byte[] array4 = array2;
											int num8 = num5;
											array4[num8] |= (byte)(1 << 7 - num6);
										}
									}
								}
								else if (array[i * num + j] == -1)
								{
									ColorBase color = colorSpaceBase.GetColor(new double[] { 1.0 });
									RgbColor rgbColor3 = color as RgbColor;
									if (rgbColor3 != null && rgbColor3.R != 0)
									{
										byte[] array5 = array2;
										int num9 = num5;
										array5[num9] |= (byte)(1 << 7 - num6);
									}
								}
								else
								{
									ColorSpaceBase colorSpaceBase3 = colorSpaceBase;
									double[] pars2 = new double[1];
									ColorBase color = colorSpaceBase3.GetColor(pars2);
									RgbColor rgbColor4 = color as RgbColor;
									if (rgbColor4 != null && rgbColor4.R != 0)
									{
										byte[] array6 = array2;
										int num10 = num5;
										array6[num10] |= (byte)(1 << 7 - num6);
									}
								}
								num6++;
								if (num6 == 8)
								{
									num5++;
									num6 = 0;
								}
							}
							if (num6 != 0)
							{
								num5++;
								num6 = 0;
							}
						}
						decodedObject.ColorSpace = ColorSpace.Gray;
						decodedObject.BitsPerComponent = 1;
						result = array2;
					}
				}
			}
			return result;
		}

		const int White = -1;

		const int Black = -16777216;
	}
}
