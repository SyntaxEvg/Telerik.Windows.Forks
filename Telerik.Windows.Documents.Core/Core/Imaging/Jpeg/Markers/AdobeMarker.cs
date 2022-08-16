using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class AdobeMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return 14;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.APP14;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			decoder.Reader.Read16();
			byte[] buffer = new byte[5];
			decoder.Reader.Read(buffer, 5);
			decoder.Reader.Read16();
			decoder.Reader.Read16();
			decoder.Reader.Read16();
			decoder.ColorTransform = (JpegColorSpace)decoder.Reader.Read8();
		}
	}
}
