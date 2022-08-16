using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Enums;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core
{
	class TextStateOld
	{
		public TextStateOld()
		{
			this.CharSpacing = 0.0;
			this.WordSpacing = 0.0;
			this.HorizontalScaling = 100.0;
			this.Leading = 0.0;
			this.RenderingMode = RenderingMode.Fill;
			this.Rise = 0.0;
		}

		public TextStateOld(TextStateOld other)
		{
			this.CharSpacing = other.CharSpacing;
			this.Font = other.Font;
			this.FontSize = other.FontSize;
			this.HorizontalScaling = other.HorizontalScaling;
			this.IsInTextMode = other.IsInTextMode;
			this.Knockout = other.Knockout;
			this.Leading = other.Leading;
			this.RenderingMode = other.RenderingMode;
			this.Rise = other.Rise;
			this.TextLineMatrix = other.TextLineMatrix;
			this.TextMatrix = other.TextMatrix;
			this.WordSpacing = other.WordSpacing;
		}

		public bool IsInTextMode { get; set; }

		public double CharSpacing { get; set; }

		public double WordSpacing { get; set; }

		public double HorizontalScaling { get; set; }

		public double Leading { get; set; }

		public FontBaseOld Font { get; set; }

		public double FontSize { get; set; }

		public RenderingMode RenderingMode { get; set; }

		public double Rise { get; set; }

		public bool Knockout { get; set; }

		public Matrix TextMatrix { get; set; }

		public Matrix TextLineMatrix { get; set; }
	}
}
