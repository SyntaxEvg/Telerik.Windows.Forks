using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	class ColumnRowElement : WorksheetDrawingElementBase
	{
		public ColumnRowElement(XlsxPartsManager partsManager, string elementName)
			: base(partsManager)
		{
			Guard.ThrowExceptionIfNullOrEmpty(elementName, "elementName");
			this.elementName = elementName;
		}

		public override string ElementName
		{
			get
			{
				return this.elementName;
			}
		}

		readonly string elementName;
	}
}
