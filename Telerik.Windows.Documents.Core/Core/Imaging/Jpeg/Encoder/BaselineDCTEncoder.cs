using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	class BaselineDCTEncoder : ScanEncoder
	{
		public override void EncodeBlock(JpegEncoder encoder, JpegComponent component, Block block)
		{
			int num = block[0] - component.LastDCCoefficient;
			int num2 = num;
			if (num < 0)
			{
				num = -num;
				num2--;
			}
			int num3 = 0;
			while (num != 0)
			{
				num3++;
				num >>= 1;
			}
			component.DcTable.Encode(encoder.Writer, num3);
			if (num3 != 0)
			{
				encoder.Writer.WriteBits(num3, num2);
			}
			int i = 0;
			for (int j = 1; j < 64; j++)
			{
				num = block[j];
				if (num == 0)
				{
					i++;
				}
				else
				{
					while (i > 15)
					{
						component.AcTable.Encode(encoder.Writer, 240);
						i -= 16;
					}
					num2 = num;
					if (num < 0)
					{
						num = -num;
						num2--;
					}
					num3 = 1;
					while ((num >>= 1) != 0)
					{
						num3++;
					}
					int nbits = (i << 4) + num3;
					component.AcTable.Encode(encoder.Writer, nbits);
					encoder.Writer.WriteBits(num3, num2);
					i = 0;
				}
			}
			if (i > 0)
			{
				component.AcTable.Encode(encoder.Writer, 0);
			}
		}

		public override void PrepareHuffmanTables(JpegEncoder jpegEncoder)
		{
			HuffmanTable table = new HuffmanTable(TableClass.DCTable, 0, JpegEncoder.StandardDCLuminanceLengths, JpegEncoder.StandardDCLuminanceValues);
			jpegEncoder.AddHuffmanTable(table);
			HuffmanTable table2 = new HuffmanTable(TableClass.ACTable, 0, JpegEncoder.StandardACLuminanceLengths, JpegEncoder.StandardACLuminanceValues);
			jpegEncoder.AddHuffmanTable(table2);
			HuffmanTable table3 = new HuffmanTable(TableClass.DCTable, 1, JpegEncoder.StandardDCChromianceLengths, JpegEncoder.StandardDCChromianceValues);
			jpegEncoder.AddHuffmanTable(table3);
			HuffmanTable table4 = new HuffmanTable(TableClass.ACTable, 1, JpegEncoder.StandardACChromianceLengths, JpegEncoder.StandardACChromianceValues);
			jpegEncoder.AddHuffmanTable(table4);
		}

		public override void PrepareQuantizationTables(JpegEncoder encoder)
		{
			byte[] byteData = BaselineDCTEncoder.ScaleQuantizationTable(encoder.Parameters.LuminanceTable, encoder.Parameters.QuantizingQuality);
			encoder.AddQuantizationTable(new QuantizationTable(0, byteData));
			byte[] byteData2 = BaselineDCTEncoder.ScaleQuantizationTable(encoder.Parameters.ChrominanceTable, encoder.Parameters.QuantizingQuality);
			encoder.AddQuantizationTable(new QuantizationTable(1, byteData2));
		}

		public override void ResetEncoder(JpegEncoder encoder)
		{
			throw new NotImplementedException();
		}

		static byte[] ScaleQuantizationTable(byte[] inTable, float qualityScale)
		{
			byte[] array = new byte[64];
			if (qualityScale <= 0f)
			{
				qualityScale = 1f;
			}
			else if (qualityScale > 100f)
			{
				qualityScale = 100f;
			}
			else if (qualityScale < 50f)
			{
				qualityScale = 5000f / qualityScale;
			}
			else
			{
				qualityScale = 200f - qualityScale * 2f;
			}
			for (int i = 0; i < 64; i++)
			{
				long num = (long)((float)inTable[i] * qualityScale / 100f);
				num = Math.Max(num, 1L);
				num = System.Math.Min(num, 255L);
				array[i] = (byte)num;
			}
			return array;
		}

		const byte LuminanceTableIdx = 0;

		const byte ChrominanceTableIdx = 1;
	}
}
