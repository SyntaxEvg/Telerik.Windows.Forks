using System;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class ComboBoxField : ChoiceField
	{
		public ComboBoxField(string fieldName)
			: base(fieldName)
		{
		}

		public override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.ComboBox;
			}
		}

		public bool HasEditableTextBox { get; set; }

		public bool ShouldSpellCheck { get; set; }

		public ChoiceOption Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					if (this.value != null)
					{
						this.value.UserInterfaceValueChanged -= this.ValueUserInterfaceValueChanged;
					}
					this.value = value;
					base.InvalidateWidgetAppearances();
					if (this.value != null)
					{
						this.value.UserInterfaceValueChanged += this.ValueUserInterfaceValueChanged;
					}
				}
			}
		}

		public ChoiceOption DefaultValue { get; set; }

		internal sealed override ChoiceField GetClonedChoiceField(RadFixedDocumentCloneContext cloneContext)
		{
			return new ComboBoxField(base.Name)
			{
				HasEditableTextBox = this.HasEditableTextBox,
				ShouldSpellCheck = this.ShouldSpellCheck,
				Value = cloneContext.GetClonedOption(this.Value),
				DefaultValue = cloneContext.GetClonedOption(this.DefaultValue)
			};
		}

		void ValueUserInterfaceValueChanged(object sender, EventArgs e)
		{
			base.InvalidateWidgetAppearances();
		}

		ChoiceOption value;
	}
}
