using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public sealed class RadioButtonField : FormField, IWidgetCreator<RadioButtonWidget>
	{
		public RadioButtonField(string fieldName)
			: base(fieldName)
		{
			this.options = new RadioOptionCollection();
			this.widgets = new RadioButtonWidgetCollection(this);
			this.ShouldUpdateRadiosInUnison = true;
		}

		public override FormFieldType FieldType
		{
			get
			{
				return FormFieldType.RadioButton;
			}
		}

		public RadioOptionCollection Options
		{
			get
			{
				return this.options;
			}
		}

		public new RadioButtonWidgetCollection Widgets
		{
			get
			{
				return this.widgets;
			}
		}

		public RadioOption Value { get; set; }

		public RadioOption DefaultValue { get; set; }

		public bool AllowToggleOff { get; set; }

		public bool ShouldUpdateRadiosInUnison { get; set; }

		internal sealed override VariableTextProperties GetDefaultTextProperties()
		{
			return new VariableTextProperties
			{
				Font = FontsRepository.ZapfDingbats
			};
		}

		internal sealed override IEnumerable<Widget> GetWidgets()
		{
			foreach (RadioButtonWidget widget in this.Widgets)
			{
				yield return widget;
			}
			yield break;
		}

		internal sealed override Widget AddWidget()
		{
			return this.Widgets.AddEmptyWidget();
		}

		internal sealed override void AddClonedWidget(Widget clonedWidget)
		{
			this.Widgets.AddClonedWidget(clonedWidget);
		}

		internal sealed override void PrepareWidgetAppearancesForExport()
		{
			int num = 0;
			if (this.ShouldUpdateRadiosInUnison)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				using (IEnumerator<RadioButtonWidget> enumerator = this.Widgets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RadioButtonWidget radioButtonWidget = enumerator.Current;
						string text;
						if (!dictionary.TryGetValue(radioButtonWidget.Option.Value, out text))
						{
							text = num.ToString();
							dictionary.Add(radioButtonWidget.Option.Value, text);
						}
						radioButtonWidget.PrepareAppearancesForExport(text, radioButtonWidget.IsSelected);
						num++;
					}
					return;
				}
			}
			foreach (RadioButtonWidget radioButtonWidget2 in this.Widgets)
			{
				string onStateName = num.ToString();
				radioButtonWidget2.PrepareAppearancesForExport(onStateName, radioButtonWidget2.IsSelected);
				num++;
			}
		}

		internal sealed override FormField GetClonedInstanceWithoutWidgetsOverride(RadFixedDocumentCloneContext cloneContext)
		{
			RadioButtonField radioButtonField = new RadioButtonField(base.Name);
			foreach (RadioOption option in this.options)
			{
				radioButtonField.Options.Add(cloneContext.GetClonedOption(option));
			}
			radioButtonField.Value = cloneContext.GetClonedOption(this.Value);
			radioButtonField.DefaultValue = cloneContext.GetClonedOption(this.DefaultValue);
			radioButtonField.AllowToggleOff = this.AllowToggleOff;
			radioButtonField.ShouldUpdateRadiosInUnison = this.ShouldUpdateRadiosInUnison;
			return radioButtonField;
		}

		RadioButtonWidget IWidgetCreator<RadioButtonWidget>.CreateWidget()
		{
			return new RadioButtonWidget(this);
		}

		readonly RadioOptionCollection options;

		readonly RadioButtonWidgetCollection widgets;
	}
}
