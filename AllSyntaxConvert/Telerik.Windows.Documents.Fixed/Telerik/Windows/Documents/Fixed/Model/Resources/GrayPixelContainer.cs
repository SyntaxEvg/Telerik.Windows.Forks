using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class GrayPixelContainer : PixelContainer
	{
		public override ImageDecodedDataType DataType
		{
			get
			{
				return ImageDecodedDataType.Gray;
			}
		}

		public override int PixelCount
		{
			get
			{
				return base.Array.Length;
			}
		}

		public GrayPixelContainer(int pixelCount)
			: base(pixelCount)
		{
			this.index = 0;
		}

		public override void Add(Color color)
		{
			byte grayComponent = color.GetGrayComponent();
			base.Array[this.index] = grayComponent;
			this.index++;
		}

		public override Color GetColorFromIndex(int index)
		{
			byte gray = base.Array[index];
			return Color.FromGray(gray);
		}

		public override void SetColorToIndex(Color color, int index)
		{
			base.Array[index] = color.GetGrayComponent();
		}

		int index;
	}
}
