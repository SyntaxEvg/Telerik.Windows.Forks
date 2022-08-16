using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class RadioButtonWidgetCollection : WidgetCollectionBase<RadioButtonWidget>
	{
		internal RadioButtonWidgetCollection(IWidgetCreator<RadioButtonWidget> creator)
			: base(creator)
		{
		}

		public RadioButtonWidget AddWidget(RadioOption option)
		{
			RadioButtonWidget radioButtonWidget = base.AddEmptyWidget();
			radioButtonWidget.Option = option;
			radioButtonWidget.Border = new AnnotationBorder(FixedDocumentDefaults.ButtonWidgetBorderThickness, AnnotationBorderStyle.Inset, null);
			radioButtonWidget.AppearanceCharacteristics.BorderColor = RgbColors.Black;
			radioButtonWidget.AppearanceCharacteristics.Background = RgbColors.White;
			radioButtonWidget.AppearanceCharacteristics.NormalCaption = "l";
			return radioButtonWidget;
		}
	}
}
