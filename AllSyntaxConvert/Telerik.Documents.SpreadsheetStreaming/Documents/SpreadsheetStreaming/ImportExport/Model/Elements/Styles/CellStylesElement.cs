using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class CellStylesElement : DirectElementBase<List<CellStyleInfo>>
	{
		public CellStylesElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "cellStyles";
			}
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<CellStyleInfo> value)
		{
			this.Count = value.Count;
		}

		protected override void WriteChildElementsOverride(List<CellStyleInfo> value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				CellStyleInfo value2 = value[i];
				CellStyleElement cellStyleElement = base.CreateChildElement<CellStyleElement>();
				cellStyleElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref List<CellStyleInfo> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<CellStyleInfo> value)
		{
			CellStyleElement cellStyleElement = element as CellStyleElement;
			CellStyleInfo item = new CellStyleInfo();
			cellStyleElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
