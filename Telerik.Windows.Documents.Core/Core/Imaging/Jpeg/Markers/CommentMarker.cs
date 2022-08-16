using System;
using System.Text;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Markers
{
	class CommentMarker : JpegMarker
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
				return JpegMarkerType.COM;
			}
		}

		public string Comment { get; set; }

		public override void InterpretMarker(JpegDecoderBase decoder)
		{
			Guard.ThrowExceptionIfNull<JpegDecoderBase>(decoder, "decoder");
			this.length = decoder.Reader.Read16();
			byte[] array = new byte[(int)(this.length - 2)];
			decoder.Reader.Read(array, array.Length);
			this.Comment = Encoding.UTF8.GetString(array, 0, array.Length);
		}

		ushort length;
	}
}
