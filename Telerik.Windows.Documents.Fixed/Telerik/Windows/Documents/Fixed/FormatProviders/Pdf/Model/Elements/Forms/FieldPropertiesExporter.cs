using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FieldPropertiesExporter : VariableTextObjectPropertiesExporter<FormFieldNode>
	{
		public FieldPropertiesExporter(FormFieldNode node, IPdfExportContext exportContext)
			: base(node, exportContext)
		{
		}

		FlagWriter<FieldFlag> FlagWriter { get; set; }

		public void ExportFieldProperties(FormField field)
		{
			this.FlagWriter = new FlagWriter<FieldFlag>();
			switch (field.FieldType)
			{
			case FormFieldType.PushButton:
			case FormFieldType.CheckBox:
			case FormFieldType.RadioButton:
				this.CopyFromButton(field);
				break;
			case FormFieldType.CombTextBox:
			case FormFieldType.TextBox:
				this.CopyFromText((TextField)field);
				break;
			case FormFieldType.ComboBox:
			case FormFieldType.ListBox:
				this.CopyFromChoice((ChoiceField)field);
				break;
			case FormFieldType.Signature:
				this.CopyFromSignature((SignatureField)field);
				break;
			default:
				throw new NotSupportedException(string.Format("Not supported field type: {0}", field.FieldType));
			}
			this.CopyCommonFieldProperties(field);
		}

		void CopyCommonFieldProperties(FormField field)
		{
			this.FlagWriter.SetFlagOnCondition(FieldFlag.Required, field.IsRequired);
			this.FlagWriter.SetFlagOnCondition(FieldFlag.NoExport, field.ShouldBeSkipped);
			this.FlagWriter.SetFlagOnCondition(FieldFlag.ReadOnly, field.IsReadOnly);
			base.Node.PartialName = field.Name.ToPdfString();
			base.Node.UserInterfaceName = field.UserInterfaceName.ToPdfString();
			base.Node.MappingName = field.MappingName.ToPdfString();
			base.Node.FieldFlags = new PdfInt(this.FlagWriter.ResultFlags);
			base.ExportVariableProperties(field.TextProperties);
			this.CopyWidgets(field.Widgets);
		}

		void CopyWidgets(IEnumerable<Widget> widgets)
		{
			foreach (Widget widget in widgets)
			{
				WidgetObject primitive;
				if (base.Context.TryGetWidget(widget, out primitive))
				{
					if (base.Node.Kids == null)
					{
						base.Node.Kids = new PdfArray(new PdfPrimitive[0]);
					}
					IndirectObject indirectObject = base.Context.CreateIndirectObject(primitive);
					base.Node.Kids.Add(indirectObject.Reference);
				}
				widget.Appearances = null;
			}
		}

		void CopyFromButton(FormField buttonField)
		{
			base.Node.FieldType = new PdfName("Btn");
			switch (buttonField.FieldType)
			{
			case FormFieldType.PushButton:
				this.FlagWriter.SetFlag(FieldFlag.PushButton);
				return;
			case FormFieldType.CheckBox:
			{
				CheckBoxField checkBox = (CheckBoxField)buttonField;
				this.CopyFromCheckBox(checkBox);
				return;
			}
			case FormFieldType.RadioButton:
			{
				RadioButtonField radioButtonField = (RadioButtonField)buttonField;
				this.FlagWriter.SetFlag(FieldFlag.Radio);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.NoToggleToOff, !radioButtonField.AllowToggleOff);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.RadiosInUnison, radioButtonField.ShouldUpdateRadiosInUnison);
				this.CopyFromRadioButton(radioButtonField);
				return;
			}
			default:
				return;
			}
		}

		void CopyFromCheckBox(CheckBoxField checkBox)
		{
			PdfName primitive = (checkBox.IsChecked ? new PdfName("Yes") : new PdfName("Off"));
			base.Node.FieldValue = new PrimitiveWrapper(primitive);
			PdfName primitive2 = (checkBox.IsCheckedByDefault ? new PdfName("Yes") : new PdfName("Off"));
			base.Node.DefaultFieldValue = new PrimitiveWrapper(primitive2);
			if (checkBox.ExportValue != "Yes")
			{
				PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
				foreach (Widget widget in this.GetPageExportedWidgets(checkBox))
				{
					pdfArray.Add(checkBox.ExportValue.ToPdfString());
				}
				if (pdfArray.Count > 0)
				{
					base.Node.Options = pdfArray;
				}
			}
		}

		void CopyFromRadioButton(RadioButtonField radio)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			bool shouldUpdateRadiosInUnison = radio.ShouldUpdateRadiosInUnison;
			foreach (Widget widget in this.GetPageExportedWidgets(radio))
			{
				RadioButtonWidget radioButtonWidget = (RadioButtonWidget)widget;
				pdfArray.Add(radioButtonWidget.Option.Value.ToPdfString());
				if (radio.Value != null && base.Node.FieldValue == null)
				{
					RadioOption value = radio.Value;
					string initialValue;
					if (this.TryGetOptionOnStateFromWidget(radioButtonWidget, value, shouldUpdateRadiosInUnison, out initialValue))
					{
						base.Node.FieldValue = new PrimitiveWrapper(new PdfName(initialValue));
					}
				}
				if (radio.DefaultValue != null && base.Node.DefaultFieldValue == null)
				{
					RadioOption defaultValue = radio.DefaultValue;
					string initialValue2;
					if (this.TryGetOptionOnStateFromWidget(radioButtonWidget, defaultValue, shouldUpdateRadiosInUnison, out initialValue2))
					{
						base.Node.DefaultFieldValue = new PrimitiveWrapper(new PdfName(initialValue2));
					}
				}
			}
			if (pdfArray.Count > 0)
			{
				base.Node.Options = pdfArray;
			}
		}

		bool TryGetOptionOnStateFromWidget(RadioButtonWidget widget, RadioOption option, bool shouldUpdateRadiosInUnison, out string stateName)
		{
			bool flag = widget.Option.Equals(option, shouldUpdateRadiosInUnison);
			if (flag)
			{
				MultiStateAppearances source = (MultiStateAppearances)widget.Appearances;
				stateName = (from x in source
					where x.Key != "Off"
					select x).First<KeyValuePair<string, SingleStateAppearances>>().Key;
				return true;
			}
			stateName = null;
			return false;
		}

		IEnumerable<Widget> GetPageExportedWidgets(FormField formField)
		{
			foreach (Widget widgetModel in formField.Widgets)
			{
				WidgetObject widget;
				if (base.Context.TryGetWidget(widgetModel, out widget))
				{
					yield return widgetModel;
				}
			}
			yield break;
		}

		void CopyFromChoice(ChoiceField choiceField)
		{
			base.Node.FieldType = new PdfName("Ch");
			this.FlagWriter.SetFlagOnCondition(FieldFlag.CommitOnSelChange, choiceField.ShouldCommitOnSelectionChange);
			this.CopyFromChoiceOptionsCollection(choiceField.Options);
			switch (choiceField.FieldType)
			{
			case FormFieldType.ComboBox:
			{
				ComboBoxField comboBoxField = (ComboBoxField)choiceField;
				this.FlagWriter.SetFlag(FieldFlag.Combo);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.Edit, comboBoxField.HasEditableTextBox);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.DoNotSpellCheck, !comboBoxField.ShouldSpellCheck);
				this.CopyComboBoxValues(comboBoxField);
				return;
			}
			case FormFieldType.ListBox:
			{
				ListBoxField listBoxField = (ListBoxField)choiceField;
				this.FlagWriter.SetFlagOnCondition(FieldFlag.MultiSelect, listBoxField.AllowMultiSelection);
				base.Node.TopIndex = new PdfInt(listBoxField.TopIndex);
				this.CopyListBoxValues(listBoxField);
				return;
			}
			default:
				return;
			}
		}

		void CopyFromChoiceOptionsCollection(ChoiceOptionCollection options)
		{
			base.Node.Options = new PdfArray(new PdfPrimitive[0]);
			foreach (ChoiceOption choiceOption in options)
			{
				PdfPrimitive item;
				if (choiceOption.UserInterfaceValue == null)
				{
					item = choiceOption.Value.ToPdfString();
				}
				else
				{
					item = new PdfArray(new PdfPrimitive[]
					{
						choiceOption.Value.ToPdfString(),
						choiceOption.UserInterfaceValue.ToPdfString()
					});
				}
				base.Node.Options.Add(item);
			}
		}

		void CopyComboBoxValues(ComboBoxField comboBox)
		{
			if (comboBox.Value != null)
			{
				int defaultValue;
				bool flag = comboBox.Options.TryGetOptionIndex(comboBox.Value, out defaultValue);
				if (comboBox.HasEditableTextBox || flag)
				{
					base.Node.FieldValue = new PrimitiveWrapper(comboBox.Value.Value.ToPdfString());
				}
				if (flag)
				{
					base.Node.SortedIndices = new PdfArray(new PdfPrimitive[]
					{
						new PdfInt(defaultValue)
					});
				}
			}
			int num;
			if (comboBox.DefaultValue != null && comboBox.Options.TryGetOptionIndex(comboBox.DefaultValue, out num))
			{
				base.Node.DefaultFieldValue = new PrimitiveWrapper(comboBox.DefaultValue.Value.ToPdfString());
			}
		}

		void CopyListBoxValues(ListBoxField listBox)
		{
			int[] selectedIndicesSorted = listBox.GetSelectedIndicesSorted();
			if (selectedIndicesSorted.Length > 0)
			{
				base.Node.FieldValue = new PrimitiveWrapper(this.GetSelectedValueArray(listBox.Options, selectedIndicesSorted));
				PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
				foreach (int defaultValue in selectedIndicesSorted)
				{
					pdfArray.Add(new PdfInt(defaultValue));
				}
				base.Node.SortedIndices = pdfArray;
			}
			int[] defaultSelectedIndicesSorted = listBox.GetDefaultSelectedIndicesSorted();
			if (defaultSelectedIndicesSorted.Length > 0)
			{
				base.Node.DefaultFieldValue = new PrimitiveWrapper(this.GetSelectedValueArray(listBox.Options, defaultSelectedIndicesSorted));
			}
		}

		PdfArray GetSelectedValueArray(ChoiceOptionCollection options, int[] sortedSelectedIndices)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			foreach (int index in sortedSelectedIndices)
			{
				ChoiceOption choiceOption = options[index];
				pdfArray.Add(choiceOption.Value.ToPdfString());
			}
			return pdfArray;
		}

		void CopyFromText(TextField textField)
		{
			base.Node.FieldType = new PdfName("Tx");
			switch (textField.FieldType)
			{
			case FormFieldType.CombTextBox:
			{
				CombTextBoxField combTextBoxField = (CombTextBoxField)textField;
				this.FlagWriter.SetFlag(FieldFlag.Comb);
				base.Node.MaxLengthOfInputCharacters = new PdfInt(combTextBoxField.MaxLengthOfInputCharacters);
				break;
			}
			case FormFieldType.TextBox:
			{
				TextBoxField textBoxField = (TextBoxField)textField;
				this.FlagWriter.SetFlagOnCondition(FieldFlag.Multiline, textBoxField.IsMultiline);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.Password, textBoxField.IsPassword);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.FileSelect, textBoxField.IsFileSelect);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.DoNotSpellCheck, !textBoxField.ShouldSpellCheck);
				this.FlagWriter.SetFlagOnCondition(FieldFlag.DoNotScroll, !textBoxField.AllowScroll);
				if (textBoxField.MaxLengthOfInputCharacters != null)
				{
					base.Node.MaxLengthOfInputCharacters = new PdfInt(textBoxField.MaxLengthOfInputCharacters.Value);
				}
				break;
			}
			}
			if (textField.Value != null)
			{
				base.Node.FieldValue = new PrimitiveWrapper(textField.Value.ToPdfString());
			}
			if (textField.DefaultValue != null)
			{
				base.Node.DefaultFieldValue = new PrimitiveWrapper(textField.DefaultValue.ToPdfString());
			}
		}

		void CopyFromSignature(SignatureField signatureField)
		{
			base.Node.FieldType = new PdfName("Sig");
			if (signatureField.Signature != null)
			{
				SignatureObject signatureObject = new SignatureObject();
				signatureObject.CopyPropertiesFrom(signatureField.Signature);
				IndirectObject indirectObject = base.Context.CreateIndirectObject(signatureObject);
				base.Node.FieldValue = new PrimitiveWrapper(indirectObject.Reference);
			}
		}
	}
}
