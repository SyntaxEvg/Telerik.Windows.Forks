using System;
using System.IO;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Import.Parser;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Fixed.Model.InteractiveForms;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Forms
{
	class VariableTextObjectPropertiesImporter<T> where T : IVariableTextPropertiesObject
	{
		protected VariableTextObjectPropertiesImporter(T node, PostScriptReader reader, IRadFixedDocumentImportContext context)
		{
			this.node = node;
			this.reader = reader;
			this.context = context;
		}

		protected T Node
		{
			get
			{
				return this.node;
			}
		}

		protected PostScriptReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		protected IRadFixedDocumentImportContext Context
		{
			get
			{
				return this.context;
			}
		}

		protected VariableTextProperties ReadTextProperties()
		{
			VariableTextProperties variableTextProperties = new VariableTextProperties();
			AcroFormObject acroForm = this.context.Root.GetValue().AcroForm;
			T t = this.node;
			PdfString defaultAppearance = t.GetDefaultAppearance(acroForm);
			if (defaultAppearance != null && defaultAppearance.Value.Length > 0)
			{
				byte[] value = defaultAppearance.Value;
				using (Stream stream = new MemoryStream(value))
				{
					IPdfContentImportContext acroFormContentImportContext = this.Context.GetAcroFormContentImportContext();
					ContentStreamInterpreter contentStreamInterpreter = new ContentStreamInterpreter(stream, acroFormContentImportContext);
					contentStreamInterpreter.Execute();
					contentStreamInterpreter.ApplyTextProperties(variableTextProperties.PropertiesOwner);
				}
			}
			T t2 = this.node;
			PdfInt quadding = t2.GetQuadding(acroForm);
			switch (quadding.Value)
			{
			case 1:
				variableTextProperties.HorizontalAlignment = HorizontalAlignment.Center;
				break;
			case 2:
				variableTextProperties.HorizontalAlignment = HorizontalAlignment.Right;
				break;
			default:
				variableTextProperties.HorizontalAlignment = HorizontalAlignment.Left;
				break;
			}
			return variableTextProperties;
		}

		readonly T node;

		readonly PostScriptReader reader;

		readonly IRadFixedDocumentImportContext context;
	}
}
