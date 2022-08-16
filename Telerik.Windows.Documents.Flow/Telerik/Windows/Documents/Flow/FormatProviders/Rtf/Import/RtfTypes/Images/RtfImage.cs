using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images
{
	class RtfImage : RtfImageBase
	{
		public RtfImage()
		{
			this.ScaleX = 1.0;
			this.ScaleY = 1.0;
		}

		public float Width { get; set; }

		public float Height { get; set; }

		public double DesiredWidth { get; set; }

		public double DesiredHeight { get; set; }

		public double ScaleX { get; set; }

		public double ScaleY { get; set; }

		public string Extension { get; set; }

		public string Data { get; set; }

		public byte[] BinaryData { get; set; }

		public bool IsWordArtShape { get; set; }
	}
}
