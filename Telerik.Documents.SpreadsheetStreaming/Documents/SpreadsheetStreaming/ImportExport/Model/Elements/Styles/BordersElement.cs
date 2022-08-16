using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class BordersElement : DirectElementBase<List<SpreadCellBorders>>
	{
		public BordersElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "borders";
			}
		}

		public int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<SpreadCellBorders> value)
		{
			this.Count = value.Count;
		}

		protected override void WriteChildElementsOverride(List<SpreadCellBorders> value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				SpreadCellBorders value2 = value[i];
				BorderElement borderElement = base.CreateChildElement<BorderElement>();
				borderElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref List<SpreadCellBorders> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<SpreadCellBorders> value)
		{
			BorderElement borderElement = element as BorderElement;
			SpreadCellBorders item = new SpreadCellBorders();
			borderElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
