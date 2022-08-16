using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common
{
	abstract class XlsxPartRootElementBase : XlsxElementBase
	{
		public XlsxPartRootElementBase(XlsxPartsManager partsManager, OpenXmlPartBase part)
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
