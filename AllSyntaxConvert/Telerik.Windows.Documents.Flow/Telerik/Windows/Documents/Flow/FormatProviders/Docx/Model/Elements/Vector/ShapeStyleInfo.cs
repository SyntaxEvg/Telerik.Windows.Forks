using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Vector
{
	class ShapeStyleInfo
	{
		public ShapeStyleInfo()
		{
		}

		public ShapeStyleInfo(double width, double height, double rotationAngle)
		{
			this.Width = width;
			this.Height = height;
			this.RotationAngle = rotationAngle;
		}

		public double Width { get; set; }

		public double Height { get; set; }

		public double RotationAngle { get; set; }
	}
}
