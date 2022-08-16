using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.ColorSpaces;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations
{
	class WidgetPropertiesExporter : VariableTextObjectPropertiesExporter<WidgetObject>
	{
		public WidgetPropertiesExporter(WidgetObject node, IPdfExportContext exportContext)
			: base(node, exportContext)
		{
		}

		public void ExportWidgetProperties(Widget widget)
		{
			base.ExportVariableProperties(widget.TextProperties);
			this.ExportHighlightingMode(widget.HighlightingMode);
			this.ExportDynamicAppearanceCharacteristics(widget);
		}

		void ExportHighlightingMode(HighlightingMode highlightingMode)
		{
			switch (highlightingMode)
			{
			case HighlightingMode.NoHighlighting:
				base.Node.HighlightingMode = new PdfName("N");
				return;
			case HighlightingMode.InvertBorderOfAnnotationRectangle:
				base.Node.HighlightingMode = new PdfName("O");
				return;
			case HighlightingMode.UseAnnotationDownAppearance:
				base.Node.HighlightingMode = new PdfName("P");
				return;
			}
			base.Node.HighlightingMode = new PdfName("I");
		}

		void ExportDynamicAppearanceCharacteristics(Widget widget)
		{
			base.Node.DynamicAppearanceCharacteristics = new AppearanceCharacteristics();
			switch (widget.WidgetContentType)
			{
			case WidgetContentType.VariableContent:
			case WidgetContentType.SignatureContent:
			{
				Widget<DynamicAppearanceCharacteristics> widget2 = (Widget<DynamicAppearanceCharacteristics>)widget;
				this.CopyDynamicAppearance(widget2.AppearanceCharacteristics);
				return;
			}
			case WidgetContentType.TwoStatesContent:
			{
				TwoStatesButtonWidget twoStatesButtonWidget = (TwoStatesButtonWidget)widget;
				this.CopyButtonDynamicAppearance(twoStatesButtonWidget.AppearanceCharacteristics);
				return;
			}
			case WidgetContentType.PushButtonContent:
			{
				PushButtonWidget pushButtonWidget = (PushButtonWidget)widget;
				this.CopyPushButtonDynamicAppearance(pushButtonWidget.AppearanceCharacteristics);
				return;
			}
			default:
				return;
			}
		}

		void CopyDynamicAppearance(DynamicAppearanceCharacteristics appearance)
		{
			base.Node.DynamicAppearanceCharacteristics.Rotation = Page.GetRotate(appearance.Rotation);
			base.Node.DynamicAppearanceCharacteristics.BorderColor = WidgetPropertiesExporter.GetColorArray(appearance.BorderColor);
			base.Node.DynamicAppearanceCharacteristics.Background = WidgetPropertiesExporter.GetColorArray(appearance.Background);
		}

		void CopyButtonDynamicAppearance(ButtonAppearanceCharacteristics appearance)
		{
			this.CopyDynamicAppearance(appearance);
			base.Node.DynamicAppearanceCharacteristics.ButtonNormalCaption = appearance.NormalCaption.ToPdfString();
		}

		void CopyPushButtonDynamicAppearance(PushButtonAppearanceCharacteristics appearance)
		{
			this.CopyButtonDynamicAppearance(appearance);
			base.Node.DynamicAppearanceCharacteristics.DownCaption = appearance.MouseDownCaption.ToPdfString();
			base.Node.DynamicAppearanceCharacteristics.RolloverCaption = appearance.MouseOverCaption.ToPdfString();
			base.Node.DynamicAppearanceCharacteristics.NormalIcon = this.GetIcon(appearance.NormalIconSource);
			base.Node.DynamicAppearanceCharacteristics.DownIcon = this.GetIcon(appearance.MouseDownIconSource);
			base.Node.DynamicAppearanceCharacteristics.RolloverIcon = this.GetIcon(appearance.MouseOverIconSource);
			base.Node.DynamicAppearanceCharacteristics.IconFit = this.GetIconFit(appearance.IconFitOptions);
			base.Node.DynamicAppearanceCharacteristics.PushButtonTextPosition = new PdfInt((int)appearance.IconAndCaptionPosition);
		}

		IconFit GetIconFit(IconFitOptions iconFitOptions)
		{
			IconFit iconFit = new IconFit();
			switch (iconFitOptions.ScaleCondition)
			{
			case IconScaleCondition.ScaleIfBiggerThanAnnotationRectangle:
				iconFit.ScaleCondition = new PdfName("B");
				break;
			case IconScaleCondition.ScaleIfSmallerThanAnnotationRectangle:
				iconFit.ScaleCondition = new PdfName("S");
				break;
			case IconScaleCondition.NeverScale:
				iconFit.ScaleCondition = new PdfName("N");
				break;
			default:
				iconFit.ScaleCondition = new PdfName("A");
				break;
			}
			iconFit.ScaleType = ((iconFitOptions.ScalingType == IconScalingType.FitExactly) ? new PdfName("A") : new PdfName("P"));
			iconFit.TranslationAfterProportionalScaling = new PdfArray(new PdfPrimitive[]
			{
				new PdfReal(iconFitOptions.BlankSpaceFromTheLeftSide),
				new PdfReal(iconFitOptions.BlankSpaceFromTheBottomSide)
			});
			iconFit.FitIgnoringBorderWidth = new PdfBool(iconFitOptions.IgnoreBorderWidth);
			return iconFit;
		}

		FormXObject GetIcon(FormSource source)
		{
			if (source == null)
			{
				return null;
			}
			ResourceEntry resource = base.Context.GetResource(source);
			return (FormXObject)resource.Resource.Content;
		}

		static PdfArray GetColorArray(RgbColor color)
		{
			if (color == null)
			{
				return null;
			}
			PdfReal pdfReal = ColorObjectBase.ConvertFromByte(color.R);
			PdfReal pdfReal2 = ColorObjectBase.ConvertFromByte(color.G);
			PdfReal pdfReal3 = ColorObjectBase.ConvertFromByte(color.B);
			return new PdfArray(new PdfPrimitive[] { pdfReal, pdfReal2, pdfReal3 });
		}
	}
}
