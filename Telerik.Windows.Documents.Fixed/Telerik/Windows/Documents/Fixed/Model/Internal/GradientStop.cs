using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	struct GradientStop : IGradientStop
	{
		public GradientStop(Color color, double offset)
		{
			this.color = color;
			this.offset = offset;
		}

		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		public double Offset
		{
			get
			{
				return this.offset;
			}
		}

		readonly Color color;

		readonly double offset;
	}
}
