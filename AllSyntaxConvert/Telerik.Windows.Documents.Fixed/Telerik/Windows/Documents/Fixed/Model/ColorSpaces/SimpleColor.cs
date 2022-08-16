using System;

namespace Telerik.Windows.Documents.Fixed.Model.ColorSpaces
{
	public abstract class SimpleColor : ColorBase
	{
		internal abstract int[] GetColorComponents();
	}
}
