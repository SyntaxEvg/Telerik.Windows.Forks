using System;
using System.Windows;

namespace Telerik.Windows.Documents.Core.Fonts.Glyphs
{
	class Glyph
	{
		internal ushort GlyphId { get; set; }

		public GlyphOutlinesCollection Outlines { get; set; }

		public double AdvancedWidth { get; set; }

		public Point HorizontalKerning { get; set; }

		public Point VerticalKerning { get; set; }

		public string Name { get; set; }
	}
}
