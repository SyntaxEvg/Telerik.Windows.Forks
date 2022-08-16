using System;
using Telerik.Windows.Documents.Core.Imaging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
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
	class WidgetPropertiesImporter : VariableTextObjectPropertiesImporter<WidgetObject>
	{
		public WidgetPropertiesImporter(WidgetObject node, PostScriptReader reader, IRadFixedDocumentImportContext context)
			: base(node, reader, context)
		{
		}

		public void ImportWidgetProperties(Widget widget)
		{
			widget.TextProperties = base.ReadTextProperties();
			widget.HighlightingMode = this.ReadHighlightingMode();
			if (base.Node.DynamicAppearanceCharacteristics == null)
			{
				return;
			}
			switch (widget.WidgetContentType)
			{
			case WidgetContentType.VariableContent:
			case WidgetContentType.SignatureContent:
			{
				Widget<DynamicAppearanceCharacteristics> widget2 = (Widget<DynamicAppearanceCharacteristics>)widget;
				widget2.AppearanceCharacteristics = new DynamicAppearanceCharacteristics();
				this.ReadDynamicAppearanceProperties(widget2.AppearanceCharacteristics);
				return;
			}
			case WidgetContentType.TwoStatesContent:
			{
				TwoStatesButtonWidget twoStatesButtonWidget = (TwoStatesButtonWidget)widget;
				twoStatesButtonWidget.AppearanceCharacteristics = new ButtonAppearanceCharacteristics();
				this.ReadButtonDynamicAppearanceProperties(twoStatesButtonWidget.AppearanceCharacteristics);
				return;
			}
			case WidgetContentType.PushButtonContent:
			{
				PushButtonWidget pushButtonWidget = (PushButtonWidget)widget;
				pushButtonWidget.AppearanceCharacteristics = new PushButtonAppearanceCharacteristics();
				this.ReadPushButtonDynamicAppearanceProperties(pushButtonWidget.AppearanceCharacteristics);
				return;
			}
			default:
				throw new NotSupportedException(string.Format("Not supported widget type: {0}", widget.WidgetContentType));
			}
		}

		void ReadPushButtonDynamicAppearanceProperties(PushButtonAppearanceCharacteristics appearance)
		{
			this.ReadButtonDynamicAppearanceProperties(appearance);
			appearance.MouseDownCaption = WidgetPropertiesImporter.GetCaption(base.Node.DynamicAppearanceCharacteristics.DownCaption);
			appearance.MouseOverCaption = WidgetPropertiesImporter.GetCaption(base.Node.DynamicAppearanceCharacteristics.RolloverCaption);
			appearance.NormalIconSource = this.GetIconSource(base.Node.DynamicAppearanceCharacteristics.NormalIcon);
			appearance.MouseDownIconSource = this.GetIconSource(base.Node.DynamicAppearanceCharacteristics.DownIcon);
			appearance.MouseOverIconSource = this.GetIconSource(base.Node.DynamicAppearanceCharacteristics.RolloverIcon);
			appearance.IconFitOptions.ScaleCondition = this.ReadScaleCondition();
			appearance.IconFitOptions.ScalingType = this.ReadScalingType();
			double[] array = base.Node.DynamicAppearanceCharacteristics.IconFit.TranslationAfterProportionalScaling.ToDoubleArray();
			appearance.IconFitOptions.BlankSpaceFromTheLeftSide = array[0];
			appearance.IconFitOptions.BlankSpaceFromTheBottomSide = array[1];
			appearance.IconFitOptions.IgnoreBorderWidth = base.Node.DynamicAppearanceCharacteristics.IconFit.FitIgnoringBorderWidth.Value;
			appearance.IconAndCaptionPosition = (IconAndCaptionPosition)base.Node.DynamicAppearanceCharacteristics.PushButtonTextPosition.Value;
		}

		IconScalingType ReadScalingType()
		{
			string value = base.Node.DynamicAppearanceCharacteristics.IconFit.ScaleType.Value;
			if (!value.Equals("A"))
			{
				return IconScalingType.Proportional;
			}
			return IconScalingType.FitExactly;
		}

		IconScaleCondition ReadScaleCondition()
		{
			string value = base.Node.DynamicAppearanceCharacteristics.IconFit.ScaleCondition.Value;
			string a;
			if ((a = value) != null)
			{
				if (a == "B")
				{
					return IconScaleCondition.ScaleIfBiggerThanAnnotationRectangle;
				}
				if (a == "S")
				{
					return IconScaleCondition.ScaleIfSmallerThanAnnotationRectangle;
				}
				if (a == "N")
				{
					return IconScaleCondition.NeverScale;
				}
			}
			return IconScaleCondition.AlwaysScale;
		}

		void ReadButtonDynamicAppearanceProperties(ButtonAppearanceCharacteristics appearance)
		{
			this.ReadDynamicAppearanceProperties(appearance);
			appearance.NormalCaption = WidgetPropertiesImporter.GetCaption(base.Node.DynamicAppearanceCharacteristics.ButtonNormalCaption);
		}

		void ReadDynamicAppearanceProperties(DynamicAppearanceCharacteristics appearance)
		{
			appearance.Rotation = Page.GetRotation(base.Node.DynamicAppearanceCharacteristics.Rotation);
			appearance.Background = WidgetPropertiesImporter.GetColor(base.Node.DynamicAppearanceCharacteristics.Background);
			appearance.BorderColor = WidgetPropertiesImporter.GetColor(base.Node.DynamicAppearanceCharacteristics.BorderColor);
		}

		FormSource GetIconSource(FormXObject form)
		{
			if (form != null)
			{
				return form.ToFormSource(base.Reader, base.Context);
			}
			return null;
		}

		static string GetCaption(PdfString caption)
		{
			if (caption != null)
			{
				return caption.ToString();
			}
			return null;
		}

		static RgbColor GetColor(PdfArray colorArray)
		{
			if (colorArray == null)
			{
				return null;
			}
			double[] array = colorArray.ToDoubleArray();
			switch (colorArray.Count)
			{
			case 1:
			{
				byte b = Color.ConvertColorComponentToByte(array[0]);
				return new RgbColor(b, b, b);
			}
			case 3:
			{
				byte r = Color.ConvertColorComponentToByte(array[0]);
				byte g = Color.ConvertColorComponentToByte(array[1]);
				byte b2 = Color.ConvertColorComponentToByte(array[2]);
				return new RgbColor(r, g, b2);
			}
			case 4:
			{
				double cyan = array[0];
				double magenta = array[1];
				double yellow = array[2];
				double black = array[3];
				Color color = Color.FromCmyk(cyan, magenta, yellow, black);
				return new RgbColor(color.R, color.G, color.B);
			}
			}
			return null;
		}

		HighlightingMode ReadHighlightingMode()
		{
			string value = base.Node.HighlightingMode.Value;
			string a;
			if ((a = value) != null)
			{
				if (a == "N")
				{
					return HighlightingMode.NoHighlighting;
				}
				if (a == "O")
				{
					return HighlightingMode.InvertBorderOfAnnotationRectangle;
				}
				if (a == "P" || a == "T")
				{
					return HighlightingMode.UseAnnotationDownAppearance;
				}
			}
			return HighlightingMode.InvertContentOfAnnotationRectangle;
		}
	}
}
