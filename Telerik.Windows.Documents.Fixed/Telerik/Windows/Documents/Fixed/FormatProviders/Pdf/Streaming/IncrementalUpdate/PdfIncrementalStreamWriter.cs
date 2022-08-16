using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate
{
	sealed class PdfIncrementalStreamWriter : IDisposable
	{
		public PdfIncrementalStreamWriter(PdfFileSource initialFile, Stream resultStream)
			: this(initialFile, resultStream, false)
		{
		}

		public PdfIncrementalStreamWriter(PdfFileSource initialFile, Stream resultStream, bool leaveStreamOpen)
		{
			this.fileSource = initialFile;
			this.leaveStreamOpen = leaveStreamOpen;
			this.disposeValidator = new DisposeValidator();
			this.context = new PdfIncrementalStreamExportContext(resultStream, initialFile.Context.CrossReferences.MaxObjectNumber);
			this.WriteFileBeginning();
		}

		public PagePropertiesEditor EditPageProperties(int pageIndex)
		{
			PdfPageSource page = this.fileSource.Pages[pageIndex];
			return new PagePropertiesEditor(this.context, page);
		}

		public CheckBoxPropertiesEditor EditCheckBoxProperties(string fieldName)
		{
			PdfFormFieldSource field = this.fileSource.FormFields[fieldName];
			return new CheckBoxPropertiesEditor(this.context, field);
		}

		public RadioButtonPropertiesEditor EditRadioButtonProperties(string fieldName)
		{
			PdfFormFieldSource field = this.fileSource.FormFields[fieldName];
			return new RadioButtonPropertiesEditor(this.context, field);
		}

		public ComboBoxPropertiesEditor EditComboBoxProperties(string fieldName)
		{
			PdfFormFieldSource field = this.fileSource.FormFields[fieldName];
			return new ComboBoxPropertiesEditor(this.context, field);
		}

		public ListBoxPropertiesEditor EditListBoxProperties(string fieldName)
		{
			PdfFormFieldSource field = this.fileSource.FormFields[fieldName];
			return new ListBoxPropertiesEditor(this.context, field);
		}

		public TextFieldPropertiesEditor EditTextFieldProperties(string fieldName)
		{
			PdfFormFieldSource field = this.fileSource.FormFields[fieldName];
			return new TextFieldPropertiesEditor(this.context, field);
		}

		public SignaturePropertiesEditor EditSignatureProperties(string fieldName)
		{
			PdfFormFieldSource fieldSource = this.fileSource.FormFields[fieldName];
			return new SignaturePropertiesEditor(this.context, fieldSource);
		}

		public void Dispose()
		{
			try
			{
				this.disposeValidator.Dispose();
				this.WriteFileEnding();
			}
			finally
			{
				if (!this.leaveStreamOpen)
				{
					this.context.Writer.OutputStream.Dispose();
				}
			}
		}

		void WriteFileBeginning()
		{
			Stream stream = this.fileSource.Context.Reader.Reader.Stream;
			stream.Seek(0L, SeekOrigin.Begin);
			this.context.SignatureExportInfo.AddDocumentStartPosition((int)this.context.Writer.Position);
			stream.CopyTo(this.context.Writer.OutputStream);
		}

		void WriteFileEnding()
		{
			PdfExporter.WritePendingIndirectObjects(this.context, this.context.Writer);
			DocumentCatalog value = this.fileSource.Context.Root.GetValue();
			IndirectReference reference = this.fileSource.Context.Root.Reference;
			this.context.RegisterIndirectReference(value, reference.ObjectNumber, false);
			PdfExporter.WriteFontsFromContext(this.context.Writer, this.context);
			long position = this.context.Writer.Position;
			PdfInt pdfInt = new PdfInt(this.fileSource.Context.StartXRefOffset);
			bool startsWithCrossReferenceTable = this.fileSource.Context.StartsWithCrossReferenceTable;
			if (startsWithCrossReferenceTable)
			{
				PdfExporter.WriteTrailerAndCrossReferenceTable(this.context.Writer, this.context, value, pdfInt);
			}
			else
			{
				PdfExporter.WriteCrossReferenceStream(this.context.Writer, this.context, value, pdfInt);
			}
			PdfExporter.WriteDocumentEnd(position, this.context.Writer, this.context);
			this.context.SignatureExportInfo.AddDocumentEndPosition((int)this.context.Writer.Position - 1);
			this.UpdateSignatures(this.context.Writer, this.context);
		}

		void UpdateSignatures(PdfWriter writer, PdfIncrementalStreamExportContext context)
		{
			if (context.SignatureToUpdate != null)
			{
				PdfExporter.UpdateSignatureByteRange(context.SignatureToUpdate.Properties.FieldName, writer, context);
				PdfExporter.UpdateSignatureContent(context.SignatureToUpdate, writer, context);
			}
		}

		readonly PdfIncrementalStreamExportContext context;

		readonly PdfFileSource fileSource;

		readonly bool leaveStreamOpen;

		readonly DisposeValidator disposeValidator;
	}
}
