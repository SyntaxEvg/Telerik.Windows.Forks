using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming
{
	class PdfFormFieldSourceCollection : IEnumerable<PdfFormFieldSource>, IEnumerable
	{
		public PdfFormFieldSourceCollection(PdfFileSource fileSource)
		{
			this.fields = new Dictionary<string, PdfFormFieldSource>();
			foreach (PdfFormFieldSource pdfFormFieldSource in PdfFormFieldSourceCollection.EnumerateFieldSources(fileSource))
			{
				this.fields.Add(pdfFormFieldSource.FieldName, pdfFormFieldSource);
			}
		}

		public PdfFormFieldSource this[string fieldName]
		{
			get
			{
				return this.fields[fieldName];
			}
		}

		public int Count
		{
			get
			{
				return this.fields.Count;
			}
		}

		public IEnumerator<PdfFormFieldSource> GetEnumerator()
		{
			return this.fields.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.fields.Values.GetEnumerator();
		}

		static IEnumerable<PdfFormFieldSource> EnumerateFieldSources(PdfFileSource fileSource)
		{
			string[] propertyNames = new string[] { "AcroForm", "Fields" };
			PdfArray fieldsArray;
			if (fileSource.Context.TryGetNestedPrimitive<PdfArray>(fileSource.Context.Root.Reference, propertyNames, out fieldsArray))
			{
				foreach (PdfFormFieldSource fieldSource in PdfFormFieldSourceCollection.EnumerateFieldSources(fileSource, fieldsArray, null, string.Empty))
				{
					yield return fieldSource;
				}
			}
			yield break;
		}

		static IEnumerable<PdfFormFieldSource> EnumerateFieldSources(PdfFileSource fileSource, PdfArray currentLevelNodes, IndirectObject parentObject, string parentName)
		{
			bool addParentAsTerminalNode = false;
			foreach (PdfPrimitive item in currentLevelNodes)
			{
				if (item.Type == PdfElementType.IndirectReference)
				{
					IndirectReference childReference = (IndirectReference)item;
					IndirectObject childObject = fileSource.Context.ReadIndirectObject(childReference);
					if (childObject.Content.Type == PdfElementType.Dictionary)
					{
						PdfDictionary dictionary = (PdfDictionary)childObject.Content;
						if (FieldsConverter.ContainsWidgetProperties(fileSource.Context.Reader, fileSource.Context, dictionary))
						{
							if (FieldsConverter.ContainsMergedFieldProperties(dictionary))
							{
								string fieldName = PdfFormFieldSourceCollection.GetFieldName(dictionary, parentName);
								yield return new PdfFormFieldSource(fileSource, childObject, fieldName);
							}
							else
							{
								addParentAsTerminalNode = true;
							}
						}
						else
						{
							string fieldName2 = PdfFormFieldSourceCollection.GetFieldName(dictionary, parentName);
							PdfArray kids;
							if (dictionary.TryGetElement<PdfArray>(fileSource.Context.Reader, fileSource.Context, "Kids", out kids) && kids.Count > 0)
							{
								foreach (PdfFormFieldSource fieldSource in PdfFormFieldSourceCollection.EnumerateFieldSources(fileSource, kids, childObject, fieldName2))
								{
									yield return fieldSource;
								}
							}
							else
							{
								yield return new PdfFormFieldSource(fileSource, childObject, fieldName2);
							}
						}
					}
				}
			}
			if (addParentAsTerminalNode)
			{
				yield return new PdfFormFieldSource(fileSource, parentObject, parentName);
			}
			yield break;
		}

		static string GetFieldName(PdfDictionary fieldNodeDictionary, string parentName)
		{
			if (fieldNodeDictionary.ContainsKey("T"))
			{
				PdfString pdfString = fieldNodeDictionary["T"] as PdfString;
				if (pdfString != null)
				{
					string text = pdfString.ToString();
					if (!string.IsNullOrEmpty(text))
					{
						if (string.IsNullOrEmpty(parentName))
						{
							return text;
						}
						return string.Format("{0}.{1}", parentName, text);
					}
				}
			}
			return parentName;
		}

		readonly Dictionary<string, PdfFormFieldSource> fields;
	}
}
