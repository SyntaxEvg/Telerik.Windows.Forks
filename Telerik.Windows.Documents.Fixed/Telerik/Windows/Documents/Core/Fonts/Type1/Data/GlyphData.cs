using System;
using Telerik.Windows.Documents.Core.Fonts.Glyphs;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Data
{
	class GlyphData
	{
		public GlyphOutlinesCollection Oultlines { get; set; }

		public ushort AdvancedWidth { get; set; }

		public bool HasWidth { get; set; }

		public GlyphData(GlyphOutlinesCollection outlines, ushort? width)
		{
			this.Oultlines = outlines;
			if (width != null)
			{
				this.AdvancedWidth = width.Value;
				this.HasWidth = true;
			}
		}
	}
}
