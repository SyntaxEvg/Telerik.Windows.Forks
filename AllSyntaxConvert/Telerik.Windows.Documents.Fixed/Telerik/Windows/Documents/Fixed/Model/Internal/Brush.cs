using System;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	abstract class Brush
	{
		public Brush()
		{
			this.AlphaConstant = byte.MaxValue;
		}

		public byte AlphaConstant { get; set; }

		public abstract Brush Clone();
	}
}
