using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	class PdfFormFieldSource
	{
		internal PdfFormFieldSource(PdfFileSource pdfFileSource, IndirectObject sourceObject, string fieldName)
		{
			this.fileSource = pdfFileSource;
			this.sourceObject = sourceObject;
			this.fieldName = fieldName;
		}

		internal string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}

		internal PdfFileSource FileSource
		{
			get
			{
				return this.fileSource;
			}
		}

		internal IndirectObject IndirectObject
		{
			get
			{
				return this.sourceObject;
			}
		}

		internal PdfDictionary PdfDictionary
		{
			get
			{
				return this.IndirectObject.GetContent<PdfDictionary>();
			}
		}

		internal IndirectObject[] WidgetObjects
		{
			get
			{
				if (this.widgetObjects == null)
				{
					this.widgetObjects = this.ReadWidgetObjects().ToArray<IndirectObject>();
				}
				return this.widgetObjects;
			}
		}

		internal bool HasMergedWidgetDictionary
		{
			get
			{
				return FieldsConverter.ContainsWidgetProperties(this.FileSource.Context.Reader, this.FileSource.Context, this.PdfDictionary);
			}
		}

		public override string ToString()
		{
			return this.FieldName;
		}

		IEnumerable<IndirectObject> ReadWidgetObjects()
		{
			if (this.HasMergedWidgetDictionary)
			{
				yield return this.IndirectObject;
			}
			else if (this.PdfDictionary.ContainsKey("Kids"))
			{
				PdfArray pdfArray = this.PdfDictionary["Kids"] as PdfArray;
				if (pdfArray != null)
				{
					foreach (PdfPrimitive item in pdfArray)
					{
						IndirectReference kidReference = (IndirectReference)item;
						IndirectObject widgetObject = this.FileSource.Context.ReadIndirectObject(kidReference);
						if (widgetObject.Content.Type == PdfElementType.Dictionary)
						{
							yield return widgetObject;
						}
					}
				}
			}
			yield break;
		}

		readonly string fieldName;

		readonly PdfFileSource fileSource;

		readonly IndirectObject sourceObject;

		IndirectObject[] widgetObjects;
	}
}
