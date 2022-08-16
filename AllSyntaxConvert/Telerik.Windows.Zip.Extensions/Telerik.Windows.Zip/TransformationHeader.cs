using System;

namespace Telerik.Windows.Zip
{
	public class TransformationHeader
	{
		internal TransformationHeader()
		{
			this.Buffer = null;
			this.BytesToRead = 0;
		}

		public byte[] Buffer { get; set; }

		public int BytesToRead { get; set; }

		public byte[] InitData { get; set; }

		public int Length { get; internal set; }

		public bool CountHeaderInCompressedSize { get; set; }
	}
}
