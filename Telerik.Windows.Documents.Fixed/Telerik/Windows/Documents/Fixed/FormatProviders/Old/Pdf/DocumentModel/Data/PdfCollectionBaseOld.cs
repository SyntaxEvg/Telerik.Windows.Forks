using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	abstract class PdfCollectionBaseOld<TIndex> : PdfObjectOld
	{
		public PdfCollectionBaseOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public T GetElement<T>(TIndex index, IConverter converter) where T : PdfObjectOld
		{
			object elementAtIndex = this.GetElementAtIndex(index);
			if (elementAtIndex == null)
			{
				return default(T);
			}
			T t = elementAtIndex as T;
			if (t != null)
			{
				return t;
			}
			IndirectReferenceOld indirectReferenceOld = elementAtIndex as IndirectReferenceOld;
			if (indirectReferenceOld != null && !converter.HandlesIndirectReference)
			{
				t = Converters.IndirectReferenceToPdfObjectConverter.Convert<T>(base.ContentManager, indirectReferenceOld);
				t.Load();
			}
			else
			{
				t = (T)((object)converter.Convert(typeof(T), base.ContentManager, elementAtIndex));
			}
			return t;
		}

		public T GetElement<T>(TIndex index) where T : PdfObjectOld
		{
			object elementAtIndex = this.GetElementAtIndex(index);
			if (elementAtIndex == null)
			{
				return default(T);
			}
			T t = elementAtIndex as T;
			if (t != null)
			{
				return t;
			}
			IndirectReferenceOld indirectReferenceOld = elementAtIndex as IndirectReferenceOld;
			if (indirectReferenceOld != null)
			{
				t = Converters.IndirectReferenceToPdfObjectConverter.Convert<T>(base.ContentManager, indirectReferenceOld);
				t.Load();
			}
			else
			{
				PdfDictionaryOld pdfDictionaryOld = elementAtIndex as PdfDictionaryOld;
				if (pdfDictionaryOld != null)
				{
					t = (T)((object)Converters.PdfDictionaryToPdfObjectConverter.Convert(typeof(T), base.ContentManager, pdfDictionaryOld));
				}
				else
				{
					t = elementAtIndex as T;
					if (t != null)
					{
						t.Load();
					}
				}
			}
			return t;
		}

		public bool TryGetBool(TIndex index, out bool res)
		{
			res = false;
			if (this.ContainsElementAtIndex(index))
			{
				PdfBoolOld element = this.GetElement<PdfBoolOld>(index, Converters.PdfBoolConverter);
				if (element != null)
				{
					res = element.Value;
					return true;
				}
			}
			return false;
		}

		public bool TryGetInt(TIndex index, out int res)
		{
			res = 0;
			if (this.ContainsElementAtIndex(index))
			{
				PdfIntOld element = this.GetElement<PdfIntOld>(index, Converters.PdfIntConverter);
				if (element != null)
				{
					res = element.Value;
					return true;
				}
			}
			return false;
		}

		public bool TryGetReal(TIndex index, out double res)
		{
			res = 0.0;
			if (this.ContainsElementAtIndex(index))
			{
				PdfRealOld element = this.GetElement<PdfRealOld>(index, Converters.PdfRealConverter);
				if (element != null)
				{
					res = element.Value;
					return true;
				}
			}
			return false;
		}

		protected abstract object GetElementAtIndex(TIndex index);

		protected abstract bool ContainsElementAtIndex(TIndex index);
	}
}
