using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class GradientStop
	{
		public GradientStop(double position, ThemableColor color)
		{
			Guard.ThrowExceptionIfNull<ThemableColor>(color, "color");
			this.position = position;
			this.color = color;
		}

		public double Position
		{
			get
			{
				return this.position;
			}
		}

		public ThemableColor ThemableColor
		{
			get
			{
				return this.color;
			}
		}

		readonly double position;

		readonly ThemableColor color;
	}
}
