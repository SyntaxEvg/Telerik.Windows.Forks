using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class CheckBoxField : FormField<TwoStatesButtonWidget>
	{
		public CheckBoxField(string fieldName)
			: base(fieldName)
		{
			this.ExportValue = "Yes";
		}

		public sealed override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.CheckBox;
			}
		}

		public bool IsChecked { get; set; }

		public bool IsCheckedByDefault { get; set; }

		public string ExportValue
		{
			get
			{
				return this.exportValue;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				this.exportValue = value;
			}
		}

		internal sealed override VariableTextProperties GetDefaultTextProperties()
		{
			return new VariableTextProperties
			{
				Font = FontsRepository.ZapfDingbats,
				FontSize = 0.0
			};
		}

		internal sealed override TwoStatesButtonWidget CreateEmptyWidget()
		{
			return new TwoStatesButtonWidget(this);
		}

		internal sealed override void PrepareWidgetAppearancesForExport()
		{
			foreach (TwoStatesButtonWidget twoStatesButtonWidget in base.Widgets)
			{
				twoStatesButtonWidget.PrepareAppearancesForExport("Yes", this.IsChecked);
			}
		}

		internal sealed override FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext radFixedDocumentCloneContext)
		{
			return new CheckBoxField(base.Name)
			{
				IsChecked = this.IsChecked,
				IsCheckedByDefault = this.IsCheckedByDefault,
				ExportValue = this.ExportValue
			};
		}

		internal sealed override void InitializeWidgetAppearanceProperties(TwoStatesButtonWidget widget)
		{
			widget.AppearanceCharacteristics.NormalCaption = "4";
			widget.Border = new AnnotationBorder(FixedDocumentDefaults.ButtonWidgetBorderThickness, AnnotationBorderStyle.Solid, null);
			widget.AppearanceCharacteristics.BorderColor = RgbColors.Black;
			widget.AppearanceCharacteristics.Background = RgbColors.White;
		}

		internal const string DefaultExportValue = "Yes";

		string exportValue;
	}
}
