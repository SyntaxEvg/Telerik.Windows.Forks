using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg2000.Decoder
{
	class Box
	{
		public ulong Length { get; set; }

		public string Type { get; set; }

		public byte[] Content { get; set; }
	}
}
