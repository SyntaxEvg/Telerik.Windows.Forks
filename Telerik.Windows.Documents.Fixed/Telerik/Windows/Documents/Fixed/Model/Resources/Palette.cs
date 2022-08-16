using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Resources
{
	class Palette
	{
		public Palette(Color[] colors)
		{
			this.colorToIndex = new Dictionary<Color, int>();
			this.indexToColor = colors;
			for (int i = 0; i < colors.Length; i++)
			{
				this.colorToIndex[colors[i]] = i;
			}
		}

		public IEnumerable<Color> Colors
		{
			get
			{
				return this.indexToColor;
			}
		}

		public Color GetColorAtIndex(int index)
		{
			return this.indexToColor[index];
		}

		public int GetIndexOfColor(Color color)
		{
			return this.colorToIndex[color];
		}

		readonly Dictionary<Color, int> colorToIndex;

		readonly Color[] indexToColor;
	}
}
