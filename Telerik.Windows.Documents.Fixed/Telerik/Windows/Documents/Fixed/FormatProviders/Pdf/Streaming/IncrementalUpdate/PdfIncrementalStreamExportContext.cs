using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming.IncrementalUpdate
{
	class PdfIncrementalStreamExportContext : PdfExportContext, IResourceRenamingExportContext
	{
		public PdfIncrementalStreamExportContext(Stream stream, int initialMaxObjectNumber)
			: base(null, new PdfExportSettings())
		{
			this.writer = new PdfWriter(stream);
			this.initialMaxObjectNumber = initialMaxObjectNumber;
			this.sourceToContextReferenceMapping = new Dictionary<IndirectReference, IndirectReference>();
		}

		public PdfWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		public Signature SignatureToUpdate
		{
			get
			{
				return this.signatureToUpdate;
			}
			set
			{
				Guard.ThrowExceptionIfNotNull<Signature>(this.signatureToUpdate, "signatureToUpdate");
				this.signatureToUpdate = value;
			}
		}

		public void RegisterIndirectReference(PdfPrimitive primitive, int objectNumber, bool writeObject)
		{
			base.CreateIndirectObject(primitive, objectNumber, writeObject, writeObject);
		}

		public void AddSourceToContextReferenceMapping(PdfFileSource source, IndirectReference sourceReference, IndirectReference contextReference)
		{
			this.sourceToContextReferenceMapping.Add(sourceReference, contextReference);
		}

		public bool TryGetContextIndirectReference(PdfFileSource source, IndirectReference sourceReference, out IndirectReference contextReference)
		{
			if (!this.sourceToContextReferenceMapping.TryGetValue(sourceReference, out contextReference))
			{
				contextReference = sourceReference;
			}
			return true;
		}

		protected override int GetNextObjectNumber()
		{
			return Math.Max(this.initialMaxObjectNumber + 1, base.GetNextObjectNumber());
		}

		readonly PdfWriter writer;

		readonly int initialMaxObjectNumber;

		readonly Dictionary<IndirectReference, IndirectReference> sourceToContextReferenceMapping;

		Signature signatureToUpdate;
	}
}
