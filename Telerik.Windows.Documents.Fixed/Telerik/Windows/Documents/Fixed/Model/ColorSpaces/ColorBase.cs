using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public abstract class ColorBase : IEquatable<ColorBase>
	{
		internal abstract ColorSpaceBase ColorSpace { get; }

		internal abstract bool IsTransparent { get; }

		public abstract bool Equals(ColorBase other);

		internal abstract Color ToColor();
	}
}
