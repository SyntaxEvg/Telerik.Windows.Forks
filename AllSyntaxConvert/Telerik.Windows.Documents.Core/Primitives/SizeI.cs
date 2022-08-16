using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Primitives
{
	struct SizeI
	{
		public SizeI(int width, int height)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, width, "width");
			Guard.ThrowExceptionIfLessThan<int>(0, height, "height");
			this.width = width;
			this.height = height;
		}

		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(0, value, "value");
				this.width = value;
			}
		}

		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(0, value, "value");
				this.height = value;
			}
		}

		int width;

		int height;
	}
}
