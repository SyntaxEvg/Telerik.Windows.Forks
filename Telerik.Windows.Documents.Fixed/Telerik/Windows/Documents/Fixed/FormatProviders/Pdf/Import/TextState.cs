using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Text;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import
{
	class TextState
	{
		public TextState()
		{
		}

		public TextState(TextState other)
		{
			this.Font = other.Font;
			this.FontSize = other.FontSize;
			this.TextRise = other.TextRise;
			this.HorizontalScaling = other.HorizontalScaling;
			this.CharacterSpacing = other.CharacterSpacing;
			this.WordSpacing = other.WordSpacing;
			this.RenderingMode = other.RenderingMode;
			this.TextLead = other.TextLead;
			this.TextMatrix = other.TextMatrix;
			this.TextLineMatrix = other.TextLineMatrix;
		}

		public Matrix TextMatrix { get; set; }

		public Matrix TextLineMatrix { get; set; }

		public double TextLead { get; set; }

		public double? TextRise { get; set; }

		public double? HorizontalScaling { get; set; }

		public double? CharacterSpacing { get; set; }

		public double? WordSpacing { get; set; }

		public RenderingMode RenderingMode { get; set; }

		public double FontSize { get; set; }

		public FontObject Font { get; set; }
	}
}
