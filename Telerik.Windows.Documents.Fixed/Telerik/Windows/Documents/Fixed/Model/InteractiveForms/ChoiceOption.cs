using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class ChoiceOption : FieldOptionBase, IContextClonable<ChoiceOption>
	{
		public ChoiceOption(string value)
			: base(value)
		{
		}

		public override string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				Guard.ThrowExceptionIfNullOrEmpty(value, "value");
				if (this.value != value)
				{
					this.value = value;
					if (this.UserInterfaceValue == null)
					{
						this.OnUserInterfaceValueChanged();
					}
				}
			}
		}

		public string UserInterfaceValue
		{
			get
			{
				return this.userInterfaceValue;
			}
			set
			{
				if (this.userInterfaceValue != value)
				{
					this.userInterfaceValue = value;
					this.OnUserInterfaceValueChanged();
				}
			}
		}

		internal event EventHandler UserInterfaceValueChanged;

		void OnUserInterfaceValueChanged()
		{
			if (this.UserInterfaceValueChanged != null)
			{
				this.UserInterfaceValueChanged(this, EventArgs.Empty);
			}
		}

		ChoiceOption IContextClonable<ChoiceOption>.Clone(RadFixedDocumentCloneContext cloneContext)
		{
			return new ChoiceOption(this.Value)
			{
				UserInterfaceValue = this.UserInterfaceValue
			};
		}

		string value;

		string userInterfaceValue;
	}
}
