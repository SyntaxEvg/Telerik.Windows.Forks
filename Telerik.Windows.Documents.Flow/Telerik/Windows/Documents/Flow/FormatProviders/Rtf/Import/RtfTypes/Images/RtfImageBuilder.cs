using System;
using System.IO;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Media;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images
{
	class RtfImageBuilder : RtfElementIteratorBase
	{
		public RtfImageBuilder(RtfImportContext context)
		{
			this.context = context;
		}

		public ImageInline ReadImage(RtfGroup group)
		{
			Util.EnsureGroupDestination(group, "pict");
			this.currentImage = new RtfImage();
			base.VisitGroupChildren(group, false);
			ImageInline result = this.CreateImageInline(this.context.Document);
			this.currentImage = null;
			return result;
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			switch (name = tag.Name)
			{
			case "defshp":
				this.currentImage.IsWordArtShape = true;
				return;
			case "pngblip":
				this.currentImage.Extension = "png";
				return;
			case "jpegblip":
				this.currentImage.Extension = "jpg";
				return;
			case "dibitmap":
			case "wbitmap":
				this.currentImage.Extension = "bmp";
				return;
			case "emfblip":
				this.currentImage.Extension = "emf";
				return;
			case "macpict":
				this.currentImage.Extension = "pict";
				return;
			case "wmetafile":
				this.currentImage.Extension = "wmf";
				return;
			case "picw":
				this.currentImage.Width = (float)tag.ValueAsNumber;
				return;
			case "pich":
				this.currentImage.Height = (float)tag.ValueAsNumber;
				return;
			case "picwgoal":
				this.currentImage.DesiredWidth = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "pichgoal":
				this.currentImage.DesiredHeight = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "picscalex":
				this.currentImage.ScaleX = (double)tag.ValueAsNumber / 100.0;
				return;
			case "picscaley":
				this.currentImage.ScaleY = (double)tag.ValueAsNumber / 100.0;
				break;
			case "shppict":
			case "nonshppict":
			case "pmmetafile":
			case "pict":
				break;

				return;
			}
		}

		protected override void DoVisitText(RtfText text)
		{
			this.currentImage.Data = text.Text;
		}

		protected override void DoVisitBinary(RtfBinary bin)
		{
			this.currentImage.BinaryData = bin.Data;
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (!(destination == "picprop"))
				{
					if (!(destination == "sp"))
					{
						if (!(destination == "shprslt"))
						{
							return;
						}
					}
					else if (this.isInInlinePictureShapeProperties)
					{
						this.currentImage.ReadInstructionGroup(group);
					}
				}
				else if (!this.isInInlinePictureShapeProperties)
				{
					this.isInInlinePictureShapeProperties = true;
					base.VisitGroupChildren(group, false);
					this.isInInlinePictureShapeProperties = false;
					return;
				}
			}
		}

		ImageInline CreateImageInline(RadFlowDocument doc)
		{
			if (this.currentImage == null)
			{
				return null;
			}
			if (this.currentImage.IsWordArtShape)
			{
				return null;
			}
			double width;
			double height;
			if (this.currentImage.Extension == "bmp" || this.currentImage.Extension == "pict")
			{
				width = Math.Max((double)this.currentImage.Width * this.currentImage.ScaleX, 1.0);
				height = Math.Max((double)this.currentImage.Height * this.currentImage.ScaleY, 1.0);
			}
			else
			{
				width = Math.Max(this.currentImage.DesiredWidth * this.currentImage.ScaleX, 1.0);
				height = Math.Max(this.currentImage.DesiredHeight * this.currentImage.ScaleY, 1.0);
			}
			ImageInline imageInline = new ImageInline(doc);
			MemoryStream memoryStream;
			if (this.currentImage.BinaryData != null)
			{
				memoryStream = new MemoryStream(this.currentImage.BinaryData);
			}
			else
			{
				memoryStream = new MemoryStream(RtfHelper.GetBytesFromHexString(this.currentImage.Data));
			}
			using (memoryStream)
			{
				imageInline.Image.ImageSource = new ImageSource(memoryStream, this.currentImage.Extension);
			}
			imageInline.Image.IsHorizontallyFlipped = this.currentImage.GetPropertyIntValue("fFlipH", 0) == 1;
			imageInline.Image.IsVerticallyFlipped = this.currentImage.GetPropertyIntValue("fFlipV", 0) == 1;
			double num = RtfHelper.RtfValueToAngle(this.currentImage.GetPropertyIntValue("rotation", 0));
			Size size = new Size(width, height);
			size = RtfHelper.CalculateImageSizeForRotateAngle(size, num);
			imageInline.Image.RotationAngle = num;
			imageInline.Image.Size = size;
			return imageInline;
		}

		readonly RtfImportContext context;

		bool isInInlinePictureShapeProperties;

		RtfImage currentImage;
	}
}
