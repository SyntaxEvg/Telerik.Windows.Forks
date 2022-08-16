using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Data
{
	public class SimplePosition : IPosition
	{
		public SimplePosition()
		{
			this.InitializeDefaults();
		}

		public static SimplePosition Default
		{
			get
			{
				return new SimplePosition();
			}
		}

		public Matrix Matrix
		{
			get
			{
				Matrix m = default(Matrix).ScaleMatrixAt(this.scaleX, this.scaleY, this.scaleCenter.X, this.scaleCenter.Y);
				m = m.RotateMatrixAt(this.angle, this.rotateCenter.X, this.rotateCenter.Y);
				return m.TranslateMatrix(this.offsetX, this.offsetY);
			}
		}

		public void Scale(double scaleX, double scaleY)
		{
			this.scaleX = scaleX;
			this.scaleY = scaleY;
			this.scaleCenter = SimplePosition.defaultScaleCenter;
		}

		public void ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
		{
			this.scaleX = scaleX;
			this.scaleY = scaleY;
			this.scaleCenter = new Point(centerX, centerY);
		}

		public void Rotate(double angle)
		{
			this.angle = angle;
			this.rotateCenter = SimplePosition.defaultRotateCenter;
		}

		public void RotateAt(double angle, double centerX, double centerY)
		{
			this.angle = angle;
			this.rotateCenter = new Point(centerX, centerY);
		}

		public void Translate(double offsetX, double offsetY)
		{
			this.offsetX = offsetX;
			this.offsetY = offsetY;
		}

		public void Clear()
		{
			this.InitializeDefaults();
		}

		public IPosition Clone()
		{
			SimplePosition simplePosition = new SimplePosition();
			simplePosition.Translate(this.offsetX, this.offsetY);
			simplePosition.ScaleAt(this.scaleX, this.scaleY, this.scaleCenter.X, this.scaleCenter.Y);
			simplePosition.RotateAt(this.angle, this.rotateCenter.X, this.rotateCenter.Y);
			return simplePosition;
		}

		void InitializeDefaults()
		{
			this.offsetX = 0.0;
			this.offsetY = 0.0;
			this.scaleX = 1.0;
			this.scaleY = 1.0;
			this.angle = 0.0;
			this.rotateCenter = SimplePosition.defaultRotateCenter;
			this.scaleCenter = SimplePosition.defaultScaleCenter;
		}

		static readonly Point defaultRotateCenter = new Point(0.0, 0.0);

		static readonly Point defaultScaleCenter = new Point(0.0, 0.0);

		double offsetX;

		double offsetY;

		double scaleX;

		double scaleY;

		double angle;

		Point rotateCenter;

		Point scaleCenter;
	}
}
