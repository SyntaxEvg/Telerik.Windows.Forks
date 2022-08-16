using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	class TextWatermarkSettings : WatermarkSettingsBase
	{
		public TextWatermarkSettings()
		{
			this.Font = FixedDocumentDefaults.Font;
		}

		public string Text { get; set; }

		public FontBase Font { get; set; }

		public RgbColor ForegroundColor { get; set; }

		public bool TrySetFont(FontFamily fontFamily)
		{
			FontBase font;
			bool flag = FontsRepository.TryCreateFont(fontFamily, FontStyles.Normal, FontWeights.Normal, out font);
			if (flag)
			{
				this.Font = font;
			}
			return flag;
		}
	}
}
