using System;
using Telerik.Windows.Documents.Core.Imaging;

namespace Telerik.Windows.Documents.Fixed.Model.Internal
{
	class SolidColorBrush : Brush
	{
		public SolidColorBrush(Color color)
		{
			this.color = color;
		}

		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		public override Brush Clone()
		{
			return new SolidColorBrush(this.Color);
		}

		public override bool Equals(object obj)
		{
			SolidColorBrush solidColorBrush = obj as SolidColorBrush;
			return solidColorBrush != null && this.Color.Equals(solidColorBrush.Color);
		}

		public override int GetHashCode()
		{
			return 391 + this.Color.GetHashCode();
		}

		readonly Color color;
	}
}
