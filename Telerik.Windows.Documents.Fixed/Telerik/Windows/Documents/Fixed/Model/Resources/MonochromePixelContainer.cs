using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class MonochromePixelContainer : PixelContainer
	{
		public override int PixelCount
		{
			get
			{
				return this.width * this.height;
			}
		}

		public IEnumerable<Color> Palette
		{
			get
			{
				yield return this.firstColor;
				yield return this.secondColor;
				yield break;
			}
		}

		public override ImageDecodedDataType DataType
		{
			get
			{
				return ImageDecodedDataType.Monochrome;
			}
		}

		public MonochromePixelContainer(int width, int height, Color firstColor, Color secondColor)
			: base(BitWriter.CalculateNumberOfBytes(width, 1) * height)
		{
			this.width = width;
			this.height = height;
			this.firstColor = firstColor;
			this.secondColor = secondColor;
			this.reader = new ImageBitReader(base.Array, 1, width);
			this.writer = new ImageBitWriter(base.Array, 1, width);
		}

		public override Color GetColorFromIndex(int index)
		{
			int bitAt = this.reader.GetBitAt(index);
			return (bitAt == 0) ? this.firstColor : this.secondColor;
		}

		public override void SetColorToIndex(Color color, int index)
		{
			int monochromeColorValue = this.GetMonochromeColorValue(color);
			this.writer.WriteBitsToIndex(monochromeColorValue, index);
		}

		public override void Add(Color color)
		{
			int monochromeColorValue = this.GetMonochromeColorValue(color);
			this.writer.Write(monochromeColorValue);
		}

		int GetMonochromeColorValue(Color color)
		{
			if (!(color == this.firstColor))
			{
				return 1;
			}
			return 0;
		}

		public static readonly Color DefaultFirstColor = Color.Black;

		public static readonly Color DefaultSecondColor = Color.White;

		readonly Color firstColor;

		readonly Color secondColor;

		readonly int width;

		readonly int height;

		readonly ImageBitWriter writer;

		readonly ImageBitReader reader;
	}
}
