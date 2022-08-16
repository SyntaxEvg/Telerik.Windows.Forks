using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Watermarks
{
	public class WatermarkSettingsBase
	{
		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
			internal set
			{
				Guard.ThrowExceptionIfNotNull<Watermark>(this.watermark, "watermark");
				this.watermark = value;
			}
		}

		public double Width
		{
			get
			{
				return this.width;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				this.width = value;
			}
		}

		public double Height
		{
			get
			{
				return this.height;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
				this.height = value;
			}
		}

		public double Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				this.angle = value;
			}
		}

		Watermark watermark;

		double width;

		double height;

		double angle;
	}
}
