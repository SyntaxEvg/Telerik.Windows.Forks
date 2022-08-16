using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class DocumentElement : DocxPartRootElementBase
	{
		public DocumentElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
			this.bodyChildElement = base.RegisterChildElement<BodyElement>("body");
		}

		public override string ElementName
		{
			get
			{
				return "document";
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			base.CreateElement(this.bodyChildElement);
		}

		readonly OpenXmlChildElement<BodyElement> bodyChildElement;
	}
}
