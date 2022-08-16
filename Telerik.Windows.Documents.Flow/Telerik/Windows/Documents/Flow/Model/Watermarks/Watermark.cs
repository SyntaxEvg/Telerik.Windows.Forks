using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model.Watermarks
{
	public class Watermark
	{
		public Watermark(ImageWatermarkSettings imageWatermarkSettings)
		{
			Guard.ThrowExceptionIfNull<ImageWatermarkSettings>(imageWatermarkSettings, "imageWatermarkSettings");
			this.imageSettings = imageWatermarkSettings;
			this.imageSettings.Watermark = this;
			this.WatermarkType = WatermarkType.Image;
		}

		public Watermark(TextWatermarkSettings textWatermarkSettings)
		{
			Guard.ThrowExceptionIfNull<TextWatermarkSettings>(textWatermarkSettings, "textWatermarkSettings");
			this.textSettings = textWatermarkSettings;
			this.textSettings.Watermark = this;
			this.WatermarkType = WatermarkType.Text;
		}

		public WatermarkType WatermarkType { get; set; }

		public ImageWatermarkSettings ImageSettings
		{
			get
			{
				return this.imageSettings;
			}
		}

		public TextWatermarkSettings TextSettings
		{
			get
			{
				return this.textSettings;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
			internal set
			{
				if (value != null)
				{
					Guard.ThrowExceptionIfNotNull<RadFlowDocument>(this.document, "document");
				}
				if (this.document != null && this.ImageSettings != null)
				{
					this.ImageSettings.ReleaseImageSourceFromDocument();
				}
				this.document = value;
				if (this.document != null && this.ImageSettings != null)
				{
					this.ImageSettings.RegisterImageSourceInDocument();
				}
			}
		}

		public Watermark Clone()
		{
			if (this.WatermarkType == WatermarkType.Image)
			{
				ImageWatermarkSettings imageWatermarkSettings = this.ImageSettings.Clone();
				return new Watermark(imageWatermarkSettings);
			}
			if (this.WatermarkType == WatermarkType.Text)
			{
				TextWatermarkSettings textWatermarkSettings = this.TextSettings.Clone();
				return new Watermark(textWatermarkSettings);
			}
			throw new NotSupportedException("Unsupported watermark.");
		}

		readonly ImageWatermarkSettings imageSettings;

		readonly TextWatermarkSettings textSettings;

		RadFlowDocument document;
	}
}
