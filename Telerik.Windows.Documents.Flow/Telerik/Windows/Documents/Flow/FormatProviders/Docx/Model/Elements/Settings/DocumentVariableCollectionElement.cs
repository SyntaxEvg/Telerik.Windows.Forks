using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Settings
{
	class DocumentVariableCollectionElement : DocxElementBase
	{
		public DocumentVariableCollectionElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "docVars";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return context.Document.DocumentVariables.Count > 0;
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (KeyValuePair<string, string> docVariable in context.Document.DocumentVariables)
			{
				KeyValuePair<string, string> keyValuePair = docVariable;
				string key = keyValuePair.Key;
				KeyValuePair<string, string> keyValuePair2 = docVariable;
				yield return this.CreateDocumentVariable(key, keyValuePair2.Value);
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			DocumentVariableElement documentVariableElement = (DocumentVariableElement)childElement;
			context.Document.DocumentVariables.Add(documentVariableElement.Name, documentVariableElement.Value);
		}

		DocumentVariableElement CreateDocumentVariable(string name, string value)
		{
			DocumentVariableElement documentVariableElement = base.CreateElement<DocumentVariableElement>("docVar");
			documentVariableElement.Name = name;
			documentVariableElement.Value = value;
			return documentVariableElement;
		}
	}
}
