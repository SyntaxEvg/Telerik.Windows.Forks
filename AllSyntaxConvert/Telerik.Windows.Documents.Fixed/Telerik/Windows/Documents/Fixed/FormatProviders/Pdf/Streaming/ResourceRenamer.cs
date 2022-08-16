using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser.Keywords;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	static class ResourceRenamer
	{
		public static void FillEmptyDictionaryWithRenamedResources(PdfDictionary emptyDictionary, PdfDictionary original, PdfFileSource fileSource, IResourceRenamingExportContext context)
		{
			ResourceRenamer.RenamingContext renamingContext = new ResourceRenamer.RenamingContext(fileSource, context);
			ResourceRenamer.FillEmptyDictionaryWithRenamedResources(emptyDictionary, original, renamingContext);
		}

		public static PdfDictionary GetDictionaryWithRenamedResources(PdfDictionary original, PdfFileSource fileSource, IResourceRenamingExportContext context)
		{
			ResourceRenamer.RenamingContext renamingContext = new ResourceRenamer.RenamingContext(fileSource, context);
			return ResourceRenamer.GetDictionaryWithRenamedResources(original, renamingContext);
		}

		public static PdfArray GetArrayWithRenamedResources(PdfArray original, PdfFileSource fileSource, IResourceRenamingExportContext context)
		{
			ResourceRenamer.RenamingContext renamingContext = new ResourceRenamer.RenamingContext(fileSource, context);
			return ResourceRenamer.GetArrayWithRenamedResources(original, renamingContext);
		}

		static PdfDictionary GetDictionaryWithRenamedResources(PdfDictionary original, ResourceRenamer.RenamingContext renamingContext)
		{
			PdfDictionary pdfDictionary = new PdfDictionary();
			ResourceRenamer.FillEmptyDictionaryWithRenamedResources(pdfDictionary, original, renamingContext);
			return pdfDictionary;
		}

		static PdfArray GetArrayWithRenamedResources(PdfArray original, ResourceRenamer.RenamingContext renamingContext)
		{
			PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
			ResourceRenamer.FillEmptyArrayWithRenamedResources(pdfArray, original, renamingContext);
			return pdfArray;
		}

		static void FillEmptyDictionaryWithRenamedResources(PdfDictionary emptyDictionary, PdfDictionary original, ResourceRenamer.RenamingContext renamingContext)
		{
			foreach (KeyValuePair<string, PdfPrimitive> keyValuePair in original)
			{
				PdfPrimitive primitiveWithRenamedResources = ResourceRenamer.GetPrimitiveWithRenamedResources(keyValuePair.Value, renamingContext);
				emptyDictionary[keyValuePair.Key] = primitiveWithRenamedResources;
			}
		}

		static void FillEmptyArrayWithRenamedResources(PdfArray emptyArray, PdfArray original, ResourceRenamer.RenamingContext renamingContext)
		{
			foreach (PdfPrimitive original2 in original)
			{
				PdfPrimitive primitiveWithRenamedResources = ResourceRenamer.GetPrimitiveWithRenamedResources(original2, renamingContext);
				emptyArray.Add(primitiveWithRenamedResources);
			}
		}

		static PdfStream CreateStreamWithEmptyDictionary(PdfStream original, ResourceRenamer.RenamingContext renamingContext)
		{
			Stream stream = original.Stream;
			PdfInt length = original.Length;
			long offset = original.Offset;
			if (renamingContext.FileSource.Context.Encryption != null)
			{
				byte[] array = original.ReadRawPdfData();
				length = new PdfInt(array.Length);
				offset = 0L;
				stream = new MemoryStream();
				stream.Write(array, 0, array.Length);
				stream.Write(StreamStartKeyword.EndStreamKeywordBytes, 0, "endstream".Length);
			}
			return new PdfStream(renamingContext.FileSource.Context, stream, new PdfDictionary(), length, offset);
		}

		static PdfPrimitive GetPrimitiveWithRenamedResources(PdfPrimitive original, ResourceRenamer.RenamingContext renamingContext)
		{
			if (original == null)
			{
				return PdfNull.Instance;
			}
			PdfElementType type = original.Type;
			switch (type)
			{
			case PdfElementType.Stream:
			{
				PdfStream pdfStream = (PdfStream)original;
				PdfStream pdfStream2 = ResourceRenamer.CreateStreamWithEmptyDictionary(pdfStream, renamingContext);
				ResourceRenamer.FillEmptyDictionaryWithRenamedResources(pdfStream2.Dictionary, pdfStream.Dictionary, renamingContext);
				return pdfStream2;
			}
			case PdfElementType.PdfName:
				break;
			case PdfElementType.Dictionary:
			{
				PdfDictionary original2 = (PdfDictionary)original;
				return ResourceRenamer.GetDictionaryWithRenamedResources(original2, renamingContext);
			}
			case PdfElementType.Array:
			{
				PdfArray original3 = (PdfArray)original;
				return ResourceRenamer.GetArrayWithRenamedResources(original3, renamingContext);
			}
			default:
				if (type == PdfElementType.IndirectReference)
				{
					IndirectReference sourceReference = (IndirectReference)original;
					return ResourceRenamer.GetContextReference(sourceReference, renamingContext);
				}
				break;
			}
			return original;
		}

		static PdfPrimitive GetContextReference(IndirectReference sourceReference, ResourceRenamer.RenamingContext renamingContext)
		{
			IndirectReference result;
			if (!renamingContext.ExportContext.TryGetContextIndirectReference(renamingContext.FileSource, sourceReference, out result))
			{
				if (renamingContext.FileSource.IsPageTreeReference(sourceReference) || renamingContext.FileSource.Context.Root.Reference.Equals(sourceReference))
				{
					return PdfNull.Instance;
				}
				result = ResourceRenamer.AddContextIndirectReference(sourceReference, renamingContext);
			}
			return result;
		}

		static IndirectReference AddContextIndirectReference(IndirectReference sourceReference, ResourceRenamer.RenamingContext renamingContext)
		{
			IndirectObject indirectObject = renamingContext.FileSource.Context.ReadIndirectObject(sourceReference);
			PdfPrimitive pdfPrimitive = indirectObject.Content ?? PdfNull.Instance;
			IndirectReference result;
			switch (pdfPrimitive.Type)
			{
			case PdfElementType.Stream:
			{
				PdfStream pdfStream = (PdfStream)pdfPrimitive;
				PdfStream pdfStream2 = ResourceRenamer.CreateStreamWithEmptyDictionary(pdfStream, renamingContext);
				result = ResourceRenamer.CreateContextReferenceMapping(pdfStream2, sourceReference, renamingContext);
				ResourceRenamer.FillEmptyDictionaryWithRenamedResources(pdfStream2.Dictionary, pdfStream.Dictionary, renamingContext);
				return result;
			}
			case PdfElementType.Dictionary:
			{
				PdfDictionary original = (PdfDictionary)pdfPrimitive;
				PdfDictionary pdfDictionary = new PdfDictionary();
				result = ResourceRenamer.CreateContextReferenceMapping(pdfDictionary, sourceReference, renamingContext);
				ResourceRenamer.FillEmptyDictionaryWithRenamedResources(pdfDictionary, original, renamingContext);
				return result;
			}
			case PdfElementType.Array:
			{
				PdfArray original2 = (PdfArray)pdfPrimitive;
				PdfArray pdfArray = new PdfArray(new PdfPrimitive[0]);
				result = ResourceRenamer.CreateContextReferenceMapping(pdfArray, sourceReference, renamingContext);
				ResourceRenamer.FillEmptyArrayWithRenamedResources(pdfArray, original2, renamingContext);
				return result;
			}
			}
			PdfPrimitive contextPrimitive = pdfPrimitive;
			result = ResourceRenamer.CreateContextReferenceMapping(contextPrimitive, sourceReference, renamingContext);
			return result;
		}

		static IndirectReference CreateContextReferenceMapping(PdfPrimitive contextPrimitive, IndirectReference sourceReference, ResourceRenamer.RenamingContext renamingContext)
		{
			IndirectObject indirectObject = renamingContext.ExportContext.CreateIndirectObject(contextPrimitive);
			IndirectReference reference = indirectObject.Reference;
			renamingContext.ExportContext.AddSourceToContextReferenceMapping(renamingContext.FileSource, sourceReference, reference);
			return reference;
		}

		class RenamingContext
		{
			public RenamingContext(PdfFileSource fileSource, IResourceRenamingExportContext exportContext)
			{
				this.FileSource = fileSource;
				this.ExportContext = exportContext;
			}

			public readonly PdfFileSource FileSource;

			public readonly IResourceRenamingExportContext ExportContext;
		}
	}
}
