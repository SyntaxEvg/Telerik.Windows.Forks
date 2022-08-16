using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing.Flow
{
	public class CharacterProperties
	{
		public CharacterProperties()
		{
			this.FontSize = FixedDocumentDefaults.FontSize;
			this.Font = FixedDocumentDefaults.Font;
			this.ForegroundColor = FixedDocumentDefaults.ForegroundColor;
			this.HighlightColor = FixedDocumentDefaults.HighlightColor;
			this.BaselineAlignment = FixedDocumentDefaults.BaselineAlignment;
			this.UnderlinePattern = FixedDocumentDefaults.UnderlinePattern;
			this.UnderlineColor = FixedDocumentDefaults.ForegroundColor;
		}

		public double FontSize { get; set; }

		public FontBase Font { get; set; }

		public ColorBase ForegroundColor { get; set; }

		public ColorBase HighlightColor { get; set; }

		public BaselineAlignment BaselineAlignment { get; set; }

		public UnderlinePattern UnderlinePattern { get; set; }

		public ColorBase UnderlineColor { get; set; }

		public bool TrySetFont(FontFamily fontFamily)
		{
			return this.TrySetFont(fontFamily, FontStyles.Normal, FontWeights.Normal);
		}

		public bool TrySetFont(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight)
		{
			FontBase font;
			bool flag = FontsRepository.TryCreateFont(fontFamily, fontStyle, fontWeight, out font);
			if (flag)
			{
				this.Font = font;
			}
			return flag;
		}

		public void CopyPropertiesFrom(CharacterProperties fromProperties)
		{
			this.FontSize = fromProperties.FontSize;
			this.Font = fromProperties.Font;
			this.ForegroundColor = fromProperties.ForegroundColor;
			this.HighlightColor = fromProperties.HighlightColor;
			this.BaselineAlignment = fromProperties.BaselineAlignment;
			this.UnderlineColor = fromProperties.UnderlineColor;
			this.UnderlinePattern = fromProperties.UnderlinePattern;
		}

		internal void CopyTo(Block block)
		{
			Guard.ThrowExceptionIfNull<Block>(block, "block");
			block.TextProperties.FontSize = this.FontSize;
			block.TextProperties.Font = this.Font;
			block.TextProperties.HighlightColor = this.HighlightColor;
			block.TextProperties.BaselineAlignment = this.BaselineAlignment;
			block.TextProperties.UnderlineColor = this.UnderlineColor;
			block.TextProperties.UnderlinePattern = this.UnderlinePattern;
			block.GraphicProperties.FillColor = this.ForegroundColor;
		}
	}
}
