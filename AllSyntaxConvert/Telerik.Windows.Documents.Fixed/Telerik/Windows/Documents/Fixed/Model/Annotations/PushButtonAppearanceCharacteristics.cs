using System;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class PushButtonAppearanceCharacteristics : ButtonAppearanceCharacteristics
	{
		public PushButtonAppearanceCharacteristics()
		{
			this.iconFitOptions = new IconFitOptions();
			this.IconAndCaptionPosition = IconAndCaptionPosition.NoIconCaptionOnly;
		}

		public PushButtonAppearanceCharacteristics(PushButtonAppearanceCharacteristics other)
			: base(other)
		{
			this.MouseOverCaption = other.MouseOverCaption;
			this.MouseDownCaption = other.MouseDownCaption;
			this.NormalIconSource = other.NormalIconSource;
			this.MouseOverIconSource = other.MouseOverIconSource;
			this.MouseDownIconSource = other.MouseDownIconSource;
			this.iconFitOptions = new IconFitOptions(other.IconFitOptions);
			this.IconAndCaptionPosition = other.IconAndCaptionPosition;
		}

		public string MouseOverCaption { get; set; }

		public string MouseDownCaption { get; set; }

		public FormSource NormalIconSource { get; set; }

		public FormSource MouseOverIconSource { get; set; }

		public FormSource MouseDownIconSource { get; set; }

		public IconFitOptions IconFitOptions
		{
			get
			{
				return this.iconFitOptions;
			}
		}

		public IconAndCaptionPosition IconAndCaptionPosition { get; set; }

		readonly IconFitOptions iconFitOptions;
	}
}
