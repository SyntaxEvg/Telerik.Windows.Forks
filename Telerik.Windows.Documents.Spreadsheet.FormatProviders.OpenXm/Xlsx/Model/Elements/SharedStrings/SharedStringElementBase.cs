using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings
{
	abstract class SharedStringElementBase : XlsxElementBase
	{
		public SharedStringElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
