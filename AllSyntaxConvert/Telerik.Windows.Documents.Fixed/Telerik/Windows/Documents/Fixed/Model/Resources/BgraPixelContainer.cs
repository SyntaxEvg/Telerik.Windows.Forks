using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class BgraPixelContainer : PixelContainer
	{
		public override int PixelCount
		{
			get
			{
				return base.Array.Length / 4;
			}
		}

		public override ImageDecodedDataType DataType
		{
			get
			{
				return ImageDecodedDataType.Bgra;
			}
		}

		public BgraPixelContainer(int pixelCount)
			: base(pixelCount * 4)
		{
			this.index = 0;
		}

		public override void SetColorToIndex(Color color, int index)
		{
			base.Array[index * 4] = color.B;
			base.Array[index * 4 + 1] = color.G;
			base.Array[index * 4 + 2] = color.R;
			base.Array[index * 4 + 3] = color.A;
		}

		public override Color GetColorFromIndex(int index)
		{
			return new Color(base.Array[index * 4 + 3], base.Array[index * 4 + 2], base.Array[index * 4 + 1], base.Array[index * 4]);
		}

		public override void Add(Color color)
		{
			this.Add(color.B);
			this.Add(color.G);
			this.Add(color.R);
			this.Add(color.A);
		}

		void Add(byte item)
		{
			base.Array[this.index] = item;
			this.index++;
		}

		const int BgraComponentsPerColor = 4;

		int index;
	}
}
