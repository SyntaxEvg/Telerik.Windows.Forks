using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public class GradientStop : IGradientStop
	{
		public GradientStop(SimpleColor color, double offset)
		{
			Guard.ThrowExceptionIfNull<SimpleColor>(color, "color");
			Guard.ThrowExceptionIfOutOfRange<double>(0.0, 1.0, offset, "offset");
			this.color = color;
			this.offset = offset;
		}

		public SimpleColor Color
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

		Color IGradientStop.Color
		{
			get
			{
				return this.Color.ToColor();
			}
		}

		double IGradientStop.Offset
		{
			get
			{
				return this.Offset;
			}
		}

		readonly SimpleColor color;

		readonly double offset;
	}
}
