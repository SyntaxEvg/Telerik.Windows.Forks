using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	abstract class DocxPartBase : OpenXmlPartBase
	{
		public DocxPartBase(DocxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public new DocxPartsManager PartsManager
		{
			get
			{
				return (DocxPartsManager)base.PartsManager;
			}
		}
	}
}
