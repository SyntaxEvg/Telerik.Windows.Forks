using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate
{
	class PagePropertiesEditor : IDisposable
	{
		public PagePropertiesEditor(PdfIncrementalStreamExportContext context, PdfPageSource page)
		{
			this.disposeValidator = new DisposeValidator();
			this.context = context;
			this.page = page;
			IndirectObject indirectObject = page.FileSource.Context.ReadIndirectObject(page.PageReference);
			this.originalPageDictionary = (PdfDictionary)indirectObject.Content;
		}

		public void SetMediaBox(Rect mediaBox)
		{
			if (!(this.page.MediaBox == mediaBox))
			{
				this.EnsureClonedPageDictionary();
				this.clonedPageDictionary["MediaBox"] = mediaBox.ToBottomLeftCoordinateSystem(mediaBox.Height);
			}
		}

		public void SetRotation(Rotation rotation)
		{
			if (this.page.Rotation != rotation)
			{
				this.EnsureClonedPageDictionary();
				this.clonedPageDictionary["Rotate"] = Page.GetRotate(rotation);
			}
		}

		public void Dispose()
		{
			this.disposeValidator.Dispose();
			this.WritePageObject();
		}

		void WritePageObject()
		{
			this.context.RegisterIndirectReference(this.clonedPageDictionary, this.page.PageReference.ObjectNumber, true);
			PdfExporter.WritePendingIndirectObjects(this.context, this.context.Writer);
		}

		void EnsureClonedPageDictionary()
		{
			if (this.clonedPageDictionary == null)
			{
				this.clonedPageDictionary = ResourceRenamer.GetDictionaryWithRenamedResources(this.originalPageDictionary, this.page.FileSource, this.context);
			}
		}

		readonly DisposeValidator disposeValidator;

		readonly PdfIncrementalStreamExportContext context;

		readonly PdfPageSource page;

		readonly PdfDictionary originalPageDictionary;

		PdfDictionary clonedPageDictionary;
	}
}
