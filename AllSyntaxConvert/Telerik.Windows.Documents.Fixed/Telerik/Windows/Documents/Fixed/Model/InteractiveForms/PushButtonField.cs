using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class PushButtonField : FormField<PushButtonWidget>
	{
		public PushButtonField(string fieldName)
			: base(fieldName)
		{
		}

		public override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.PushButton;
			}
		}

		internal override PushButtonWidget CreateEmptyWidget()
		{
			return new PushButtonWidget(this);
		}

		internal override void PrepareWidgetAppearancesForExport()
		{
			foreach (PushButtonWidget pushButtonWidget in base.Widgets)
			{
				pushButtonWidget.PrepareAppearancesForExport();
			}
		}

		internal override FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext cloneContext)
		{
			return new PushButtonField(base.Name);
		}

		internal sealed override void InitializeWidgetAppearanceProperties(PushButtonWidget widget)
		{
			widget.AppearanceCharacteristics.Background = FixedDocumentDefaults.PushButtonBackground;
			widget.AppearanceCharacteristics.BorderColor = RgbColors.Black;
			widget.Border = new AnnotationBorder(FixedDocumentDefaults.ButtonWidgetBorderThickness, AnnotationBorderStyle.Beveled, null);
		}
	}
}
