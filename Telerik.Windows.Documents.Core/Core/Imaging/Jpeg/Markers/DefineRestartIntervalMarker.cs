using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class DefineRestartIntervalMarker : JpegMarker
	{
		public override ushort Length
		{
			get
			{
				return 4;
			}
		}

		public override JpegMarkerType MarkerType
		{
			get
			{
				return JpegMarkerType.DRI;
			}
		}

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			decoder.Reader.Read16();
			decoder.RestartInterval = (int)decoder.Reader.Read16();
			if (decoder.RestartInterval > 0)
			{
				decoder.IsRestartIntervalDefined = true;
			}
		}
	}
}
