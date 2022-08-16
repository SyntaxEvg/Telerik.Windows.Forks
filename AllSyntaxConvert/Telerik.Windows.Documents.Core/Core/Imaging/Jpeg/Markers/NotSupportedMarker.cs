using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class NotSupportedMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return this.length;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.NotSupported;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			this.length = JpegMarker.ReadLengthAndSkipData(decoder);
		}

		ushort length;
	}
}
