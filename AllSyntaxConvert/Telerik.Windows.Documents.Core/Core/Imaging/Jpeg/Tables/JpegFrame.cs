using System;

namespace Telerik.Windows.Documents.Core.Imaging.Jpeg.Tables
{
	class JpegFrame
	{
		public int Height
		{
			get
			{
				return this.height;
			}
		}

		public int Width
		{
			get
			{
				return this.width;
			}
		}

		public JpegScan Scan { get; set; }

		public JpegFrame(int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		readonly int width;

		readonly int height;
	}
}
