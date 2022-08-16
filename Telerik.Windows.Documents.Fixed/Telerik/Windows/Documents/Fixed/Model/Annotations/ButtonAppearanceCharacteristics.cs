using System;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class ButtonAppearanceCharacteristics : DynamicAppearanceCharacteristics
	{
		public ButtonAppearanceCharacteristics()
		{
		}

		public ButtonAppearanceCharacteristics(ButtonAppearanceCharacteristics other)
			: base(other)
		{
			this.NormalCaption = other.NormalCaption;
		}

		public string NormalCaption { get; set; }
	}
}
