using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.DocumentStructure;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Objects;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Utilities;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Fixed.Model.Annotations;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Utilities.Rendering;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	public sealed class PdfPageStreamWriter : IDisposable
	{
		internal PdfPageStreamWriter(PdfPageStreamWriterContext pageContext)
		{
			this.pageContext = pageContext;
			this.xObjectResources = new PdfDictionary();
			this.xFormResourceIds = new Dictionary<IndirectReference, string>();
			this.annotations = new PdfArray(new PdfPrimitive[0]);
			this.contentStream = new MemoryStream();
			this.contentWriter = new PdfWriter(this.contentStream);
			this.positionStack = new PositionStack(new SimplePosition());
			this.disposeValidator = new DisposeValidator();
			this.currentResourceId = 0;
		}

		public Size PageSize
		{
			get
			{
				return new Size(this.pageContext.MediaBox.Width, this.pageContext.MediaBox.Height);
			}
		}

		public Rect MediaBox
		{
			get
			{
				return this.pageContext.MediaBox;
			}
		}

		public Rect CropBox
		{
			get
			{
				return this.pageContext.CropBox;
			}
		}

		public Rotation PageRotation
		{
			get
			{
				return this.pageContext.PageRotation;
			}
		}

		public IPosition ContentPosition
		{
			get
			{
				return this.positionStack.Position;
			}
			set
			{
				this.positionStack.Position = value;
			}
		}

		PdfStreamWriterSettings Settings
		{
			get
			{
				return this.pageContext.Settings;
			}
		}

		PdfFileStreamExportContext ExportContext
		{
			get
			{
				return this.pageContext.ExportContext;
			}
		}

		public void WriteContent(PdfPageSource pdfPageSource)
		{
			Matrix? transformation = this.CalculateTransformationMatrix(pdfPageSource.Size);
			if (this.Settings.WriteAnnotations)
			{
				this.AppendAnnotations(pdfPageSource, transformation);
			}
			IndirectReference xformReference = this.GetXFormReference(pdfPageSource);
			this.AppendXFormContent(xformReference, transformation);
		}

		public void WriteContent(RadFixedPage page)
		{
			Matrix? transformation = this.CalculateTransformationMatrix(page.Size);
			if (this.Settings.WriteAnnotations)
			{
				this.AppendAnnotations(page, transformation);
			}
			IndirectReference xformReference = this.GetXFormReference(page);
			this.AppendXFormContent(xformReference, transformation);
		}

		public void Dispose()
		{
			this.disposeValidator.Dispose();
			if (this.ExportContext.LicenceMessageContentOwner != null)
			{
				this.WriteLicenseMessage(this.ExportContext.LicenceMessageContentOwner);
			}
			this.WritePdfPage();
			this.contentStream.Dispose();
			this.pageContext.DoOnPageWritingEndedAction();
		}

		public IDisposable SaveContentPosition()
		{
			return this.positionStack.SavePosition();
		}

		static T GetNonReferencePrimitive<T>(IPdfImportContext importContext, PdfPrimitive primitive) where T : PdfPrimitive
		{
			T t = primitive as T;
			if (t == null)
			{
				IndirectReference reference = (IndirectReference)primitive;
				IndirectObject indirectObject = importContext.ReadIndirectObject(reference);
				t = (T)((object)indirectObject.Content);
			}
			return t;
		}

		IndirectReference GetXFormReference(PdfPageSource pageSource)
		{
			IndirectReference reference;
			if (!this.ExportContext.TryGetXFormReference(pageSource, out reference))
			{
				FormXObject formXObject = this.CreateXFormWithContents(pageSource);
				Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(pageSource);
				formXObject.BoundingBox = visibleContentBox.ToBottomLeftCoordinateSystem(pageSource.Size.Height);
				if (pageSource.Page.Resources != null)
				{
					formXObject.CopyResourcesFrom(pageSource.Page, pageSource.FileSource, this.ExportContext);
				}
				reference = this.ExportContext.CreateIndirectObject(formXObject).Reference;
				this.ExportContext.AddXFormReference(pageSource, reference);
				PdfExporter.WritePendingIndirectObjects(this.ExportContext, this.ExportContext.Writer);
			}
			return reference;
		}

		IndirectReference GetXFormReference(RadFixedPage pageRoot)
		{
			IndirectReference reference;
			if (!this.ExportContext.TryGetXFormReference(pageRoot, out reference))
			{
				FormXObject formXObject = new FormXObject();
				Rect visibleContentBox = PageLayoutHelper.GetVisibleContentBox(pageRoot);
				formXObject.CopyPropertiesFrom(this.ExportContext, pageRoot, visibleContentBox);
				reference = this.ExportContext.CreateIndirectObject(formXObject).Reference;
				this.ExportContext.AddXFormReference(pageRoot, reference);
				PdfExporter.WritePendingIndirectObjects(this.ExportContext, this.ExportContext.Writer);
			}
			return reference;
		}

		void AppendAnnotations(PdfPageSource pdfPageSource, Matrix? transformation)
		{
			PdfSourceImportContext context = pdfPageSource.FileSource.Context;
			List<PdfDictionary> list;
			List<PdfDictionary> list2;
			this.RegisterOldAndNewAnnotationDictionariesMapping(pdfPageSource, out list, out list2);
			for (int i = 0; i < list.Count; i++)
			{
				PdfDictionary pdfDictionary = list[i];
				PdfDictionary pdfDictionary2 = list2[i];
				ResourceRenamer.FillEmptyDictionaryWithRenamedResources(pdfDictionary2, pdfDictionary, pdfPageSource.FileSource, this.ExportContext);
				if (transformation != null)
				{
					PdfPrimitive primitive = pdfDictionary["Rect"];
					PdfArray nonReferencePrimitive = PdfPageStreamWriter.GetNonReferencePrimitive<PdfArray>(context, primitive);
					Rect rect = nonReferencePrimitive.ToRect(context.Reader, context);
					Rect rect2 = transformation.Value.Transform(rect);
					pdfDictionary2["Rect"] = rect2.ToPdfArray();
				}
				IndirectObject indirectObject = this.ExportContext.CreateIndirectObject(pdfDictionary2);
				this.annotations.Add(indirectObject.Reference);
			}
		}

		void RegisterOldAndNewAnnotationDictionariesMapping(PdfPageSource pdfPageSource, out List<PdfDictionary> oldAnnotations, out List<PdfDictionary> newAnnotationsEmptyDictionaries)
		{
			oldAnnotations = new List<PdfDictionary>();
			newAnnotationsEmptyDictionaries = new List<PdfDictionary>();
			PdfDictionary pageDictionary = pdfPageSource.PageDictionary;
			PdfPrimitive primitive;
			if (pageDictionary.TryGetElement("Annots", out primitive))
			{
				PdfSourceImportContext context = pdfPageSource.FileSource.Context;
				PdfArray nonReferencePrimitive = PdfPageStreamWriter.GetNonReferencePrimitive<PdfArray>(context, primitive);
				foreach (PdfPrimitive pdfPrimitive in nonReferencePrimitive)
				{
					if (pdfPrimitive != null && pdfPrimitive.Type != PdfElementType.Null)
					{
						PdfDictionary pdfDictionary = new PdfDictionary();
						IndirectObject indirectObject = this.ExportContext.CreateIndirectObject(pdfDictionary);
						PdfDictionary item;
						if (pdfPrimitive.Type == PdfElementType.Dictionary)
						{
							item = (PdfDictionary)pdfPrimitive;
						}
						else
						{
							IndirectReference indirectReference = (IndirectReference)pdfPrimitive;
							IndirectObject indirectObject2 = context.ReadIndirectObject(indirectReference);
							item = (PdfDictionary)indirectObject2.Content;
							this.ExportContext.AddAnnotationDictionariesMappings(pdfPageSource, indirectReference, indirectObject.Reference);
						}
						oldAnnotations.Add(item);
						newAnnotationsEmptyDictionaries.Add(pdfDictionary);
					}
				}
			}
		}

		void AppendAnnotations(RadFixedPage fixedPage, Matrix? transformation)
		{
			foreach (Annotation annotation in fixedPage.Annotations)
			{
				AnnotationObject annotationObject = AnnotationFactory.CreatePdfAnnotation(this.ExportContext, annotation, fixedPage);
				if (transformation != null)
				{
					PdfReal x = (PdfReal)annotationObject.Rect[0];
					PdfReal y = (PdfReal)annotationObject.Rect[1];
					PdfReal x2 = (PdfReal)annotationObject.Rect[2];
					PdfReal y2 = (PdfReal)annotationObject.Rect[3];
					Rect rect = PdfObjectsExtensions.CreateRectFromArrayValues(x, y, x2, y2);
					Rect rect2 = transformation.Value.Transform(rect);
					annotationObject.Rect = rect2.ToPdfArray();
				}
				IndirectObject indirectObject = this.ExportContext.CreateIndirectObject(annotationObject);
				this.annotations.Add(indirectObject.Reference);
			}
		}

		FormXObject CreateXFormWithContents(PdfPageSource pdfPageSource)
		{
			PdfDictionary pageDictionary = pdfPageSource.PageDictionary;
			PdfPrimitive contentPrimitive;
			FormXObject result;
			if (pageDictionary.TryGetElement("Contents", out contentPrimitive))
			{
				PdfStream importedStream;
				if (this.TryGetSingleContentStream(pdfPageSource.FileSource, contentPrimitive, out importedStream))
				{
					result = new FormXObject(importedStream, pdfPageSource.FileSource, this.ExportContext);
				}
				else
				{
					result = new FormXObject(pdfPageSource.Page.Contents.Data);
				}
			}
			else
			{
				result = new FormXObject(new byte[0]);
			}
			return result;
		}

		bool TryGetSingleContentStream(PdfFileSource fileSource, PdfPrimitive contentPrimitive, out PdfStream importedStream)
		{
			bool result;
			if (contentPrimitive.Type == PdfElementType.IndirectReference)
			{
				IndirectReference reference = (IndirectReference)contentPrimitive;
				IndirectObject indirectObject = fileSource.Context.ReadIndirectObject(reference);
				if (indirectObject.Content.Type == PdfElementType.Array)
				{
					PdfArray contentArray = (PdfArray)indirectObject.Content;
					result = this.TryGetSingleContentStreamFromDirectArray(fileSource, contentArray, out importedStream);
				}
				else
				{
					importedStream = (PdfStream)indirectObject.Content;
					result = true;
				}
			}
			else
			{
				PdfArray contentArray2 = (PdfArray)contentPrimitive;
				result = this.TryGetSingleContentStreamFromDirectArray(fileSource, contentArray2, out importedStream);
			}
			return result;
		}

		bool TryGetSingleContentStreamFromDirectArray(PdfFileSource fileSource, PdfArray contentArray, out PdfStream importedStream)
		{
			if (contentArray.Count == 1)
			{
				IndirectReference reference = (IndirectReference)contentArray[0];
				IndirectObject indirectObject = fileSource.Context.ReadIndirectObject(reference);
				importedStream = (PdfStream)indirectObject.Content;
				return true;
			}
			importedStream = null;
			return false;
		}

		void AppendXFormContent(IndirectReference reference, Matrix? transformation)
		{
			if (transformation != null)
			{
				this.contentWriter.WriteLine("q");
				foreach (PdfReal pdfReal in PdfPageStreamWriter.GetMatrixOperands(transformation.Value))
				{
					pdfReal.Write(this.contentWriter, this.ExportContext);
					this.contentWriter.WriteSeparator();
				}
				this.contentWriter.Write("cm");
				this.contentWriter.WriteLine();
			}
			string xformResourceId = this.GetXFormResourceId(reference);
			this.contentWriter.WritePdfName(xformResourceId);
			this.contentWriter.WriteSeparator();
			this.contentWriter.Write("Do");
			this.contentWriter.WriteLine();
			if (transformation != null)
			{
				this.contentWriter.WriteLine("Q");
			}
		}

		static IEnumerable<PdfReal> GetMatrixOperands(Matrix matrix)
		{
			yield return new PdfReal(matrix.M11);
			yield return new PdfReal(matrix.M12);
			yield return new PdfReal(matrix.M21);
			yield return new PdfReal(matrix.M22);
			yield return new PdfReal(matrix.OffsetX);
			yield return new PdfReal(matrix.OffsetY);
			yield break;
		}

		Matrix? CalculateTransformationMatrix(Size formSize)
		{
			if (formSize.Equals(this.PageSize) && this.ContentPosition.Matrix.IsIdentity)
			{
				return null;
			}
			Matrix matrix = this.ContentPosition.Matrix;
			double offsetX = Unit.DipToPoint(matrix.M21 * formSize.Height + matrix.OffsetX);
			double offsetY = Unit.DipToPoint(this.PageSize.Height - matrix.M22 * formSize.Height - matrix.OffsetY);
			Matrix value = new Matrix(matrix.M11, -matrix.M12, -matrix.M21, matrix.M22, offsetX, offsetY);
			return new Matrix?(value);
		}

		string GetXFormResourceId(IndirectReference reference)
		{
			string text;
			if (!this.xFormResourceIds.TryGetValue(reference, out text))
			{
				text = string.Format("XForm{0}", this.currentResourceId++);
				this.xObjectResources[text] = reference;
				this.xFormResourceIds.Add(reference, text);
			}
			return text;
		}

		void WritePdfPage()
		{
			this.pageContext.PageObject.MediaBox = this.MediaBox.ToBottomLeftCoordinateSystem(this.PageSize.Height);
			this.pageContext.PageObject.CropBox = this.CropBox.ToBottomLeftCoordinateSystem(this.PageSize.Height);
			this.pageContext.PageObject.Rotate = Page.GetRotate(this.PageRotation);
			this.pageContext.PageObject.Contents = new ContentStream(this.contentStream.ToArray());
			this.pageContext.PageObject.Resources = new PdfResource();
			this.pageContext.PageObject.Resources.XObjects = this.xObjectResources;
			this.pageContext.PageObject.Annots = ((this.annotations.Count > 0) ? this.annotations : null);
			IndirectObject item = this.ExportContext.CreateIndirectObject(this.pageContext.PageObject);
			this.ExportContext.IndirectObjectsQueue.Enqueue(item);
			PdfExporter.WritePendingIndirectObjects(this.ExportContext, this.ExportContext.Writer);
		}

		void WriteLicenseMessage(RadFixedPage licenceMessageContentOwner)
		{
			using (this.SaveContentPosition())
			{
				this.ContentPosition = new SimplePosition();
				this.WriteContent(licenceMessageContentOwner);
			}
		}

		readonly PdfPageStreamWriterContext pageContext;

		readonly PdfDictionary xObjectResources;

		readonly Dictionary<IndirectReference, string> xFormResourceIds;

		readonly PdfArray annotations;

		readonly PdfWriter contentWriter;

		readonly MemoryStream contentStream;

		readonly PositionStack positionStack;

		readonly DisposeValidator disposeValidator;

		int currentResourceId;
	}
}
