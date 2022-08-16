using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class ProgressiveDCTDecoder : ScanDecoder
	{
		public override void ResetDecoder(JpegDecoderBase decoder)
		{
			this.eobrun = 0;
			this.successiveACState = 0;
			this.successiveACNextValue = 0;
			if (decoder.ScanHeader.SpectralSelectionStart == 0)
			{
				if (decoder.ScanHeader.BitPositionHigh == 0)
				{
					this.decodeBlockAction = new Action<JpegDecoder, JpegComponent, Block>(this.DecodeDCFirst);
					return;
				}
				this.decodeBlockAction = new Action<JpegDecoder, JpegComponent, Block>(this.DecodeDCSuccessive);
				return;
			}
			else
			{
				if (decoder.ScanHeader.BitPositionHigh == 0)
				{
					this.decodeBlockAction = new Action<JpegDecoder, JpegComponent, Block>(this.DecodeACFirst);
					return;
				}
				this.decodeBlockAction = new Action<JpegDecoder, JpegComponent, Block>(this.DecodeACSuccessive);
				return;
			}
		}

		public override void DecodeBlock(JpegDecoder decoder, JpegComponent component, Block block)
		{
			Guard.ThrowExceptionIfNull<Action<JpegDecoder, JpegComponent, Block>>(this.decodeBlockAction, "decodeBlockAction");
			this.decodeBlockAction(decoder, component, block);
		}

		void DecodeDCFirst(JpegDecoder decoder, JpegComponent component, Block block)
		{
			int num = component.DcTable.Decode(decoder.Reader);
			int num2 = ((num == 0) ? 0 : (decoder.Reader.ReceiveAndExtend(num) << (int)decoder.ScanHeader.BitPositionLow));
			block[0] = component.LastDCCoefficient + num2;
			component.UpdateLastDCCoefficient(block);
		}

		void DecodeDCSuccessive(JpegDecoder decoder, JpegComponent component, Block block)
		{
			block[0] = block[0] | (decoder.Reader.ReadBit() << (int)decoder.ScanHeader.BitPositionLow);
		}

		void DecodeACFirst(JpegDecoder decoder, JpegComponent component, Block block)
		{
			if (this.eobrun > 0)
			{
				this.eobrun--;
				return;
			}
			int i = (int)decoder.ScanHeader.SpectralSelectionStart;
			int spectralSelectionEnd = (int)decoder.ScanHeader.SpectralSelectionEnd;
			while (i <= spectralSelectionEnd)
			{
				int num = component.AcTable.Decode(decoder.Reader);
				int num2 = num & 15;
				int num3 = num >> 4;
				if (num2 == 0)
				{
					if (num3 < 15)
					{
						this.eobrun = decoder.Reader.Receive(num3) + (1 << num3) - 1;
						return;
					}
					i += 16;
				}
				else
				{
					i += num3;
					int index = ZigZagScan.ZigZagMap[i];
					block[index] = decoder.Reader.ReceiveAndExtend(num2) * (1 << (int)decoder.ScanHeader.BitPositionLow);
					i++;
				}
			}
		}

		void DecodeACSuccessive(JpegDecoder decoder, JpegComponent component, Block block)
		{
			int i = (int)decoder.ScanHeader.SpectralSelectionStart;
			int spectralSelectionEnd = (int)decoder.ScanHeader.SpectralSelectionEnd;
			int num = 0;
			while (i <= spectralSelectionEnd)
			{
				int num2 = ZigZagScan.ZigZagMap[i];
				switch (this.successiveACState)
				{
				case 0:
				{
					int num3 = component.AcTable.Decode(decoder.Reader);
					int num4 = num3 & 15;
					num = num3 >> 4;
					if (num4 == 0)
					{
						if (num < 15)
						{
							this.eobrun = decoder.Reader.Receive(num) + (1 << num);
							this.successiveACState = 4;
							continue;
						}
						num = 16;
						this.successiveACState = 1;
						continue;
					}
					else
					{
						if (num4 != 1)
						{
							throw new InvalidOperationException("invalid ACn encoding");
						}
						this.successiveACNextValue = decoder.Reader.ReceiveAndExtend(num4);
						this.successiveACState = ((num != 0) ? 2 : 3);
						continue;
					}
					break;
				}
				case 1:
				case 2:
					if (block[num2] != 0)
					{
						int index;
						block[index = num2] = block[index] + (decoder.Reader.ReadBit() << (int)decoder.ScanHeader.BitPositionLow);
					}
					else
					{
						num--;
						if (num == 0)
						{
							this.successiveACState = ((this.successiveACState == 2) ? 3 : 0);
						}
					}
					break;
				case 3:
					if (block[num2] != 0)
					{
						int index2;
						block[index2 = num2] = block[index2] + (decoder.Reader.ReadBit() << (int)decoder.ScanHeader.BitPositionLow);
					}
					else
					{
						block[num2] = this.successiveACNextValue << (int)decoder.ScanHeader.BitPositionLow;
						this.successiveACState = 0;
					}
					break;
				case 4:
					if (block[num2] != 0)
					{
						int index3;
						block[index3 = num2] = block[index3] + (decoder.Reader.ReadBit() << (int)decoder.ScanHeader.BitPositionLow);
					}
					break;
				}
				i++;
			}
			if (this.successiveACState == 4)
			{
				this.eobrun--;
				if (this.eobrun == 0)
				{
					this.successiveACState = 0;
				}
			}
		}

		int eobrun;

		int successiveACState;

		int successiveACNextValue;

		Action<JpegDecoder, JpegComponent, Block> decodeBlockAction;
	}
}
