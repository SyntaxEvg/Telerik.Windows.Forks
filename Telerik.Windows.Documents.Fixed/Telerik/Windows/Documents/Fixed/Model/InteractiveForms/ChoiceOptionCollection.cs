using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class ChoiceOptionCollection : FieldOptionCollection<ChoiceOption>
	{
		internal ChoiceOptionCollection(ChoiceField choiceField)
		{
			this.parent = choiceField;
		}

		internal override void OnBeforeAddOption(ChoiceOption option)
		{
			this.parent.InvalidateWidgetAppearances();
			option.UserInterfaceValueChanged += this.OptionUserInterfaceValueChanged;
			base.OnBeforeAddOption(option);
		}

		internal override void OnBeforeRemoveOptions(IEnumerable<ChoiceOption> options)
		{
			this.parent.InvalidateWidgetAppearances();
			foreach (ChoiceOption choiceOption in options)
			{
				choiceOption.UserInterfaceValueChanged -= this.OptionUserInterfaceValueChanged;
			}
			base.OnBeforeRemoveOptions(options);
		}

		void OptionUserInterfaceValueChanged(object sender, EventArgs e)
		{
			this.parent.InvalidateWidgetAppearances();
		}

		readonly ChoiceField parent;
	}
}
