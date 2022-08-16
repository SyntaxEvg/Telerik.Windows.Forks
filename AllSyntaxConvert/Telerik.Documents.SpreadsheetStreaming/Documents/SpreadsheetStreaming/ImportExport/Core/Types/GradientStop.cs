using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class GradientStop
	{
		public GradientStop()
		{
		}

		public GradientStop(double position, SpreadThemableColor color)
		{
			this.Position = position;
			this.ThemableColor = color;
		}

		public double Position { get; internal set; }

		public SpreadThemableColor ThemableColor { get; internal set; }
	}
}
