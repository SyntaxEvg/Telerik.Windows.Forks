using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class RestartMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return 16;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.RST;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			decoder.Reader.Read();
			decoder.Reader.Read();
			RestartMarker.RestartDecoder(decoder);
		}

		public static void RestartDecoder(JpegDecoderBase decoder)
		{
			decoder.Reader.Restart();
			decoder.ComponentDecoder.ResetDecoder(decoder);
		}

		public static void RestartScan(JpegScan scan)
		{
			foreach (JpegComponent jpegComponent in scan.Components)
			{
				jpegComponent.Restart();
			}
		}
	}
}
