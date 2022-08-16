using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Annot;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters
{
	class FieldsConverterOld : IndirectReferenceConverterBase
	{
		protected override object ConvertFromPdfArray(Type type, PdfContentManager contentManager, PdfArrayOld array)
		{
			Guard.ThrowExceptionIfNull<Type>(type, "type");
			Guard.ThrowExceptionIfNull<PdfContentManager>(contentManager, "contentManager");
			Guard.ThrowExceptionIfNull<PdfArrayOld>(array, "array");
			FieldsConverterOld.FieldsConvertionContext fieldsConvertionContext = new FieldsConverterOld.FieldsConvertionContext(new FormFieldsCollectionOld(contentManager), array, null);
			this.LoadFormFields(fieldsConvertionContext);
			return fieldsConvertionContext.FieldsTree;
		}

		void LoadFormFields(FieldsConverterOld.FieldsConvertionContext context)
		{
			bool flag = false;
			foreach (object obj in context.Kids)
			{
				IndirectReferenceOld indirectReferenceOld = obj as IndirectReferenceOld;
				IndirectObjectOld indirectObjectOld;
				if (indirectReferenceOld != null && context.ContentManager.TryGetIndirectObject(indirectReferenceOld, out indirectObjectOld))
				{
					PdfDictionaryOld pdfDictionaryOld = indirectObjectOld.Value as PdfDictionaryOld;
					if (pdfDictionaryOld != null)
					{
						if (FieldsConverterOld.ContainsWidgetProperties(pdfDictionaryOld))
						{
							WidgetOld widgetOld = new WidgetOld(context.ContentManager);
							widgetOld.Load(indirectObjectOld);
							context.ContentManager.RegisterIndirectReference(indirectReferenceOld, widgetOld);
							if (FieldsConverterOld.ContainsMergedFieldProperties(pdfDictionaryOld))
							{
								FormFieldNodeOld formFieldNodeOld = new FormFieldNodeOld(context.ContentManager);
								formFieldNodeOld.Load(indirectObjectOld);
								context.FieldsTree.Add(formFieldNodeOld);
								context.ContentManager.AddWidgetParent(widgetOld, formFieldNodeOld);
							}
							else
							{
								context.ContentManager.AddWidgetParent(widgetOld, context.Parent);
								flag = true;
							}
						}
						else
						{
							FormFieldNodeOld formFieldNodeOld2 = new FormFieldNodeOld(context.ContentManager);
							formFieldNodeOld2.Load(indirectObjectOld);
							context.ContentManager.RegisterIndirectReference(indirectReferenceOld, formFieldNodeOld2);
							if (formFieldNodeOld2.Kids == null || formFieldNodeOld2.Kids.Count == 0)
							{
								context.FieldsTree.Add(formFieldNodeOld2);
							}
							else
							{
								this.LoadFormFields(new FieldsConverterOld.FieldsConvertionContext(context.FieldsTree, formFieldNodeOld2.Kids, formFieldNodeOld2));
							}
						}
					}
				}
			}
			if (flag)
			{
				context.FieldsTree.Add(context.Parent);
			}
		}

		static bool ContainsMergedFieldProperties(PdfDictionaryOld dictionary)
		{
			return dictionary.ContainsKey("FT") || dictionary.ContainsKey("T");
		}

		static bool ContainsWidgetProperties(PdfDictionaryOld dictionary)
		{
			if (dictionary.ContainsKey("Subtype"))
			{
				PdfNameOld element = dictionary.GetElement<PdfNameOld>("Subtype");
				return element.Value == "Widget";
			}
			return false;
		}

		class FieldsConvertionContext
		{
			public FieldsConvertionContext(FormFieldsCollectionOld fields, PdfArrayOld currentLevelKids, FormFieldNodeOld currentLevelParent)
			{
				this.fields = fields;
				this.currentLevelKids = currentLevelKids;
				this.currentLevelParent = currentLevelParent;
			}

			public PdfArrayOld Kids
			{
				get
				{
					return this.currentLevelKids;
				}
			}

			public FormFieldNodeOld Parent
			{
				get
				{
					return this.currentLevelParent;
				}
			}

			public FormFieldsCollectionOld FieldsTree
			{
				get
				{
					return this.fields;
				}
			}

			public PdfContentManager ContentManager
			{
				get
				{
					return this.fields.ContentManager;
				}
			}

			readonly FormFieldsCollectionOld fields;

			readonly PdfArrayOld currentLevelKids;

			readonly FormFieldNodeOld currentLevelParent;
		}
	}
}
