using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.ColorSpaces;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg
{
	public class JpegImageInfo
	{
		public int Width { get; set; }

		public int Height { get; set; }

		public JpegColorSpace ColorSpace { get; set; }

		public bool HasAdobeInvertedColors { get; set; }
	}
}
