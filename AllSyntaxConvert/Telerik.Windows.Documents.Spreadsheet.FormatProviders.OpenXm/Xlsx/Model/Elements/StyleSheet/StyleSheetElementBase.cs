using System;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	abstract class StyleSheetElementBase : XlsxElementBase
	{
		public StyleSheetElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}
	}
}
