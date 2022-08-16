using System;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.ColorSpaces;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public abstract class ChoiceField : FormField<VariableContentWidget>
	{
		internal ChoiceField(string fieldName)
			: base(fieldName)
		{
			this.options = new ChoiceOptionCollection(this);
		}

		public ChoiceOptionCollection Options
		{
			get
			{
				return this.options;
			}
		}

		public bool ShouldCommitOnSelectionChange { get; set; }

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
			ChoiceField clonedChoiceField = this.GetClonedChoiceField(cloneContext);
			clonedChoiceField.ShouldCommitOnSelectionChange = this.ShouldCommitOnSelectionChange;
			foreach (ChoiceOption option in this.Options)
			{
				clonedChoiceField.Options.Add(cloneContext.GetClonedOption(option));
			}
			return clonedChoiceField;
		}

		internal sealed override void InitializeWidgetAppearanceProperties(VariableContentWidget widget)
		{
			widget.AppearanceCharacteristics.Background = RgbColors.White;
		}

		internal abstract ChoiceField GetClonedChoiceField(RadFixedDocumentCloneContext cloneContext);

		readonly ChoiceOptionCollection options;
	}
}
