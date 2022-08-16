using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class EndOfImageMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return 0;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.EOI;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
		}
	}
}
