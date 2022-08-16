using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class UnsignedIntHex
	{
		public UnsignedIntHex(SpreadColor color)
		{
			this.color = color;
		}

		public SpreadColor Color
		{
			get
			{
				return this.color;
			}
		}

		public override string ToString()
		{
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[]
			{
				this.color.A,
				this.color.R,
				this.color.G,
				this.color.B
			});
		}

		readonly SpreadColor color;
	}
}
