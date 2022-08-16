using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Annotations;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Converters
{
	class FieldsConverter : Converter
	{
		protected override PdfPrimitive ConvertFromArray(Type type, PostScriptReader reader, IPdfImportContext context, PdfArray array)
		{
			FieldsConverter.FieldsConvertionContext fieldsConvertionContext = new FieldsConverter.FieldsConvertionContext(reader, context, new FormFieldsTree(), array, null);
			bool isRootLevel = true;
			this.LoadFormFields(fieldsConvertionContext, isRootLevel);
			return fieldsConvertionContext.FieldsTree;
		}

		internal static bool ContainsWidgetProperties(PostScriptReader reader, IPdfImportContext context, PdfDictionary dictionary)
		{
			PdfName pdfName;
			return dictionary.TryGetElement<PdfName>(reader, context, "Subtype", out pdfName) && pdfName.Value == "Widget";
		}

		internal static bool ContainsMergedFieldProperties(PdfDictionary dictionary)
		{
			return dictionary.ContainsKey("FT") || dictionary.ContainsKey("T");
		}

		void LoadFormFields(FieldsConverter.FieldsConvertionContext context, bool isRootLevel = false)
		{
			bool flag = false;
			for (int i = 0; i < context.Kids.Count; i++)
			{
				PdfPrimitive pdfPrimitive = context.Kids[i];
				if (pdfPrimitive.Type == PdfElementType.IndirectReference)
				{
					IndirectReference indirectReference = (IndirectReference)pdfPrimitive;
					IndirectObject indirectObject = context.ImportContext.ReadIndirectObject(indirectReference);
					if (indirectObject.Content.Type == PdfElementType.Dictionary)
					{
						PdfDictionary pdfDictionary = (PdfDictionary)indirectObject.Content;
						if (!isRootLevel || !pdfDictionary.ContainsKey("Parent"))
						{
							bool flag2 = this.LoadFormField(context, indirectReference, pdfDictionary);
							flag = flag || flag2;
						}
					}
				}
			}
			if (flag)
			{
				context.FieldsTree.Add(context.Parent);
			}
		}

		bool LoadFormField(FieldsConverter.FieldsConvertionContext context, IndirectReference fieldReference, PdfDictionary fieldDictionary)
		{
			bool result = false;
			if (FieldsConverter.ContainsWidgetProperties(context.Reader, context.ImportContext, fieldDictionary))
			{
				WidgetObject widgetObject = new WidgetObject();
				widgetObject.Load(context.Reader, context.ImportContext, fieldDictionary);
				context.ImportContext.RegisterIndirectObject(fieldReference, widgetObject);
				if (FieldsConverter.ContainsMergedFieldProperties(fieldDictionary))
				{
					FormFieldNode formFieldNode = new FormFieldNode();
					formFieldNode.Load(context.Reader, context.ImportContext, fieldDictionary);
					context.FieldsTree.Add(formFieldNode);
					context.ImportContext.AddWidgetParent(widgetObject, formFieldNode);
				}
				else
				{
					context.ImportContext.AddWidgetParent(widgetObject, context.Parent);
					result = true;
				}
			}
			else
			{
				FormFieldNode formFieldNode2 = new FormFieldNode();
				formFieldNode2.Load(context.Reader, context.ImportContext, fieldDictionary);
				context.ImportContext.RegisterIndirectObject(fieldReference, formFieldNode2);
				if (formFieldNode2.Kids == null || formFieldNode2.Kids.Count == 0)
				{
					context.FieldsTree.Add(formFieldNode2);
				}
				else
				{
					this.LoadFormFields(new FieldsConverter.FieldsConvertionContext(context.Reader, context.ImportContext, context.FieldsTree, formFieldNode2.Kids, formFieldNode2), false);
				}
			}
			return result;
		}

		class FieldsConvertionContext
		{
			public FieldsConvertionContext(PostScriptReader reader, IPdfImportContext importContext, FormFieldsTree fields, PdfArray currentLevelKids, FormFieldNode currentLevelParent)
			{
				this.fields = fields;
				this.currentLevelKids = currentLevelKids;
				this.currentLevelParent = currentLevelParent;
				this.reader = reader;
				this.importContext = importContext;
			}

			public PdfArray Kids
			{
				get
				{
					return this.currentLevelKids;
				}
			}

			public FormFieldNode Parent
			{
				get
				{
					return this.currentLevelParent;
				}
			}

			public FormFieldsTree FieldsTree
			{
				get
				{
					return this.fields;
				}
			}

			public PostScriptReader Reader
			{
				get
				{
					return this.reader;
				}
			}

			public IPdfImportContext ImportContext
			{
				get
				{
					return this.importContext;
				}
			}

			readonly FormFieldsTree fields;

			readonly PdfArray currentLevelKids;

			readonly FormFieldNode currentLevelParent;

			readonly PostScriptReader reader;

			readonly IPdfImportContext importContext;
		}
	}
}
