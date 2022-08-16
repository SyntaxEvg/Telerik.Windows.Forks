using System;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class RunRenderable : IRenderable
	{
		public string Text { get; set; }

		public SolidColorBrush Foreground { get; set; }

		public FontFamily FontFamily { get; set; }

		public double? FontSize { get; set; }

		public override string ToString()
		{
			return string.Format("RunRenderable: Text={0}, Foreground={1}, FontFamily={2}, FontSize={3}", new object[] { this.Text, this.Foreground, this.FontFamily, this.FontSize });
		}
	}
}
