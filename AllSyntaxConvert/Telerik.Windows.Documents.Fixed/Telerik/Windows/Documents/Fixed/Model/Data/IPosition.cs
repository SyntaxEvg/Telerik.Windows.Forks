using System;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Fixed.Model.Data
{
	public interface IPosition
	{
		Matrix Matrix { get; }

		void Scale(double scaleX, double scaleY);

		void ScaleAt(double scaleX, double scaleY, double centerX, double centerY);

		void Rotate(double angle);

		void RotateAt(double angle, double centerX, double centerY);

		void Translate(double offsetX, double offsetY);

		void Clear();

		IPosition Clone();
	}
}
