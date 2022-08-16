using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Watermarks
{
	public class TextWatermarkSettings : WatermarkSettingsBase
	{
		public TextWatermarkSettings()
		{
			this.Opacity = 1.0;
			this.FontFamily = DocumentDefaultStyleSettings.FontFamily.LocalValue;
		}

		public string Text { get; set; }

		public FontFamily FontFamily
		{
			get
			{
				return this.fontFamily;
			}
			set
			{
				Guard.ThrowExceptionIfNull<FontFamily>(value, "value");
				this.fontFamily = value;
			}
		}

		public Color ForegroundColor { get; set; }

		public double Opacity
		{
			get
			{
				return this.opacity;
			}
			set
			{
				Guard.ThrowExceptionIfOutOfRange<double>(0.0, 1.0, value, "value");
				this.opacity = value;
			}
		}

		public TextWatermarkSettings Clone()
		{
			return new TextWatermarkSettings
			{
				Text = this.Text,
				FontFamily = this.FontFamily,
				ForegroundColor = this.ForegroundColor,
				Opacity = this.Opacity,
				Angle = base.Angle,
				Width = base.Width,
				Height = base.Height
			};
		}

		const int DefaultOpacity = 1;

		double opacity;

		FontFamily fontFamily;
	}
}
