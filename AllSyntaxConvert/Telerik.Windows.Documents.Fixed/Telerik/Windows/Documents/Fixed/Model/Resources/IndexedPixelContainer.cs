using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class IndexedPixelContainer : PixelContainer
	{
		public override ImageDecodedDataType DataType
		{
			get
			{
				return ImageDecodedDataType.Indexed;
			}
		}

		public override int PixelCount
		{
			get
			{
				return base.Array.Length;
			}
		}

		public IEnumerable<Color> Palette
		{
			get
			{
				return this.palette.Colors;
			}
		}

		public IndexedPixelContainer(int pixelCount, Color[] palette)
			: base(pixelCount)
		{
			this.palette = new Palette(palette);
			this.arrayIndex = 0;
		}

		public override void Add(Color color)
		{
			byte indexOfColorInPalette = this.GetIndexOfColorInPalette(color);
			base.Array[this.arrayIndex] = indexOfColorInPalette;
			this.arrayIndex++;
		}

		public override Color GetColorFromIndex(int index)
		{
			return this.GetColorFromPalette(base.Array[index]);
		}

		public override void SetColorToIndex(Color color, int index)
		{
			base.Array[index] = this.GetIndexOfColorInPalette(color);
		}

		byte GetIndexOfColorInPalette(Color color)
		{
			return (byte)this.palette.GetIndexOfColor(color);
		}

		Color GetColorFromPalette(byte index)
		{
			return this.palette.GetColorAtIndex((int)index);
		}

		readonly Palette palette;

		int arrayIndex;
	}
}
