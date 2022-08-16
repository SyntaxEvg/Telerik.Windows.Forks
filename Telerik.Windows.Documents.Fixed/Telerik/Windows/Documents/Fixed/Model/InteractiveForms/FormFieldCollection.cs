using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Fixed.Model.InteractiveForms
{
	public class FormFieldCollection : IEnumerable<FormField>, IEnumerable
	{
		internal FormFieldCollection()
		{
			this.fields = new Dictionary<string, FormField>();
		}

		public FormField this[string fieldName]
		{
			get
			{
				return this.fields[fieldName];
			}
		}

		public PushButtonField AddPushButton(string fieldName)
		{
			PushButtonField pushButtonField = new PushButtonField(fieldName);
			this.Add(pushButtonField);
			return pushButtonField;
		}

		public CheckBoxField AddCheckBox(string fieldName)
		{
			CheckBoxField checkBoxField = new CheckBoxField(fieldName);
			this.Add(checkBoxField);
			return checkBoxField;
		}

		public RadioButtonField AddRadioButton(string fieldName)
		{
			RadioButtonField radioButtonField = new RadioButtonField(fieldName);
			this.Add(radioButtonField);
			return radioButtonField;
		}

		public CombTextBoxField AddCombTextBox(string fieldName)
		{
			CombTextBoxField combTextBoxField = new CombTextBoxField(fieldName);
			this.Add(combTextBoxField);
			return combTextBoxField;
		}

		public TextBoxField AddTextBox(string fieldName)
		{
			TextBoxField textBoxField = new TextBoxField(fieldName);
			this.Add(textBoxField);
			return textBoxField;
		}

		public ComboBoxField AddComboBox(string fieldName)
		{
			ComboBoxField comboBoxField = new ComboBoxField(fieldName);
			this.Add(comboBoxField);
			return comboBoxField;
		}

		public ListBoxField AddListBox(string fieldName)
		{
			ListBoxField listBoxField = new ListBoxField(fieldName);
			this.Add(listBoxField);
			return listBoxField;
		}

		public SignatureField AddSignature(string fieldName)
		{
			SignatureField signatureField = new SignatureField(fieldName);
			this.Add(signatureField);
			return signatureField;
		}

		public void Add(FormField field)
		{
			this.fields.Add(field.Name, field);
		}

		public void Remove(FormField field)
		{
			this.fields.Remove(field.Name);
		}

		public bool Contains(string fieldName)
		{
			return this.fields.ContainsKey(fieldName);
		}

		public int Count
		{
			get
			{
				return this.fields.Count;
			}
		}

		public IEnumerator<FormField> GetEnumerator()
		{
			return this.fields.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.fields.Values.GetEnumerator();
		}

		readonly Dictionary<string, FormField> fields;
	}
}
