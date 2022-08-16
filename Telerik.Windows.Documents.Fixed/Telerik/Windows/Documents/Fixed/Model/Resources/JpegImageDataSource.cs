using System;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class JpegImageDataSource : ImageDataSource<byte[]>
	{
		public JpegImageDataSource(int width, int height, byte[] data)
			: base(width, height, data)
		{
		}

		public override ImageDataType ImageDataType
		{
			get
			{
				return ImageDataType.Jpeg;
			}
		}
	}
}
