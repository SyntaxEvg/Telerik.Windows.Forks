using System;
using System.Globalization;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Export
{
	class ImageExporter
	{
		public ImageExporter(RtfDocumentExporter rtfExporter)
		{
			this.rtfExporter = rtfExporter;
			this.writer = this.rtfExporter.Writer;
		}

		public void ExportImage(Image image)
		{
			using (this.writer.WriteGroup("shppict", true))
			{
				this.ExportImageInternal(image, true);
			}
			if (this.rtfExporter.Settings.ExportImagesInCompatibilityMode)
			{
				using (this.writer.WriteGroup("nonshppict", false))
				{
					this.ExportImageInternal(image, true);
				}
			}
		}

		public void ExportFloatingImage(FloatingImage floatingImage)
		{
			using (this.writer.WriteGroup("shp", false))
			{
				using (this.writer.WriteGroup("shpinst", true))
				{
					this.ExportShapeInstructions(floatingImage);
				}
				using (this.writer.WriteGroup("shprslt", false))
				{
					this.writer.WriteTag("par");
					this.writer.WriteTag("pard");
					this.ExportImage(floatingImage.Image);
					this.writer.WriteTag("par");
				}
			}
		}

		public void WriteImageDataProperty(Image image)
		{
			using (this.writer.WriteGroup("sp", false))
			{
				using (this.writer.WriteGroup("sn", false))
				{
					this.writer.WriteText("pib");
				}
				using (this.writer.WriteGroup("sv", false))
				{
					this.ExportImageInternal(image, false);
				}
			}
		}

		public void WriteShapeProperty(string propertyName, int value)
		{
			this.WriteShapeProperty(propertyName, value.ToString());
		}

		public void WriteShapeProperty(string propertyName, string value)
		{
			using (this.writer.WriteGroup("sp", false))
			{
				using (this.writer.WriteGroup("sn", false))
				{
					this.writer.WriteText(propertyName);
				}
				using (this.writer.WriteGroup("sv", false))
				{
					this.writer.WriteText(value);
				}
			}
		}

		void ExportImageInternal(Image imageInline, bool exportRotation)
		{
			using (this.writer.WriteGroup("pict", false))
			{
				Size orginalSize = RtfHelper.EnsureShapeSize(imageInline.Size);
				using (this.writer.WriteGroup("picprop", true))
				{
					if (exportRotation && imageInline.RotationAngle != 0.0)
					{
						this.WriteShapeProperty("rotation", RtfHelper.AngleToRtfValue(imageInline.RotationAngle));
						orginalSize = RtfHelper.CalculateImageSizeForRotateAngle(orginalSize, imageInline.RotationAngle);
					}
					if (imageInline.IsHorizontallyFlipped)
					{
						this.WriteShapeProperty("fFlipH", 1);
					}
					if (imageInline.IsVerticallyFlipped)
					{
						this.WriteShapeProperty("fFlipV", 1);
					}
				}
				this.writer.WriteTag("picw", (int)Math.Round(orginalSize.Width));
				this.writer.WriteTag("pich", (int)Math.Round(orginalSize.Height));
				this.writer.WriteTag("picwgoal", Unit.DipToTwipI(orginalSize.Width));
				this.writer.WriteTag("pichgoal", Unit.DipToTwipI(orginalSize.Height));
				byte[] bytes = ((imageInline.ImageSource != null && imageInline.ImageSource.Data != null) ? ((byte[])imageInline.ImageSource.Data.Clone()) : new byte[0]);
				string text = ((imageInline.ImageSource != null && imageInline.ImageSource.Extension != null) ? imageInline.ImageSource.Extension : string.Empty);
				string key;
				switch (key = text.ToLower(CultureInfo.InvariantCulture).TrimStart(new char[] { '.' }))
				{
				case "png":
					this.writer.WriteTag("pngblip");
					goto IL_2B5;
				case "jpg":
				case "jpeg":
					this.writer.WriteTag("jpegblip");
					goto IL_2B5;
				case "emf":
					this.writer.WriteTag("emfblip");
					goto IL_2B5;
				case "pict":
					this.writer.WriteTag("macpict");
					goto IL_2B5;
				case "wmf":
					this.writer.WriteTag("wmetafile", 8);
					goto IL_2B5;
				case "bmp":
					this.writer.WriteTag("pngblip");
					goto IL_2B5;
				}
				this.writer.WriteTag("wbitmap", 0);
				IL_2B5:
				this.writer.WriteHex(bytes);
			}
		}

		void ExportShapeInstructions(FloatingImage imageBlock)
		{
			Size orginalSize = RtfHelper.EnsureShapeSize(imageBlock.Image.Size);
			orginalSize = RtfHelper.CalculateImageSizeForRotateAngle(orginalSize, imageBlock.Image.RotationAngle);
			this.writer.WriteTag("shpleft", Unit.DipToTwipI(imageBlock.HorizontalPosition.Offset));
			this.writer.WriteTag("shptop", Unit.DipToTwipI(imageBlock.VerticalPosition.Offset));
			this.writer.WriteTag("shpright", Unit.DipToTwipI(imageBlock.HorizontalPosition.Offset + orginalSize.Width));
			this.writer.WriteTag("shpbottom", Unit.DipToTwipI(imageBlock.VerticalPosition.Offset + orginalSize.Height));
			this.writer.WriteTag("shpfhdr", 0);
			switch (imageBlock.HorizontalPosition.RelativeFrom)
			{
			case HorizontalRelativeFrom.Column:
				this.writer.WriteTag("shpbxcolumn");
				goto IL_122;
			case HorizontalRelativeFrom.Page:
				this.writer.WriteTag("shpbxpage");
				goto IL_122;
			}
			this.writer.WriteTag("shpbxmargin");
			IL_122:
			this.writer.WriteTag("shpbxignore");
			switch (imageBlock.VerticalPosition.RelativeFrom)
			{
			case VerticalRelativeFrom.Paragraph:
				this.writer.WriteTag("shpbypara");
				goto IL_190;
			case VerticalRelativeFrom.Page:
				this.writer.WriteTag("shpbypage");
				goto IL_190;
			}
			this.writer.WriteTag("shpbymargin");
			IL_190:
			this.writer.WriteTag("shpbyignore");
			this.writer.WriteTag("shpwr", RtfHelper.WrappingStyleMapper.GetFromValue(imageBlock.Wrapping.WrappingType));
			this.writer.WriteTag("shpwrk", RtfHelper.TextWrapMapper.GetFromValue(imageBlock.Wrapping.TextWrap));
			this.writer.WriteTag("shpfblwtxt", imageBlock.IsBehindDocument ? 1 : 0);
			this.writer.WriteTag("shpz", imageBlock.ZIndex);
			this.WriteShapeProperty("shapeType", RtfHelper.ImageShapeType);
			if (imageBlock.Image.RotationAngle != 0.0)
			{
				this.WriteShapeProperty("rotation", RtfHelper.AngleToRtfValue(imageBlock.Image.RotationAngle));
			}
			this.WriteImageDataProperty(imageBlock.Image);
			this.WriteShapeProperty("pictureBiLevel", 0);
			this.WriteShapeProperty("fFilled", 0);
			if (imageBlock.HorizontalPosition.ValueType == PositionValueType.Alignment)
			{
				this.WriteShapeProperty("posh", RtfHelper.RelativeHorizontalAlignmentMapper.GetFromValue(imageBlock.HorizontalPosition.Alignment));
			}
			this.WriteShapeProperty("posrelh", RtfHelper.HorizontalRelativeFromMapper.GetFromValue(imageBlock.HorizontalPosition.RelativeFrom));
			if (imageBlock.VerticalPosition.ValueType == PositionValueType.Alignment)
			{
				this.WriteShapeProperty("posv", RtfHelper.RelativeVerticalAlignmentMapper.GetFromValue(imageBlock.VerticalPosition.Alignment));
			}
			this.WriteShapeProperty("posrelv", RtfHelper.VerticalRelativeFromMapper.GetFromValue(imageBlock.VerticalPosition.RelativeFrom));
			this.WriteShapeProperty("fAllowOverlap", imageBlock.AllowOverlap ? 1 : 0);
			this.WriteShapeProperty("fBehindDocument", imageBlock.IsBehindDocument ? 1 : 0);
			this.WriteShapeProperty("fLayoutInCell", imageBlock.LayoutInCell ? 1 : 0);
			this.WriteShapeProperty("dhgt", imageBlock.ZIndex);
			this.WriteShapeProperty("dxWrapDistLeft", Unit.DipToEmuI(imageBlock.Margin.Left));
			this.WriteShapeProperty("dyWrapDistTop", Unit.DipToEmuI(imageBlock.Margin.Top));
			this.WriteShapeProperty("dxWrapDistRight", Unit.DipToEmuI(imageBlock.Margin.Right));
			this.WriteShapeProperty("dyWrapDistBottom", Unit.DipToEmuI(imageBlock.Margin.Bottom));
		}

		readonly RtfDocumentExporter rtfExporter;

		readonly RtfWriter writer;
	}
}
