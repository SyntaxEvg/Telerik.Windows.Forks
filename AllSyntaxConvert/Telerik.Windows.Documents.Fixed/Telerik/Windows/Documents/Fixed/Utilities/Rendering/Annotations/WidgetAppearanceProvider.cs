using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.Utilities.Rendering.Annotations
{
	class WidgetAppearanceProvider : BaseAppearanceProvider
	{
		internal WidgetAppearanceProvider(BaseAppearanceProvider nextProvider)
			: base(nextProvider)
		{
		}

		protected override bool CanProvide(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode)
		{
			return annotation is Widget;
		}

		protected override FormSource ProvideAppearance(Annotation annotation, AnnotationAppearanceMode annotationAppearanceMode)
		{
			IContentAnnotation contentAnnotation = annotation as IContentAnnotation;
			if (contentAnnotation != null && contentAnnotation.Content != null)
			{
				SingleStateAppearances singleStateAppearances = new SingleStateAppearances(contentAnnotation.Content);
				return base.GetAppearanceFormSource(singleStateAppearances, annotationAppearanceMode);
			}
			RadioButtonWidget radioButtonWidget = annotation as RadioButtonWidget;
			if (radioButtonWidget != null)
			{
				SingleStateAppearances radioButtonSingleStateAppearances = this.GetRadioButtonSingleStateAppearances(radioButtonWidget);
				if (radioButtonSingleStateAppearances != null)
				{
					return base.GetAppearanceFormSource(radioButtonSingleStateAppearances, annotationAppearanceMode);
				}
			}
			TwoStatesButtonWidget twoStatesButtonWidget = annotation as TwoStatesButtonWidget;
			if (twoStatesButtonWidget != null)
			{
				CheckBoxField checkBoxField = twoStatesButtonWidget.Field as CheckBoxField;
				SingleStateAppearances checkBoxSingleStateAppearances = this.GetCheckBoxSingleStateAppearances(twoStatesButtonWidget, checkBoxField);
				if (checkBoxSingleStateAppearances != null)
				{
					return base.GetAppearanceFormSource(checkBoxSingleStateAppearances, annotationAppearanceMode);
				}
			}
			return null;
		}

		SingleStateAppearances GetRadioButtonSingleStateAppearances(RadioButtonWidget radioButtonWidget)
		{
			SingleStateAppearances result = null;
			if (radioButtonWidget.IsSelected && radioButtonWidget.OnStateContent != null)
			{
				result = new SingleStateAppearances(radioButtonWidget.OnStateContent);
			}
			else if (!radioButtonWidget.IsSelected && radioButtonWidget.OffStateContent != null)
			{
				result = new SingleStateAppearances(radioButtonWidget.OffStateContent);
			}
			return result;
		}

		SingleStateAppearances GetCheckBoxSingleStateAppearances(TwoStatesButtonWidget twoStatesButtonWidget, CheckBoxField checkBoxField)
		{
			SingleStateAppearances result = null;
			if (checkBoxField.IsChecked && twoStatesButtonWidget.OnStateContent != null)
			{
				result = new SingleStateAppearances(twoStatesButtonWidget.OnStateContent);
			}
			else if (!checkBoxField.IsChecked && twoStatesButtonWidget.OffStateContent != null)
			{
				result = new SingleStateAppearances(twoStatesButtonWidget.OffStateContent);
			}
			return result;
		}
	}
}
