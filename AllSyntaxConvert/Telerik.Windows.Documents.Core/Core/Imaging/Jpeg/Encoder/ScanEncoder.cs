using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	abstract class ScanEncoder
	{
		public static ScanEncoder GetEncoder(JpegEncodingType encodingType)
		{
			switch (encodingType)
			{
			case JpegEncodingType.BaselineDct:
				return new BaselineDCTEncoder();
			}
			return new NotSupportedScanEncoder();
		}

		public abstract void EncodeBlock(JpegEncoder encoder, JpegComponent component, Block block);

		public abstract void PrepareQuantizationTables(JpegEncoder encoder);

		public abstract void PrepareHuffmanTables(JpegEncoder jpegEncoder);

		public abstract void ResetEncoder(JpegEncoder encoder);
	}
}
