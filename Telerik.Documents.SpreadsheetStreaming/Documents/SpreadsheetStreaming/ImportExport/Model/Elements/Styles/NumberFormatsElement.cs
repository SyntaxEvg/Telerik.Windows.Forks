using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class NumberFormatsElement : DirectElementBase<List<NumberFormat>>
	{
		public NumberFormatsElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "numFmts";
			}
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<NumberFormat> value)
		{
			this.Count = value.Count;
		}

		protected override void CopyAttributesOverride(ref List<NumberFormat> value)
		{
		}

		protected override void WriteChildElementsOverride(List<NumberFormat> value)
		{
			foreach (NumberFormat value2 in value)
			{
				NumberFormatElement numberFormatElement = base.CreateChildElement<NumberFormatElement>();
				numberFormatElement.Write(value2);
			}
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<NumberFormat> value)
		{
			NumberFormatElement numberFormatElement = element as NumberFormatElement;
			NumberFormat item = new NumberFormat();
			numberFormatElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
