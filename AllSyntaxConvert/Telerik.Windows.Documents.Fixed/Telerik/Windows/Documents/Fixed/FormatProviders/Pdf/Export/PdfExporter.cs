using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.DigitalSignatures;
using Telerik.Windows.Documents.Fixed.Model.Fonts;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export
{
	class PdfExporter
	{
		public void Export(IRadFixedDocumentExportContext context, Stream output)
		{
			Guard.ThrowExceptionIfNull<Stream>(output, "output");
			Guard.ThrowExceptionIfNull<IRadFixedDocumentExportContext>(context, "context");
			PdfWriter pdfWriter = new PdfWriter(output);
			context.SignatureExportInfo.AddDocumentStartPosition((int)pdfWriter.Position);
			pdfWriter.WriteDocumentStart();
			//context.Settings.ComplianceLevel = PdfComplianceLevel.PdfA2U;
			if (context.Settings.ComplianceLevel == PdfComplianceLevel.None && context.Settings.IsEncrypted)
			{
				Encrypt encrypt = PdfExporter.CreateEncryptionBasedOnPdfSettings(context, context.Settings);
				context.CreateIndirectObject(encrypt);
				context.Encryption = encrypt;
			}
			DocumentCatalog documentCatalog = new DocumentCatalog();
			documentCatalog.CopyPropertiesFrom(context);
			PdfExporter.WriteCatalogAndRelatedProperties(pdfWriter, context, documentCatalog);
			PdfExporter.WriteFontsFromContext(pdfWriter, context);
			long position = pdfWriter.Position;
			bool flag = PdfExporter.HasSignatureField(context);
			if (flag)
			{
				PdfExporter.WriteCrossReferenceStream(pdfWriter, context, documentCatalog, null);
			}
			else
			{
				PdfExporter.WriteTrailerAndCrossReferenceTable(pdfWriter, context, documentCatalog, null);
			}
			PdfExporter.WriteDocumentEnd(position, pdfWriter, context);
			context.SignatureExportInfo.AddDocumentEndPosition((int)pdfWriter.Position - 1);
			PdfExporter.UpdateSignatures(pdfWriter, context);
		}

		internal static void WriteCatalogAndRelatedProperties(PdfWriter writer, IPdfExportContext context, DocumentCatalog catalog)
		{
			context.CreateIndirectObject(catalog);
			PdfExporter.WritePendingIndirectObjects(context, writer);
		}

		internal static void WriteFontsFromContext(PdfWriter writer, IPdfExportContext context)
		{
			foreach (FontBase fontBase in context.FontResources)
			{
				ResourceEntry resource = context.GetResource(fontBase);
				FontObject fontObject = resource.Resource.Content as FontObject;
				if (fontObject != null)
				{
					fontObject.CopyPropertiesFrom(context, fontBase);
				}
				context.IndirectObjectsQueue.Enqueue(resource.Resource);
			}
			PdfExporter.WritePendingIndirectObjects(context, writer);
		}

		internal static void WritePendingIndirectObjects(IPdfExportContext context, PdfWriter writer)
		{
			while (context.IndirectObjectsQueue.Count > 0)
			{
				IndirectObject indirectObject = context.IndirectObjectsQueue.Dequeue();
				indirectObject.Write(writer, context);
			}
		}

		internal static void WriteTrailerAndCrossReferenceTable(PdfWriter writer, IPdfExportContext context, DocumentCatalog root, PdfInt previousTablePosition)
		{
			writer.WriteLine("xref");
			IEnumerable<int> orderedObjectNumbers = from item in context.CrossReferenceCollection.Entries
				orderby item.Key
				select item.Key;
			IEnumerable<Tuple<int, int>> enumerable = PdfExporter.CalculateCrossReferenceSectionInfos(orderedObjectNumbers);
			foreach (Tuple<int, int> tuple in enumerable)
			{
				int item3 = tuple.Item1;
				int item2 = tuple.Item2;
				writer.WriteLine("{0} {1}", new object[] { item3, item2 });
				int num = item3 + item2 - 1;
				for (int i = item3; i <= num; i++)
				{
					CrossReferenceEntry crossReferenceEntry = context.CrossReferenceCollection.GetCrossReferenceEntry(i);
					crossReferenceEntry.Write(writer, context);
				}
			}
			Trailer trailer = new Trailer();
			trailer.CopyPropertiesFrom(context);
			trailer.Root = root;
			trailer.Prev = previousTablePosition;
			trailer.Write(writer, context);
			writer.WriteLine();
		}

		static IEnumerable<Tuple<int, int>> CalculateCrossReferenceSectionInfos(IEnumerable<int> orderedObjectNumbers)
		{
			int? previousId = null;
			int currentSectionCount = 0;
			foreach (int id in orderedObjectNumbers)
			{
				if (previousId == null)
				{
					goto IL_109;
				}
				int num = id;
				if (!(num != previousId + 1))
				{
					goto IL_109;
				}
				yield return new Tuple<int, int>(previousId.Value - currentSectionCount + 1, currentSectionCount);
				currentSectionCount = 1;
				IL_117:
				previousId = new int?(id);
				continue;
				IL_109:
				currentSectionCount++;
				goto IL_117;
			}
			if (previousId != null)
			{
				yield return new Tuple<int, int>(previousId.Value - currentSectionCount + 1, currentSectionCount);
			}
			yield break;
		}

		internal static void WriteCrossReferenceStream(PdfWriter writer, IPdfExportContext context, DocumentCatalog root, PdfInt previousCrossReferencePosition)
		{
			CrossReferenceStream crossReferenceStream = new CrossReferenceStream();
			context.CreateIndirectObject(crossReferenceStream);
			crossReferenceStream.CopyPropertiesFrom(context);
			crossReferenceStream.Root = root;
			crossReferenceStream.Prev = previousCrossReferencePosition;
			PdfExporter.WritePendingIndirectObjects(context, writer);
		}

		internal static void WriteDocumentEnd(long offset, PdfWriter writer, IPdfExportContext context)
		{
			writer.WriteLine("startxref");
			writer.WriteLine("{0}", new object[] { offset });
			writer.WriteDocumentEnd();
		}

		static Encrypt CreateEncryptionBasedOnPdfSettings(IPdfExportContext context, PdfExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<IPdfExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<PdfExportSettings>(settings, "settings");
			Encrypt encrypt = new StandardEncrypt();
			encrypt.CopyPropertiesFrom(context, settings);
			return encrypt;
		}

		static bool HasSignatureField(IRadFixedDocumentExportContext context)
		{
			bool result = false;
			foreach (FormField formField in context.Document.AcroForm.FormFields)
			{
				if (formField.FieldType == FormFieldType.Signature)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		internal static void UpdateSignatures(PdfWriter writer, IRadFixedDocumentExportContext context)
		{
			foreach (FormField formField in context.Document.AcroForm.FormFields)
			{
				if (formField.FieldType == FormFieldType.Signature)
				{
					SignatureField signatureField = (SignatureField)formField;
					if (signatureField.Signature != null && signatureField.Signature.CanSign)
					{
						PdfExporter.UpdateSignatureByteRange(signatureField.Name, writer, context);
						PdfExporter.UpdateSignatureContent(signatureField.Signature, writer, context);
					}
				}
			}
		}

		internal static void UpdateSignatureByteRange(string singatureFieldName, PdfWriter writer, IPdfExportContext context)
		{
			writer.Seek(context.SignatureExportInfo.GetSignaturePositions(singatureFieldName).ByteRangeStartPosition, SeekOrigin.Begin);
			int[] array = context.SignatureExportInfo.ComposeByteRangeArray(singatureFieldName);
			new PdfArray(new PdfPrimitive[0])
			{
				new PdfInt(array[0]),
				new PdfInt(array[1]),
				new PdfInt(array[2]),
				new PdfInt(array[3])
			}.Write(writer, context);
			int num = (int)((long)context.SignatureExportInfo.GetSignaturePositions(singatureFieldName).ByteRangeEndPosition - writer.Position);
			for (int i = 0; i < num; i++)
			{
				writer.WriteSeparator();
			}
		}

		internal static void UpdateSignatureContent(Signature signature, PdfWriter writer, IPdfExportContext context)
		{
			int[] array = context.SignatureExportInfo.ComposeByteRangeArray(signature.Properties.FieldName);
			List<SourcePart> list = new List<SourcePart>();
			SourcePart item = new SourcePart(array[0], array[1]);
			SourcePart item2 = new SourcePart(array[2], array[3]);
			list.Add(item);
			list.Add(item2);
			ByteRangeComposer byteRangeComposer = new ByteRangeComposer(new FileSourceStream(writer.OutputStream), list);
			byte[] byteArrayCompositionForHash = byteRangeComposer.Compose();
			byte[] initialValue = signature.CalculateContentsPackage(byteArrayCompositionForHash);
			writer.Seek(context.SignatureExportInfo.GetSignaturePositions(signature.Properties.FieldName).ContentStartPosition, SeekOrigin.Begin);
			PdfHexString pdfHexString = new PdfHexString(initialValue);
			pdfHexString.Write(writer, context);
		}
	}
}
