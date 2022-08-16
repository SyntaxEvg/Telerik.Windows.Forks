using System;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Decoder;
using Telerik.Windows.Documents.Core.Imaging.Jpeg.Encoder;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	 abstract class JpegTable
	{
		public abstract ushort Length { get; }

		public abstract void Read(IJpegReader reader);

		public abstract void Write(JpegWriter writer);
	}
}
