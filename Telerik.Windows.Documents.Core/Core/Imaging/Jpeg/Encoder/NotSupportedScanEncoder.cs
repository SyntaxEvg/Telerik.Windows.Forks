using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder
{
	class NotSupportedScanEncoder : ScanEncoder
	{
		public override void EncodeBlock(JpegEncoder encoder, JpegComponent component, Block block)
		{
			throw new NotSupportedScanEncoderException();
		}

		public override void PrepareHuffmanTables(JpegEncoder jpegEncoder)
		{
			throw new NotImplementedException();
		}

		public override void PrepareQuantizationTables(JpegEncoder encoder)
		{
			throw new NotSupportedScanEncoderException();
		}

		public override void ResetEncoder(JpegEncoder encoder)
		{
			throw new NotSupportedScanEncoderException();
		}
	}
}
