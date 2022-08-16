using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class GradientFillRenderable : IRenderable
	{
		public GradientFill GradientFill { get; set; }

		public ThemeColorScheme ColorScheme { get; set; }

		public Rect BoundingRectangle { get; set; }

		public override string ToString()
		{
			return string.Format("GradientFillRenderable: Rect={0}, GradientType={1}", this.BoundingRectangle, this.GradientFill.GradientType);
		}
	}
}
