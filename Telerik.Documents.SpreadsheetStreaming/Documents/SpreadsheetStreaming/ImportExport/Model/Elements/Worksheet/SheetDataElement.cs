using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class SheetDataElement : ConsecutiveElementBase
	{
		public SheetDataElement()
		{
			base.RegisterChildElement<RowElement>();
		}

		public override string ElementName
		{
			get
			{
				return "sheetData";
			}
		}

		public IEnumerable<RowElement> Rows
		{
			get
			{
				while (base.ReadToElement<RowElement>())
				{
					RowElement rowElement = base.CreateChildElement<RowElement>();
					rowElement.BeginReadElement();
					yield return rowElement;
					rowElement.EndReadElement();
				}
				yield break;
			}
		}

		public RowElement CreateRowElementWriter()
		{
			return base.CreateChildElement<RowElement>();
		}
	}
}
