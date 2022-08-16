using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types
{
	class UnsignedIntHex
	{
		public UnsignedIntHex(Color color)
		{
			this.color = color;
		}

		public UnsignedIntHex(string hexString)
		{
			this.color = ColorsHelper.HexStringToColor(hexString);
		}

		public Color Color
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

		readonly Color color;
	}
}
