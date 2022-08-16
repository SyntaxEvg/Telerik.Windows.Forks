using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	abstract class ScanDecoder
	{
		public static ScanDecoder GetDecoder(JpegEncodingType encodingType)
		{
			switch (encodingType)
			{
			case JpegEncodingType.BaselineDct:
				return new BaselineDCTDecoder();
			case JpegEncodingType.ProgressiveDct:
				return new ProgressiveDCTDecoder();
			}
			return new NotSupportedScanDecoder();
		}

		public abstract void ResetDecoder(JpegDecoderBase decoder);

		public abstract void DecodeBlock(JpegDecoder decoder, JpegComponent component, Block block);
	}
}
