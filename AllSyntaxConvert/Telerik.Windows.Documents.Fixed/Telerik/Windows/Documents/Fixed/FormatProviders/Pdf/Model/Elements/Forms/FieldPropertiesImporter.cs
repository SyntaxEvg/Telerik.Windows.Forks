using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DigitalSignature;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class FieldPropertiesImporter : VariableTextObjectPropertiesImporter<FormFieldNode>
	{
		public FieldPropertiesImporter(FormFieldNode node, PostScriptReader reader, IRadFixedDocumentImportContext context)
			: base(node, reader, context)
		{
		}

		public void ImportFieldProperties(FormField field)
		{
			field.TextProperties = base.ReadTextProperties();
			switch (field.FieldType)
			{
			case FormFieldType.PushButton:
			case FormFieldType.CheckBox:
			case FormFieldType.RadioButton:
				this.InitializeButtonFieldProperties(field);
				return;
			case FormFieldType.CombTextBox:
			case FormFieldType.TextBox:
				this.InitializeTextFieldProperties((TextField)field);
				return;
			case FormFieldType.ComboBox:
			case FormFieldType.ListBox:
				this.InitializeChoiceFieldProperties((ChoiceField)field);
				return;
			case FormFieldType.Signature:
				this.InitializeSignatureFieldProperties((SignatureField)field);
				return;
			default:
				throw new NotSupportedException(string.Format("Not supported field type: {0}", field.FieldType));
			}
		}

		void InitializeButtonFieldProperties(FormField field)
		{
			switch (field.FieldType)
			{
			case FormFieldType.CheckBox:
			{
				CheckBoxField checkBoxField = (CheckBoxField)field;
				this.InitializeCheckBoxFieldProperties(checkBoxField);
				return;
			}
			case FormFieldType.RadioButton:
			{
				RadioButtonField radioButtonField = (RadioButtonField)field;
				this.InitializeRadioButtonFieldProperties(radioButtonField);
				return;
			}
			default:
				return;
			}
		}

		void InitializeRadioButtonFieldProperties(RadioButtonField radioButtonField)
		{
			string value = base.Node.FieldValue.ToText();
			string value2 = base.Node.DefaultFieldValue.ToText();
			if (base.Node.Options != null)
			{
				this.ImportOptionsArray(radioButtonField);
				int num = 0;
				using (IEnumerator<WidgetObject> enumerator = base.Context.GetChildWidgets(base.Node).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WidgetObject widgetObject = enumerator.Current;
						RadioButtonWidget radioButtonWidget = (RadioButtonWidget)widgetObject.ToWidget(base.Reader, base.Context);
						radioButtonWidget.Option = radioButtonField.Options[num];
						string text;
						if (!string.IsNullOrEmpty(value) && radioButtonField.Value == null && widgetObject.TryGetOnStateName(out text) && text.Equals(value))
						{
							radioButtonField.Value = radioButtonWidget.Option;
						}
						if (!string.IsNullOrEmpty(value2) && radioButtonField.DefaultValue == null && widgetObject.TryGetOnStateName(out text) && text.Equals(value2))
						{
							radioButtonField.DefaultValue = radioButtonWidget.Option;
						}
						num++;
					}
					return;
				}
			}
			foreach (WidgetObject widgetObject2 in base.Context.GetChildWidgets(base.Node))
			{
				string text2;
				if (widgetObject2.TryGetOnStateName(out text2))
				{
					RadioOption radioOption = new RadioOption(text2);
					radioButtonField.Options.Add(radioOption);
					RadioButtonWidget radioButtonWidget2 = (RadioButtonWidget)widgetObject2.ToWidget(base.Reader, base.Context);
					radioButtonWidget2.Option = radioOption;
					if (!string.IsNullOrEmpty(value) && radioButtonField.Value == null && text2.Equals(value))
					{
						radioButtonField.Value = radioOption;
					}
					if (!string.IsNullOrEmpty(value2) && radioButtonField.DefaultValue == null && text2.Equals(value2))
					{
						radioButtonField.Value = radioOption;
					}
				}
			}
		}

		void ImportOptionsArray(RadioButtonField radioButtonField)
		{
			foreach (PdfPrimitive pdfPrimitive in base.Node.Options)
			{
				PdfString pdfString;
				if (pdfPrimitive.Type == PdfElementType.String)
				{
					pdfString = (PdfString)pdfPrimitive;
				}
				else
				{
					PdfArray pdfArray = (PdfArray)pdfPrimitive;
					pdfString = (PdfString)pdfArray[0];
				}
				radioButtonField.Options.Add(new RadioOption(pdfString.ToString()));
			}
		}

		void InitializeCheckBoxFieldProperties(CheckBoxField checkBoxField)
		{
			bool flag = false;
			if (base.Node.Options != null)
			{
				checkBoxField.ExportValue = base.Node.Options[0].ToString();
				flag = true;
			}
			string text = base.Node.FieldValue.ToText();
			if (text != null && text != "Off")
			{
				checkBoxField.IsChecked = true;
				if (!flag)
				{
					checkBoxField.ExportValue = text;
				}
			}
			string text2 = base.Node.DefaultFieldValue.ToText();
			checkBoxField.IsCheckedByDefault = text2 != null && text2 != "Off";
			if (!flag)
			{
				foreach (WidgetObject widgetObject in base.Context.GetChildWidgets(base.Node))
				{
					string exportValue;
					if (widgetObject.TryGetOnStateName(out exportValue))
					{
						checkBoxField.ExportValue = exportValue;
						break;
					}
				}
			}
		}

		void InitializeChoiceFieldProperties(ChoiceField field)
		{
			this.InitializeChoiceOptionCollection(field.Options);
			switch (field.FieldType)
			{
			case FormFieldType.ComboBox:
			{
				ComboBoxField comboBoxField = (ComboBoxField)field;
				this.InitializeComboBoxFieldProperties(comboBoxField);
				return;
			}
			case FormFieldType.ListBox:
			{
				ListBoxField listBoxField = (ListBoxField)field;
				this.InitializeListBoxFieldProperties(listBoxField);
				return;
			}
			default:
				return;
			}
		}

		void InitializeComboBoxFieldProperties(ComboBoxField comboBoxField)
		{
			string text = base.Node.FieldValue.ToText();
			ChoiceOption choiceOption = null;
			if (base.Node.SortedIndices != null)
			{
				int value = ((PdfInt)base.Node.SortedIndices[0]).Value;
				choiceOption = comboBoxField.Options[value];
			}
			if (text != null)
			{
				int index;
				if (choiceOption != null && choiceOption.Value.Equals(text))
				{
					comboBoxField.Value = choiceOption;
				}
				else if (comboBoxField.Options.TryGetValueIndex(text, out index))
				{
					comboBoxField.Value = comboBoxField.Options[index];
				}
				else
				{
					comboBoxField.Value = new ChoiceOption(text);
				}
			}
			else if (choiceOption != null)
			{
				comboBoxField.Value = choiceOption;
			}
			string text2 = base.Node.DefaultFieldValue.ToText();
			int index2;
			if (text2 != null && comboBoxField.Options.TryGetValueIndex(text2, out index2))
			{
				comboBoxField.DefaultValue = comboBoxField.Options[index2];
			}
		}

		void InitializeListBoxFieldProperties(ListBoxField listBoxField)
		{
			if (base.Node.SortedIndices != null)
			{
				listBoxField.Value = new ChoiceOption[base.Node.SortedIndices.Count];
				for (int i = 0; i < listBoxField.Value.Length; i++)
				{
					int value = ((PdfInt)base.Node.SortedIndices[i]).Value;
					listBoxField.Value[i] = listBoxField.Options[value];
				}
			}
			else
			{
				listBoxField.Value = this.InitializeListBoxValueFromValuesPrimitive(listBoxField.Options, base.Node.FieldValue);
			}
			if (base.Node.DefaultFieldValue != null)
			{
				listBoxField.DefaultValue = this.InitializeListBoxValueFromValuesPrimitive(listBoxField.Options, base.Node.DefaultFieldValue);
			}
			if (base.Node.TopIndex != null)
			{
				listBoxField.TopIndex = base.Node.TopIndex.Value;
			}
		}

		ChoiceOption[] InitializeListBoxValueFromValuesPrimitive(ChoiceOptionCollection options, PrimitiveWrapper valuesPrimitive)
		{
			ChoiceOption[] array = null;
			if (valuesPrimitive != null)
			{
				HashSet<int> hashSet = new HashSet<int>();
				PdfElementType type = valuesPrimitive.Type;
				string value;
				int item;
				if (type == PdfElementType.Array)
				{
					PdfArray pdfArray = (PdfArray)valuesPrimitive.Primitive;
					using (IEnumerator<PdfPrimitive> enumerator = pdfArray.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PdfPrimitive pdfPrimitive = enumerator.Current;
							value = pdfPrimitive.ToString();
							if (options.TryGetValueIndex(value, out item))
							{
								hashSet.Add(item);
							}
						}
						goto IL_8D;
					}
				}
				value = valuesPrimitive.Primitive.ToString();
				if (options.TryGetValueIndex(value, out item))
				{
					hashSet.Add(item);
				}
				IL_8D:
				if (hashSet.Count > 0)
				{
					int num = 0;
					array = new ChoiceOption[hashSet.Count];
					foreach (int index in from x in hashSet
						orderby x
						select x)
					{
						array[num] = options[index];
						num++;
					}
				}
			}
			return array;
		}

		void InitializeChoiceOptionCollection(ChoiceOptionCollection optionCollection)
		{
			if (base.Node.Options != null)
			{
				foreach (PdfPrimitive pdfPrimitive in base.Node.Options)
				{
					PdfElementType type = pdfPrimitive.Type;
					ChoiceOption choiceOption;
					if (type == PdfElementType.Array)
					{
						PdfArray pdfArray = (PdfArray)pdfPrimitive;
						choiceOption = new ChoiceOption(pdfArray[0].ToString());
						if (pdfArray.Count > 1)
						{
							choiceOption.UserInterfaceValue = pdfArray[1].ToString();
						}
					}
					else
					{
						choiceOption = new ChoiceOption(pdfPrimitive.ToString());
					}
					optionCollection.Add(choiceOption);
				}
			}
		}

		void InitializeTextFieldProperties(TextField field)
		{
			field.Value = base.Node.FieldValue.ToText();
			field.DefaultValue = base.Node.DefaultFieldValue.ToText();
		}

		void InitializeSignatureFieldProperties(SignatureField signatureField)
		{
			if (base.Node.FieldValue != null)
			{
				signatureField.Signature = new Signature();
				SignatureObject signatureObject = new SignatureObject();
				signatureObject.Load(base.Reader, base.Context, base.Node.FieldValue.Primitive);
				signatureObject.CopyPropertiesTo(signatureField.Signature, base.Reader, base.Context);
			}
		}
	}
}
