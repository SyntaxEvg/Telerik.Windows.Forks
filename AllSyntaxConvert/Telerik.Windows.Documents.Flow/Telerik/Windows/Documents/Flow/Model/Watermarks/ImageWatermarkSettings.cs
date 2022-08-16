using System;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.Model.Watermarks
{
	public class ImageWatermarkSettings : WatermarkSettingsBase
	{
		public ImageSource ImageSource
		{
			get
			{
				return this.imageSource;
			}
			set
			{
				if (this.imageSource != value)
				{
					this.ReleaseImageSourceFromDocument();
					this.imageSource = value;
					this.RegisterImageSourceInDocument();
				}
			}
		}

		public ImageWatermarkSettings Clone()
		{
			return new ImageWatermarkSettings
			{
				Angle = base.Angle,
				Width = base.Width,
				Height = base.Height,
				ImageSource = this.ImageSource.Clone()
			};
		}

		internal void ReleaseImageSourceFromDocument()
		{
			if (this.imageSource != null && base.Watermark != null && base.Watermark.Document != null)
			{
				base.Watermark.Document.Resources.ReleaseResource(this.imageSource);
			}
		}

		internal void RegisterImageSourceInDocument()
		{
			if (this.imageSource != null && base.Watermark != null && base.Watermark.Document != null)
			{
				base.Watermark.Document.Resources.RegisterResource(this.imageSource);
			}
		}

		ImageSource imageSource;
	}
}
