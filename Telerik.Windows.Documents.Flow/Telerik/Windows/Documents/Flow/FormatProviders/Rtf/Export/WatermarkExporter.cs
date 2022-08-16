using System;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class WatermarkExporter
	{
		public WatermarkExporter(RtfDocumentExporter rtfDocumentExporter, Watermark watermark)
		{
			Guard.ThrowExceptionIfNull<Watermark>(watermark, "watermark");
			this.rtfExporter = rtfDocumentExporter;
			this.watermark = watermark;
			this.writer = this.rtfExporter.Writer;
			this.imageExporter = this.rtfExporter.ImageExporter;
		}

		public void ExportWatermark()
		{
			switch (this.watermark.WatermarkType)
			{
			case WatermarkType.Image:
				this.ExportImageWatermark(this.watermark.ImageSettings);
				return;
			case WatermarkType.Text:
				this.ExportTextWatermark(this.watermark.TextSettings);
				return;
			default:
				throw new NotSupportedException("Watermark type is not supported.");
			}
		}

		void ExportTextWatermark(TextWatermarkSettings textSettings)
		{
			using (this.writer.WriteGroup("shp", false))
			{
				using (this.writer.WriteGroup("shpinst", true))
				{
					this.ExportTextShapeInstructions(textSettings);
				}
			}
		}

		void ExportImageWatermark(ImageWatermarkSettings imageSettings)
		{
			if (imageSettings.ImageSource == null)
			{
				return;
			}
			using (this.writer.WriteGroup("shp", false))
			{
				Image image = new Image();
				image.Size = new Size(imageSettings.Width, imageSettings.Height);
				image.ImageSource = imageSettings.ImageSource;
				image.RotationAngle = imageSettings.Angle;
				using (this.writer.WriteGroup("shpinst", true))
				{
					this.ExportImageShapeInstructions(image);
				}
				using (this.writer.WriteGroup("shprslt", false))
				{
					this.writer.WriteTag("par");
					this.writer.WriteTag("pard");
					this.imageExporter.ExportImage(image);
					this.writer.WriteTag("par");
				}
			}
		}

		void ExportImageShapeInstructions(Image image)
		{
			Size size = RtfHelper.CalculateImageSizeForRotateAngle(image.Size, image.RotationAngle);
			this.writer.WriteTag("shpleft", 0);
			this.writer.WriteTag("shptop", 0);
			this.writer.WriteTag("shpright", Unit.DipToTwipI(size.Width));
			this.writer.WriteTag("shpbottom", Unit.DipToTwipI(size.Height));
			this.writer.WriteTag("shpfhdr", 1);
			this.writer.WriteTag("shpbxcolumn");
			this.writer.WriteTag("shpbxignore");
			this.writer.WriteTag("shpbypara");
			this.writer.WriteTag("shpbyignore");
			this.writer.WriteTag("shpwrk", RtfHelper.TextWrapMapper.GetFromValue(TextWrap.BothSides));
			this.writer.WriteTag("shpfblwtxt", 1);
			this.writer.WriteTag("shpz", 0);
			this.writer.WriteTag("shpwr", 3);
			this.imageExporter.WriteShapeProperty("shapeType", RtfHelper.ImageShapeType);
			if (image.RotationAngle != 0.0)
			{
				this.imageExporter.WriteShapeProperty("rotation", RtfHelper.AngleToRtfValue(image.RotationAngle));
			}
			this.imageExporter.WriteImageDataProperty(image);
			this.imageExporter.WriteShapeProperty("posh", RtfHelper.RelativeHorizontalAlignmentMapper.GetFromValue(RelativeHorizontalAlignment.Center));
			this.imageExporter.WriteShapeProperty("posrelh", RtfHelper.HorizontalRelativeFromMapper.GetFromValue(HorizontalRelativeFrom.Margin));
			this.imageExporter.WriteShapeProperty("posv", RtfHelper.RelativeVerticalAlignmentMapper.GetFromValue(RelativeVerticalAlignment.Center));
			this.imageExporter.WriteShapeProperty("posrelv", RtfHelper.VerticalRelativeFromMapper.GetFromValue(VerticalRelativeFrom.Margin));
			this.imageExporter.WriteShapeProperty("fBehindDocument", 1);
			this.imageExporter.WriteShapeProperty("fLayoutInCell", 0);
			this.imageExporter.WriteShapeProperty("wzName", RtfTags.ImageWatermarkShapeName + this.rand.Next(32767));
		}

		void ExportTextShapeInstructions(TextWatermarkSettings textSettings)
		{
			Size orginalSize = new Size(textSettings.Width, textSettings.Height);
			orginalSize = RtfHelper.CalculateImageSizeForRotateAngle(orginalSize, textSettings.Angle);
			this.writer.WriteTag("shpleft", 0);
			this.writer.WriteTag("shptop", 0);
			this.writer.WriteTag("shpright", Unit.DipToTwipI(orginalSize.Width));
			this.writer.WriteTag("shpbottom", Unit.DipToTwipI(orginalSize.Height));
			this.writer.WriteTag("shpfhdr", 1);
			this.writer.WriteTag("shpbxmargin");
			this.writer.WriteTag("shpbxignore");
			this.writer.WriteTag("shpbymargin");
			this.writer.WriteTag("shpbyignore");
			this.writer.WriteTag("shpwrk", RtfHelper.TextWrapMapper.GetFromValue(TextWrap.BothSides));
			this.writer.WriteTag("shpfblwtxt", 1);
			this.writer.WriteTag("shpz", 0);
			this.writer.WriteTag("shpwr", 3);
			this.imageExporter.WriteShapeProperty("shapeType", RtfHelper.TextShapeType);
			if (textSettings.Angle != 0.0)
			{
				this.imageExporter.WriteShapeProperty("rotation", RtfHelper.AngleToRtfValue(textSettings.Angle));
			}
			this.imageExporter.WriteShapeProperty("gtextUNICODE", textSettings.Text);
			this.imageExporter.WriteShapeProperty("gtextFont", textSettings.FontFamily.Source);
			this.imageExporter.WriteShapeProperty("fillColor", RtfHelper.ColorToInt(textSettings.ForegroundColor));
			int value = (int)RtfHelper.DoubleToRtfFixedPointSize(textSettings.Opacity);
			this.imageExporter.WriteShapeProperty("fillOpacity", value);
			this.imageExporter.WriteShapeProperty("posh", RtfHelper.RelativeHorizontalAlignmentMapper.GetFromValue(RelativeHorizontalAlignment.Center));
			this.imageExporter.WriteShapeProperty("posrelh", RtfHelper.HorizontalRelativeFromMapper.GetFromValue(HorizontalRelativeFrom.Margin));
			this.imageExporter.WriteShapeProperty("posv", RtfHelper.RelativeVerticalAlignmentMapper.GetFromValue(RelativeVerticalAlignment.Center));
			this.imageExporter.WriteShapeProperty("posrelv", RtfHelper.VerticalRelativeFromMapper.GetFromValue(VerticalRelativeFrom.Margin));
			this.imageExporter.WriteShapeProperty("fBehindDocument", 1);
			this.imageExporter.WriteShapeProperty("fLayoutInCell", 0);
			this.imageExporter.WriteShapeProperty("fLine", 0);
			this.imageExporter.WriteShapeProperty("wzName", RtfTags.TextWatermarkShapeName + this.rand.Next(32767));
		}

		readonly RtfDocumentExporter rtfExporter;

		readonly Watermark watermark;

		readonly RtfWriter writer;

		readonly ImageExporter imageExporter;

		readonly Random rand = new Random();
	}
}
