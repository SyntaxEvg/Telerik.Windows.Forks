using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Model.Annotations
{
	public class PushButtonWidget : Widget<PushButtonAppearanceCharacteristics>, IContentAnnotation
	{
		internal PushButtonWidget(FormField field)
			: base(field)
		{
			this.content = new AnnotationContentSource();
		}

		public sealed override WidgetContentType WidgetContentType
		{
			get
			{
				return WidgetContentType.PushButtonContent;
			}
		}

		public AnnotationContentSource Content
		{
			get
			{
				base.EnsureContentIsUpToDate();
				return this.content;
			}
		}

		internal override Rect ButtonContentBox
		{
			get
			{
				if (base.AppearanceCharacteristics.IconFitOptions.IgnoreBorderWidth)
				{
					return base.Rect;
				}
				return base.ButtonContentBox;
			}
		}

		public sealed override void RecalculateContent()
		{
			this.Content.NormalContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawNormalContent));
			this.Content.MouseDownContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawMouseDownContent));
			this.Content.MouseOverContentSource = base.RecalculateAppearance(new Action<FixedContentEditor>(this.DrawMouseOverContent));
		}

		internal override void ConsumeImportedAppearances(AnnotationAppearances appearances)
		{
			if (appearances.AppearancesType == AnnotationAppearancesType.SingleStateAppearances)
			{
				SingleStateAppearances singleStateAppearances = (SingleStateAppearances)appearances;
				this.Content.Initialize(singleStateAppearances);
			}
		}

		internal void PrepareAppearancesForExport()
		{
			base.Appearances = new SingleStateAppearances(this.Content);
		}

		internal override Widget CreateClonedWidgetInstance(RadFixedDocumentCloneContext cloneContext)
		{
			FormField clonedField = cloneContext.GetClonedField(base.Field);
			PushButtonWidget pushButtonWidget = new PushButtonWidget(clonedField);
			pushButtonWidget.Content.Initialize(this.Content);
			pushButtonWidget.AppearanceCharacteristics = new PushButtonAppearanceCharacteristics(base.AppearanceCharacteristics);
			return pushButtonWidget;
		}

		void DrawNormalContent(FixedContentEditor editor)
		{
			this.DrawContent(editor, base.AppearanceCharacteristics.NormalCaption, base.AppearanceCharacteristics.NormalIconSource);
		}

		void DrawMouseDownContent(FixedContentEditor editor)
		{
			this.DrawContent(editor, base.AppearanceCharacteristics.MouseDownCaption, base.AppearanceCharacteristics.MouseDownIconSource);
		}

		void DrawMouseOverContent(FixedContentEditor editor)
		{
			this.DrawContent(editor, base.AppearanceCharacteristics.MouseOverCaption, base.AppearanceCharacteristics.MouseOverIconSource);
		}

		void DrawContent(FixedContentEditor editor, string caption, FormSource icon)
		{
			if (string.IsNullOrEmpty(caption))
			{
				if (icon != null && base.AppearanceCharacteristics.IconAndCaptionPosition != IconAndCaptionPosition.NoIconCaptionOnly)
				{
					this.DrawIcon(editor, icon, this.ButtonContentBox);
					return;
				}
			}
			else
			{
				Block block = new Block();
				base.TextProperties.CopyTo(block);
				block.InsertText(caption);
				if (icon == null)
				{
					if (base.AppearanceCharacteristics.IconAndCaptionPosition != IconAndCaptionPosition.NoCaptionIconOnly)
					{
						this.DrawCaptionCentered(editor, block);
						return;
					}
				}
				else
				{
					this.DrawCaptionAndIcon(editor, block, icon);
				}
			}
		}

		void DrawIcon(FixedContentEditor editor, FormSource icon, Rect contentBox)
		{
			bool flag = this.ShouldScale(icon, contentBox);
			if (!flag)
			{
				editor.Position.Translate(contentBox.X, contentBox.Y);
				editor.DrawForm(icon);
				return;
			}
			if (base.AppearanceCharacteristics.IconFitOptions.ScalingType == IconScalingType.FitExactly)
			{
				editor.Position.Translate(contentBox.X, contentBox.Y);
				editor.DrawForm(icon, contentBox.Width, contentBox.Height);
				return;
			}
			double val = contentBox.Width / icon.Size.Width;
			double val2 = contentBox.Height / icon.Size.Height;
			double num = System.Math.Min(val, val2);
			double num2 = num * icon.Size.Width;
			double num3 = num * icon.Size.Height;
			double num4 = contentBox.Width - num2;
			double num5 = contentBox.Height - num3;
			double offsetX = contentBox.X + num5 * base.AppearanceCharacteristics.IconFitOptions.BlankSpaceFromTheLeftSide;
			double offsetY = contentBox.Y + num4 * (1.0 - base.AppearanceCharacteristics.IconFitOptions.BlankSpaceFromTheBottomSide);
			editor.Position.Translate(offsetX, offsetY);
			editor.DrawForm(icon, num2, num3);
		}

		bool ShouldScale(FormSource icon, Rect contentBox)
		{
			switch (base.AppearanceCharacteristics.IconFitOptions.ScaleCondition)
			{
			case IconScaleCondition.ScaleIfBiggerThanAnnotationRectangle:
				return PushButtonWidget.IsBiggerIcon(icon, contentBox);
			case IconScaleCondition.ScaleIfSmallerThanAnnotationRectangle:
				return !PushButtonWidget.IsBiggerIcon(icon, contentBox);
			case IconScaleCondition.NeverScale:
				return false;
			default:
				return true;
			}
		}

		static bool IsBiggerIcon(FormSource icon, Rect contentBox)
		{
			return icon.Size.Width > contentBox.Width || icon.Size.Height > contentBox.Height;
		}

		static double GetCenteringOffset(double containerWidth, double elementWidth)
		{
			return (containerWidth - elementWidth) / 2.0;
		}

		void DrawCaptionCentered(FixedContentEditor editor, Block block)
		{
			Size size = block.Measure();
			double centeringOffset = PushButtonWidget.GetCenteringOffset(editor.Root.Size.Width, size.Width);
			double centeringOffset2 = PushButtonWidget.GetCenteringOffset(editor.Root.Size.Height, size.Height);
			editor.Position.Translate(centeringOffset, centeringOffset2);
			editor.DrawBlock(block);
		}

		void DrawCaptionAndIcon(FixedContentEditor editor, Block block, FormSource icon)
		{
			switch (base.AppearanceCharacteristics.IconAndCaptionPosition)
			{
			case IconAndCaptionPosition.NoIconCaptionOnly:
				this.DrawCaptionCentered(editor, block);
				return;
			case IconAndCaptionPosition.NoCaptionIconOnly:
				this.DrawIcon(editor, icon, this.ButtonContentBox);
				return;
			case IconAndCaptionPosition.CaptionBelowIcon:
				this.DrawCaptionBelowIcon(editor, block, icon);
				return;
			case IconAndCaptionPosition.CaptionAboveIcon:
				this.DrawCaptionAboveIcon(editor, block, icon);
				return;
			case IconAndCaptionPosition.CaptionToRightOfIcon:
				this.DrawCationToTheRightOfIcon(editor, block, icon);
				return;
			case IconAndCaptionPosition.CaptionToLeftOfIcon:
				this.DrawCaptionToTheLeftOfIcon(editor, block, icon);
				return;
			case IconAndCaptionPosition.CaptionOverIcon:
				this.DrawIcon(editor, icon, this.ButtonContentBox);
				this.DrawCaptionCentered(editor, block);
				return;
			default:
				return;
			}
		}

		void DrawCationToTheRightOfIcon(FixedContentEditor editor, Block block, FormSource icon)
		{
			Size size = block.Measure();
			Rect buttonContentBox = this.ButtonContentBox;
			if (size.Width + FixedDocumentDefaults.ButtonPadding < buttonContentBox.Width)
			{
				double num = buttonContentBox.Width - size.Width - FixedDocumentDefaults.ButtonPadding;
				double offsetX = buttonContentBox.X + num + FixedDocumentDefaults.ButtonPadding;
				double centeringOffset = PushButtonWidget.GetCenteringOffset(buttonContentBox.Height, size.Height);
				this.DrawIcon(editor, icon, new Rect(buttonContentBox.X, buttonContentBox.Y, num, buttonContentBox.Height));
				editor.Position.Translate(offsetX, centeringOffset);
				editor.DrawBlock(block);
				return;
			}
			this.DrawCaptionCentered(editor, block);
		}

		void DrawCaptionToTheLeftOfIcon(FixedContentEditor editor, Block block, FormSource icon)
		{
			Size size = block.Measure();
			Rect buttonContentBox = this.ButtonContentBox;
			if (size.Width + FixedDocumentDefaults.ButtonPadding < buttonContentBox.Width)
			{
				double width = buttonContentBox.Width - size.Width - FixedDocumentDefaults.ButtonPadding;
				double centeringOffset = PushButtonWidget.GetCenteringOffset(buttonContentBox.Height, size.Height);
				this.DrawIcon(editor, icon, new Rect(buttonContentBox.X + size.Width + FixedDocumentDefaults.ButtonPadding, buttonContentBox.Y, width, buttonContentBox.Height));
				editor.Position.Translate(buttonContentBox.X, centeringOffset);
				editor.DrawBlock(block);
				return;
			}
			this.DrawCaptionCentered(editor, block);
		}

		void DrawCaptionBelowIcon(FixedContentEditor editor, Block block, FormSource icon)
		{
			Size size = block.Measure();
			Rect buttonContentBox = this.ButtonContentBox;
			if (size.Height + FixedDocumentDefaults.ButtonPadding < buttonContentBox.Height)
			{
				double num = buttonContentBox.Height - size.Height - FixedDocumentDefaults.ButtonPadding;
				double centeringOffset = PushButtonWidget.GetCenteringOffset(buttonContentBox.Width, size.Width);
				double offsetY = buttonContentBox.X + num + FixedDocumentDefaults.ButtonPadding;
				this.DrawIcon(editor, icon, new Rect(buttonContentBox.X, buttonContentBox.Y, buttonContentBox.Width, num));
				editor.Position.Translate(centeringOffset, offsetY);
				editor.DrawBlock(block);
				return;
			}
			this.DrawCaptionCentered(editor, block);
		}

		void DrawCaptionAboveIcon(FixedContentEditor editor, Block block, FormSource icon)
		{
			Size size = block.Measure();
			Rect buttonContentBox = this.ButtonContentBox;
			if (size.Height + FixedDocumentDefaults.ButtonPadding < buttonContentBox.Height)
			{
				double height = buttonContentBox.Height - size.Height - FixedDocumentDefaults.ButtonPadding;
				double centeringOffset = PushButtonWidget.GetCenteringOffset(buttonContentBox.Width, size.Width);
				this.DrawIcon(editor, icon, new Rect(buttonContentBox.X, buttonContentBox.Y + size.Height + FixedDocumentDefaults.ButtonPadding, buttonContentBox.Width, height));
				editor.Position.Translate(centeringOffset, buttonContentBox.Y);
				editor.DrawBlock(block);
				return;
			}
			this.DrawCaptionCentered(editor, block);
		}

		readonly AnnotationContentSource content;
	}
}
