using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.Model;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Images
{
	class RtfShapeBuilder : RtfElementIteratorBase
	{
		public RtfShapeBuilder(RtfImportContext context)
		{
			this.context = context;
		}

		RtfImageBuilder ImageBuilder
		{
			get
			{
				return this.context.ImageBuilder;
			}
		}

		public void ReadShape(RtfGroup group, out InlineBase inline, out Watermark watermark)
		{
			Util.EnsureGroupDestination(group, "shp");
			this.currentShape = new RtfShape();
			base.VisitGroupChildren(group, false);
			if (this.IsWatermark())
			{
				watermark = this.CreateWatermark();
				inline = null;
			}
			else
			{
				watermark = null;
				inline = this.CreateInline();
			}
			this.Clear();
		}

		protected override void DoVisitTag(RtfTag tag)
		{
			string name;
			switch (name = tag.Name)
			{
			case "shptop":
				this.currentShape.Top = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "shpleft":
				this.currentShape.Left = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "shpbottom":
				this.currentShape.Bottom = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "shpright":
				this.currentShape.Right = Unit.TwipToDip((double)tag.ValueAsNumber);
				return;
			case "shpz":
				this.currentShape.ZIndex = tag.ValueAsNumber;
				return;
			case "shpfhdr":
				this.currentShape.IsInHeader = !tag.HasValue || tag.ValueAsNumber != 1;
				return;
			case "shpbxpage":
				this.currentShape.HorizontalRelativeFrom = HorizontalRelativeFrom.Page;
				return;
			case "shpbxmargin":
				this.currentShape.HorizontalRelativeFrom = HorizontalRelativeFrom.Margin;
				return;
			case "shpbxcolumn":
				this.currentShape.HorizontalRelativeFrom = HorizontalRelativeFrom.Column;
				return;
			case "shpbxignore":
				this.currentShape.IgnoreHorizontalRelativeFrom = true;
				return;
			case "shpbypage":
				this.currentShape.VerticalRelativeFrom = VerticalRelativeFrom.Page;
				return;
			case "shpbymargin":
				this.currentShape.VerticalRelativeFrom = VerticalRelativeFrom.Margin;
				return;
			case "shpbypara":
				this.currentShape.VerticalRelativeFrom = VerticalRelativeFrom.Paragraph;
				return;
			case "shpbyignore":
				this.currentShape.IgnoreVerticalRelativeFrom = true;
				return;
			case "shpwr":
				this.currentShape.Wrapping.WrappingType = RtfHelper.WrappingStyleMapper.GetToValue(tag.ValueAsNumber);
				return;
			case "shpwrk":
				this.currentShape.Wrapping.TextWrap = RtfHelper.TextWrapMapper.GetToValue(tag.ValueAsNumber);
				return;
			case "shpfblwtxt":
				this.currentShape.IsBelowText = tag.HasValue && tag.ValueAsNumber == 1;
				break;

				return;
			}
		}

		protected override void DoVisitGroup(RtfGroup group)
		{
			string destination;
			if ((destination = group.Destination) != null)
			{
				if (!(destination == "shpinst"))
				{
					if (!(destination == "sp"))
					{
						if (!(destination == "shprslt"))
						{
							return;
						}
					}
					else if (this.isInInstructions)
					{
						this.currentShape.ReadInstructionGroup(group);
					}
				}
				else if (!this.isInInstructions)
				{
					this.isInInstructions = true;
					base.VisitGroupChildren(group, false);
					this.isInInstructions = false;
					return;
				}
			}
		}

		void Clear()
		{
			this.currentShape = null;
			this.isInInstructions = false;
		}

		InlineBase CreateInline()
		{
			RtfGroup rtfGroup = null;
			this.currentShape.Properties.TryGetValue("pib", out rtfGroup);
			if (rtfGroup == null)
			{
				return null;
			}
			ImageInline imageInline = this.ImageBuilder.ReadImage(rtfGroup.Elements[1] as RtfGroup);
			if (imageInline == null)
			{
				return null;
			}
			imageInline.Image.RotationAngle = RtfHelper.RtfValueToAngle(this.currentShape.GetPropertyIntValue("rotation", 0));
			Size orginalSize = new Size(this.currentShape.Right - this.currentShape.Left, this.currentShape.Bottom - this.currentShape.Top);
			imageInline.Image.Size = RtfHelper.CalculateImageSizeForRotateAngle(orginalSize, imageInline.Image.RotationAngle);
			FloatingImage floatingImage = new FloatingImage(this.context.Document, imageInline.Image);
			floatingImage.AllowOverlap = this.currentShape.GetPropertyBoolValue("fAllowOverlap", true);
			floatingImage.IsBehindDocument = this.currentShape.IsBelowText;
			floatingImage.Wrapping = this.currentShape.Wrapping;
			floatingImage.LayoutInCell = this.currentShape.GetPropertyBoolValue("fLayoutInCell", false);
			floatingImage.Margin = new Padding(Unit.EmuToDip((double)this.currentShape.GetPropertyIntValue("dxWrapDistLeft", RtfHelper.DefaultFloatingBlockHorizontalMargin)), Unit.EmuToDip((double)this.currentShape.GetPropertyIntValue("dyWrapDistTop", 0)), Unit.EmuToDip((double)this.currentShape.GetPropertyIntValue("dxWrapDistRight", RtfHelper.DefaultFloatingBlockHorizontalMargin)), Unit.EmuToDip((double)this.currentShape.GetPropertyIntValue("dyWrapDistBottom", 0)));
			floatingImage.HorizontalPosition = this.GetHorizontalPosition();
			floatingImage.VerticalPosition = this.GetVerticalPosition();
			floatingImage.ZIndex = this.currentShape.GetPropertyIntValue("dhgt", this.currentShape.ZIndex);
			if (this.currentShape.GetPropertyIntValue("fPseudoInline", 0) == 1)
			{
				return imageInline;
			}
			return floatingImage;
		}

		VerticalPosition GetVerticalPosition()
		{
			VerticalPosition verticalPosition = new VerticalPosition();
			verticalPosition.RelativeFrom = RtfHelper.RtfValueToVerticalRelativeFrom(this.currentShape.GetPropertyIntValue("posrelv", -1), this.currentShape.VerticalRelativeFrom);
			verticalPosition.Offset = this.currentShape.Top;
			int propertyIntValue = this.currentShape.GetPropertyIntValue("posv", 0);
			if (propertyIntValue == 0)
			{
				verticalPosition.ValueType = PositionValueType.Offset;
			}
			else
			{
				verticalPosition.ValueType = PositionValueType.Alignment;
				verticalPosition.Alignment = RtfHelper.RelativeVerticalAlignmentMapper.GetToValue(propertyIntValue);
			}
			return verticalPosition;
		}

		HorizontalPosition GetHorizontalPosition()
		{
			HorizontalPosition horizontalPosition = new HorizontalPosition();
			horizontalPosition.RelativeFrom = RtfHelper.RtfValueToHorizontalRelativeFrom(this.currentShape.GetPropertyIntValue("posrelh", -1), this.currentShape.HorizontalRelativeFrom);
			horizontalPosition.Offset = this.currentShape.Left;
			int propertyIntValue = this.currentShape.GetPropertyIntValue("posh", 0);
			if (propertyIntValue == 0)
			{
				horizontalPosition.ValueType = PositionValueType.Offset;
			}
			else
			{
				horizontalPosition.ValueType = PositionValueType.Alignment;
				horizontalPosition.Alignment = RtfHelper.RelativeHorizontalAlignmentMapper.GetToValue(propertyIntValue);
			}
			return horizontalPosition;
		}

		Watermark CreateWatermark()
		{
			string propertyStringValue = this.currentShape.GetPropertyStringValue("wzName");
			if (propertyStringValue.Contains(RtfTags.TextWatermarkShapeName))
			{
				return this.CreateTextWatermark();
			}
			if (propertyStringValue.Contains(RtfTags.ImageWatermarkShapeName))
			{
				return this.CreateImageWatermark();
			}
			return null;
		}

		Watermark CreateImageWatermark()
		{
			InlineBase inlineBase = this.CreateInline();
			FloatingImage floatingImage = inlineBase as FloatingImage;
			ImageInline imageInline = inlineBase as ImageInline;
			Image image = ((floatingImage != null) ? floatingImage.Image : imageInline.Image);
			if (image != null)
			{
				return new Watermark(new ImageWatermarkSettings
				{
					Width = image.Width,
					Height = image.Height,
					ImageSource = image.ImageSource,
					Angle = image.RotationAngle
				});
			}
			throw new InvalidOperationException("Cannot construct watermark");
		}

		Watermark CreateTextWatermark()
		{
			TextWatermarkSettings textWatermarkSettings = new TextWatermarkSettings();
			string propertyStringValue = this.currentShape.GetPropertyStringValue("gtextFont");
			if (!string.IsNullOrEmpty(propertyStringValue))
			{
				textWatermarkSettings.FontFamily = new FontFamily(propertyStringValue);
			}
			textWatermarkSettings.ForegroundColor = RtfHelper.IntToColor(this.currentShape.GetPropertyIntValue("fillColor", int.MaxValue));
			string propertyStringValue2 = this.currentShape.GetPropertyStringValue("gtextUNICODE");
			if (!string.IsNullOrEmpty(propertyStringValue2))
			{
				textWatermarkSettings.Text = propertyStringValue2;
			}
			textWatermarkSettings.Angle = RtfHelper.RtfValueToAngle(this.currentShape.GetPropertyIntValue("rotation", 0));
			textWatermarkSettings.Opacity = RtfHelper.RtfFixedPointSizeToDouble(this.currentShape.GetPropertyIntValue("fillOpacity", 65536));
			textWatermarkSettings.Width = this.currentShape.Right - this.currentShape.Left;
			textWatermarkSettings.Height = this.currentShape.Bottom - this.currentShape.Top;
			return new Watermark(textWatermarkSettings);
		}

		bool IsWatermark()
		{
			string propertyStringValue = this.currentShape.GetPropertyStringValue("wzName");
			return propertyStringValue.Contains(RtfTags.TextWatermarkShapeName) || propertyStringValue.Contains(RtfTags.ImageWatermarkShapeName);
		}

		readonly RtfImportContext context;

		bool isInInstructions;

		RtfShape currentShape;
	}
}
