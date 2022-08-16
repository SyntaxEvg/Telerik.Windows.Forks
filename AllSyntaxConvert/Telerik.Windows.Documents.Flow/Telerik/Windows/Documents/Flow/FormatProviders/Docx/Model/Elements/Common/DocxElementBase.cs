using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common
{
	abstract class DocxElementBase : OpenXmlElementBase<IDocxImportContext, IDocxExportContext, DocxPartsManager>
	{
		public DocxElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.WordprocessingMLNamespace;
			}
		}
	}
}
