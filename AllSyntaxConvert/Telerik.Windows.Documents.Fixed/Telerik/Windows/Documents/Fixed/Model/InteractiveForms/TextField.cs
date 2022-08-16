using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public abstract class TextField : FormField<VariableContentWidget>
	{
		public TextField(string fieldName)
			: base(fieldName)
		{
		}

		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					this.value = value;
					base.InvalidateWidgetAppearances();
				}
			}
		}

		public string DefaultValue { get; set; }

		internal sealed override VariableContentWidget CreateEmptyWidget()
		{
			return new VariableContentWidget(this);
		}

		internal sealed override void PrepareWidgetAppearancesForExport()
		{
			foreach (VariableContentWidget variableContentWidget in base.Widgets)
			{
				variableContentWidget.PrepareAppearancesForExport();
			}
		}

		internal sealed override FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext cloneContext)
		{
			TextField clonedTextField = this.GetClonedTextField(cloneContext);
			clonedTextField.Value = this.Value;
			clonedTextField.DefaultValue = this.DefaultValue;
			return clonedTextField;
		}

		internal sealed override void InitializeWidgetAppearanceProperties(VariableContentWidget widget)
		{
			widget.AppearanceCharacteristics.Background = RgbColors.White;
		}

		internal abstract TextField GetClonedTextField(RadFixedDocumentCloneContext cloneContext);

		string value;
	}
}
