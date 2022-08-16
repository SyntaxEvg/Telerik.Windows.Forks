using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms
{
	class FormFieldPropertiesEditor : IDisposable
	{
		internal FormFieldPropertiesEditor(PdfIncrementalStreamExportContext context, PdfFormFieldSource field)
		{
			this.field = field;
			this.context = context;
			this.disposeValidator = new DisposeValidator();
		}

		protected PdfFormFieldSource Field
		{
			get
			{
				return this.field;
			}
		}

		protected PdfIncrementalStreamExportContext Context
		{
			get
			{
				return this.context;
			}
		}

		protected PdfDictionary GetClonedFieldDictionary()
		{
			this.EnsureClonedFieldDictionary();
			return this.clonedFieldDictionary;
		}

		protected PdfDictionary GetClonedWidgetDictionary(int widgetIndex)
		{
			if (this.Field.HasMergedWidgetDictionary)
			{
				Guard.ThrowExceptionIfNotEqual<int>(0, widgetIndex, "widgetIndex");
				return this.GetClonedFieldDictionary();
			}
			this.EnsureClonedWidget(widgetIndex);
			return this.clonedWidgetsDictionaries[widgetIndex];
		}

		public void Dispose()
		{
			this.disposeValidator.Dispose();
			this.WriteModifiedObjects();
		}

		void WriteModifiedObjects()
		{
			if (this.clonedFieldDictionary != null)
			{
				this.context.RegisterIndirectReference(this.clonedFieldDictionary, this.Field.IndirectObject.Reference.ObjectNumber, true);
			}
			if (this.clonedWidgetsDictionaries != null)
			{
				for (int i = 0; i < this.clonedWidgetsDictionaries.Length; i++)
				{
					PdfDictionary pdfDictionary = this.clonedWidgetsDictionaries[i];
					if (pdfDictionary != null)
					{
						IndirectObject indirectObject = this.Field.WidgetObjects[i];
						this.context.RegisterIndirectReference(pdfDictionary, indirectObject.Reference.ObjectNumber, true);
					}
				}
			}
			PdfExporter.WritePendingIndirectObjects(this.context, this.context.Writer);
		}

		void EnsureClonedFieldDictionary()
		{
			if (this.clonedFieldDictionary == null)
			{
				this.clonedFieldDictionary = ResourceRenamer.GetDictionaryWithRenamedResources(this.Field.PdfDictionary, this.Field.FileSource, this.context);
			}
		}

		void EnsureClonedWidget(int widgetIndex)
		{
			if (this.clonedWidgetsDictionaries == null)
			{
				this.clonedWidgetsDictionaries = new PdfDictionary[this.Field.WidgetObjects.Length];
			}
			if (this.clonedWidgetsDictionaries[widgetIndex] == null)
			{
				PdfDictionary content = this.Field.WidgetObjects[widgetIndex].GetContent<PdfDictionary>();
				PdfDictionary dictionaryWithRenamedResources = ResourceRenamer.GetDictionaryWithRenamedResources(content, this.Field.FileSource, this.Context);
				this.clonedWidgetsDictionaries[widgetIndex] = dictionaryWithRenamedResources;
			}
		}

		readonly DisposeValidator disposeValidator;

		readonly PdfIncrementalStreamExportContext context;

		readonly PdfFormFieldSource field;

		PdfDictionary[] clonedWidgetsDictionaries;

		PdfDictionary clonedFieldDictionary;
	}
}
