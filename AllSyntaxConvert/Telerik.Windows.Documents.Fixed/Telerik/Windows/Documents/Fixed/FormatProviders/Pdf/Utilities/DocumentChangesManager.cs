using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Fixed.Model.Resources;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities
{
	class DocumentChangesManager
	{
		internal DocumentChangesManager(RadFixedDocument document, Stream input)
		{
			this.document = document;
			this.inputStream = input;
			this.modifiedFieldsNames = new HashSet<string>();
			this.modifiedPages = new Dictionary<int, DocumentChangesManager.PagePropertyChanges>();
		}

		public bool HasChanges
		{
			get
			{
				return this.modifiedFieldsNames.Count > 0 || this.modifiedPages.Count > 0;
			}
		}

		public void MarkFieldAsModified(string fieldName)
		{
			this.modifiedFieldsNames.Add(fieldName);
		}

		public void MarkPageRotationAsModified(int pageIndex)
		{
			DocumentChangesManager.PagePropertyChanges pagePropertyChanges = this.GetPagePropertyChanges(pageIndex);
			pagePropertyChanges.IsRotationChanged = true;
		}

		public void MarkPageSizeAsModified(int pageIndex)
		{
			DocumentChangesManager.PagePropertyChanges pagePropertyChanges = this.GetPagePropertyChanges(pageIndex);
			pagePropertyChanges.IsSizeChanged = true;
		}

		public void SaveChanges(Stream outputStream)
		{
			using (PdfFileSource pdfFileSource = new PdfFileSource(this.inputStream, true))
			{
				using (PdfIncrementalStreamWriter pdfIncrementalStreamWriter = new PdfIncrementalStreamWriter(pdfFileSource, outputStream, true))
				{
					foreach (string fieldName in this.modifiedFieldsNames)
					{
						FormField field = this.document.AcroForm.FormFields[fieldName];
						this.WriteFieldValueChanges(pdfIncrementalStreamWriter, field);
					}
					foreach (int pageIndex in this.modifiedPages.Keys)
					{
						this.WritePagePropertiesChanges(pdfIncrementalStreamWriter, pageIndex);
					}
				}
			}
		}

		void WriteFieldValueChanges(PdfIncrementalStreamWriter writer, FormField field)
		{
			switch (field.FieldType)
			{
			case FormFieldType.CheckBox:
				this.WriteCheckBoxChanges(writer, (CheckBoxField)field);
				return;
			case FormFieldType.RadioButton:
				this.WriteRadioButtonChanges(writer, (RadioButtonField)field);
				return;
			case FormFieldType.CombTextBox:
			case FormFieldType.TextBox:
				this.WriteTextFieldChanges(writer, (TextField)field);
				return;
			case FormFieldType.ComboBox:
				this.WriteComboBoxChanges(writer, (ComboBoxField)field);
				return;
			case FormFieldType.ListBox:
				this.WriteListBoxChanges(writer, (ListBoxField)field);
				return;
			default:
				return;
			}
		}

		void WriteCheckBoxChanges(PdfIncrementalStreamWriter writer, CheckBoxField field)
		{
			using (CheckBoxPropertiesEditor checkBoxPropertiesEditor = writer.EditCheckBoxProperties(field.Name))
			{
				checkBoxPropertiesEditor.SetIsChecked(field.IsChecked);
			}
		}

		void WriteRadioButtonChanges(PdfIncrementalStreamWriter writer, RadioButtonField field)
		{
			using (RadioButtonPropertiesEditor radioButtonPropertiesEditor = writer.EditRadioButtonProperties(field.Name))
			{
				if (field.Value == null)
				{
					radioButtonPropertiesEditor.ClearValue();
				}
				else
				{
					int num = 0;
					foreach (RadioButtonWidget radioButtonWidget in field.Widgets)
					{
						if (radioButtonWidget.IsSelected)
						{
							break;
						}
						num++;
					}
					if (num == field.Widgets.Count)
					{
						radioButtonPropertiesEditor.ClearValue();
					}
					else
					{
						radioButtonPropertiesEditor.SetValue(num);
					}
				}
			}
		}

		void WriteComboBoxChanges(PdfIncrementalStreamWriter writer, ComboBoxField field)
		{
			using (ComboBoxPropertiesEditor comboBoxPropertiesEditor = writer.EditComboBoxProperties(field.Name))
			{
				int value;
				if (field.Options.TryGetOptionIndex(field.Value, out value))
				{
					comboBoxPropertiesEditor.SetValue(value);
				}
				else
				{
					string value2 = (field.HasEditableTextBox ? ((field.Value == null) ? null : field.Value.Value) : null);
					comboBoxPropertiesEditor.SetValue(value2);
				}
				DocumentChangesManager.SetAppearances(comboBoxPropertiesEditor, field);
			}
		}

		void WriteListBoxChanges(PdfIncrementalStreamWriter writer, ListBoxField field)
		{
			using (ListBoxPropertiesEditor listBoxPropertiesEditor = writer.EditListBoxProperties(field.Name))
			{
				int[] selectedIndicesSorted = field.GetSelectedIndicesSorted();
				listBoxPropertiesEditor.SetValue(selectedIndicesSorted);
				DocumentChangesManager.SetAppearances(listBoxPropertiesEditor, field);
			}
		}

		void WriteTextFieldChanges(PdfIncrementalStreamWriter writer, TextField field)
		{
			using (TextFieldPropertiesEditor textFieldPropertiesEditor = writer.EditTextFieldProperties(field.Name))
			{
				textFieldPropertiesEditor.SetValue(field.Value);
				DocumentChangesManager.SetAppearances(textFieldPropertiesEditor, field);
			}
		}

		static void SetAppearances(SingleStateAppearanceFieldPropertiesEditor editor, FormField<VariableContentWidget> field)
		{
			editor.SetAppearance(from widget in field.Widgets
				select widget.Content.NormalContentSource);
		}

		void WritePagePropertiesChanges(PdfIncrementalStreamWriter writer, int pageIndex)
		{
			RadFixedPage radFixedPage = this.document.Pages[pageIndex];
			DocumentChangesManager.PagePropertyChanges pagePropertyChanges = this.GetPagePropertyChanges(pageIndex);
			using (PagePropertiesEditor pagePropertiesEditor = writer.EditPageProperties(pageIndex))
			{
				if (pagePropertyChanges.IsRotationChanged)
				{
					pagePropertiesEditor.SetRotation(radFixedPage.Rotation);
				}
				if (pagePropertyChanges.IsSizeChanged)
				{
					pagePropertiesEditor.SetMediaBox(radFixedPage.MediaBox);
				}
			}
		}

		DocumentChangesManager.PagePropertyChanges GetPagePropertyChanges(int pageIndex)
		{
			DocumentChangesManager.PagePropertyChanges pagePropertyChanges;
			if (!this.modifiedPages.TryGetValue(pageIndex, out pagePropertyChanges))
			{
				pagePropertyChanges = new DocumentChangesManager.PagePropertyChanges();
				this.modifiedPages.Add(pageIndex, pagePropertyChanges);
			}
			return pagePropertyChanges;
		}

		readonly RadFixedDocument document;

		readonly Stream inputStream;

		readonly HashSet<string> modifiedFieldsNames;

		readonly Dictionary<int, DocumentChangesManager.PagePropertyChanges> modifiedPages;

		class PagePropertyChanges
		{
			public bool IsRotationChanged { get; set; }

			public bool IsSizeChanged { get; set; }
		}
	}
}
