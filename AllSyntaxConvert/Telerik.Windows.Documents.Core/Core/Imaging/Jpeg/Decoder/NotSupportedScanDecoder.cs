using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;
using Telerik.Windows.Documents.Exceptions;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder
{
	class NotSupportedScanDecoder : ScanDecoder
	{
		public override void ResetDecoder(JpegDecoderBase decoder)
		{
			throw new NotSupportedScanDecoderException();
		}

		public override void DecodeBlock(JpegDecoder decoder, JpegComponent component, Block block)
		{
			throw new NotSupportedScanDecoderException();
		}
	}
}
