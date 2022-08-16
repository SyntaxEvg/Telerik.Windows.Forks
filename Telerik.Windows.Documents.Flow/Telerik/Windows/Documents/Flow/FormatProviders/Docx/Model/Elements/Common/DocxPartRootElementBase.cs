using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common
{
	abstract class DocxPartRootElementBase : DocxElementBase
	{
		public DocxPartRootElementBase(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNull<OpenXmlPartBase>(part, "part");
			base.Part = part;
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}
	}
}
