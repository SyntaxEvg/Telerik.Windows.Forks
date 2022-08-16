using System;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Data;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class DynamicAppearanceCharacteristics
	{
		public DynamicAppearanceCharacteristics()
		{
			this.Rotation = Rotation.Rotate0;
		}

		public DynamicAppearanceCharacteristics(DynamicAppearanceCharacteristics other)
		{
			this.Rotation = other.Rotation;
			if (other.BorderColor != null)
			{
				this.BorderColor = new RgbColor(other.BorderColor);
			}
			if (other.Background != null)
			{
				this.Background = new RgbColor(other.Background);
			}
		}

		public Rotation Rotation { get; set; }

		public RgbColor BorderColor { get; set; }

		public RgbColor Background { get; set; }
	}
}
