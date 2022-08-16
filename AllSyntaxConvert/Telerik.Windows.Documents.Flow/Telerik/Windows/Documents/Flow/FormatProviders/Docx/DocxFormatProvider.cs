using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Validation;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx
{
	public class DocxFormatProvider : BinaryFormatProviderBase<RadFlowDocument>
	{
		public DocxFormatProvider()
		{
			this.ExportSettings = new DocxExportSettings();
		}

		public override IEnumerable<string> SupportedExtensions
		{
			get
			{
				return DocxFormatProvider.supportedExtensions;
			}
		}

		public override bool CanImport
		{
			get
			{
				return true;
			}
		}

		public override bool CanExport
		{
			get
			{
				return true;
			}
		}

		public DocxExportSettings ExportSettings { get; set; }

		protected override RadFlowDocument ImportOverride(Stream input)
		{
			Guard.ThrowExceptionIfNull<Stream>(input, "input");
			DocxImporter docxImporter = new DocxImporter();
			RadFlowDocumentImportContext radFlowDocumentImportContext = new RadFlowDocumentImportContext();
			docxImporter.Import(input, radFlowDocumentImportContext);
			return radFlowDocumentImportContext.Document;
		}

		protected override void ExportOverride(RadFlowDocument document, Stream output)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			this.ValidateAndRepairDocument(document);
			using (new RadFlowDocumentLicenseCheck(document))
			{
				DocxExporter docxExporter = new DocxExporter();
				RadFlowDocumentExportContext context = new RadFlowDocumentExportContext(document, this.ExportSettings);
				docxExporter.Export(output, context, this.ExportSettings);
			}
		}

		void ValidateAndRepairDocument(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			switch (this.ExportSettings.InvalidDocumentAction)
			{
			case InvalidDocumentAction.Repair:
				DocxValidator.Repair(document);
				return;
			case InvalidDocumentAction.ThrowException:
			{
				ValidationResult validationResult = DocxValidator.Validate(document);
				if (validationResult.ResultType == ValidationResultType.Error)
				{
					throw new InvalidDocumentException(validationResult.ValidationErrors);
				}
				return;
			}
			default:
				return;
			}
		}

		static readonly IEnumerable<string> supportedExtensions = new string[] { ".docx" };
	}
}
