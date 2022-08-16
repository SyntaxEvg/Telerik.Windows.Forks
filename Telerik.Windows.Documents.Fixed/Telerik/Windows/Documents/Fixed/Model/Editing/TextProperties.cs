using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Text;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	public class TextProperties : PropertiesBase<TextProperties>
	{
		public TextProperties()
		{
			this.RenderingMode = FixedDocumentDefaults.TextRenderingMode;
			this.FontSize = FixedDocumentDefaults.FontSize;
			this.Font = FixedDocumentDefaults.Font;
			this.UnderlinePattern = FixedDocumentDefaults.UnderlinePattern;
			this.UnderlineColor = FixedDocumentDefaults.Color;
			this.BaselineAlignment = FixedDocumentDefaults.BaselineAlignment;
			this.HighlightColor = FixedDocumentDefaults.HighlightColor;
		}

		public UnderlinePattern UnderlinePattern { get; set; }

		public ColorBase UnderlineColor { get; set; }

		public ColorBase HighlightColor { get; set; }

		public double? CharacterSpacing { get; set; }

		public double? WordSpacing { get; set; }

		public double? HorizontalScaling { get; set; }

		public double FontSize { get; set; }

		public RenderingMode RenderingMode { get; set; }

		public Telerik.Windows.Documents.Fixed.Model.Editing.Flow.BaselineAlignment BaselineAlignment { get; set; }

		public FontBase Font
		{
			get
			{
				return this.font;
			}
			set
			{
				Guard.ThrowExceptionIfNull<FontBase>(value, "Font");
				this.font = value;
			}
		}

		public bool TrySetFont(FontFamily fontFamily)
		{
			return this.TrySetFont(fontFamily, FontStyles.Normal, FontWeights.Normal);
		}

		public bool TrySetFont(FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight)
		{
			FontBase fontBase;
			bool flag = FontsRepository.TryCreateFont(fontFamily, fontStyle, fontWeight, out fontBase);
			if (flag)
			{
				this.Font = fontBase;
			}
			return flag;
		}

		public override void CopyFrom(TextProperties textProperties)
		{
			this.CharacterSpacing = textProperties.CharacterSpacing;
			this.WordSpacing = textProperties.WordSpacing;
			this.HorizontalScaling = textProperties.HorizontalScaling;
			this.FontSize = textProperties.FontSize;
			this.RenderingMode = textProperties.RenderingMode;
			this.Font = textProperties.Font;
			this.UnderlinePattern = textProperties.UnderlinePattern;
			this.UnderlineColor = textProperties.UnderlineColor;
			this.HighlightColor = textProperties.HighlightColor;
		}

		internal void CopyFrom(TextPropertiesOwner textProperties)
		{
			this.CharacterSpacing = textProperties.CharacterSpacing;
			this.WordSpacing = textProperties.WordSpacing;
			this.HorizontalScaling = textProperties.HorizontalScaling;
			this.FontSize = textProperties.FontSize;
			this.RenderingMode = textProperties.RenderingMode;
			this.Font = textProperties.Font;
		}

		internal void CopyTo(TextPropertiesOwner textProperties)
		{
			textProperties.CharacterSpacing = this.CharacterSpacing;
			textProperties.WordSpacing = this.WordSpacing;
			textProperties.HorizontalScaling = this.HorizontalScaling;
			textProperties.FontSize = this.FontSize;
			textProperties.RenderingMode = this.RenderingMode;
			textProperties.Font = this.Font;
		}

		FontBase font;
	}
}
