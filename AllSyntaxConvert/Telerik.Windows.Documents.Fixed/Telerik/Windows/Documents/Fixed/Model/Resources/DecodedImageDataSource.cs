using System;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class DecodedImageDataSource : ImageDataSource<PixelContainer>
	{
		public DecodedImageDataSource(int width, int height, PixelContainer data)
			: base(width, height, data)
		{
		}

		public override ImageDataType ImageDataType
		{
			get
			{
				return ImageDataType.Decoded;
			}
		}
	}
}
