using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import
{
	class DocxImporter : OpenXmlImporter<DocxPartsManager>
	{
		protected override DocxPartsManager CreatePartsManager()
		{
			return new DocxPartsManager();
		}
	}
}
