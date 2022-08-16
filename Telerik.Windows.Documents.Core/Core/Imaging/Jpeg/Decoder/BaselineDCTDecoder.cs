using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class BaselineDCTDecoder : ScanDecoder
	{
		public override void ResetDecoder(JpegDecoderBase decoder)
		{
		}

		public override void DecodeBlock(JpegDecoder decoder, JpegComponent component, Block block)
		{
			int num = component.DcTable.Decode(decoder.Reader);
			int num2 = ((num == 0) ? 0 : decoder.Reader.ReceiveAndExtend(num));
			block[0] = component.LastDCCoefficient + num2;
			component.UpdateLastDCCoefficient(block);
			int i = 1;
			while (i < 64)
			{
				int num3 = component.AcTable.Decode(decoder.Reader);
				int num4 = num3 & 15;
				int num5 = num3 >> 4;
				if (num4 == 0)
				{
					if (num5 < 15)
					{
						return;
					}
					i += 16;
				}
				else
				{
					i += num5;
					int index = ZigZagScan.ZigZagMap[i];
					block[index] = decoder.Reader.ReceiveAndExtend(num4);
					i++;
				}
			}
		}
	}
}
