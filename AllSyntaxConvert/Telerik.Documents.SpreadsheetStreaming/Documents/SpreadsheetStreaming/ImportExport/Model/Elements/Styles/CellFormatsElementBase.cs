using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	abstract class CellFormatsElementBase : DirectElementBase<List<DiferentialFormat>>
	{
		public CellFormatsElementBase()
		{
			this.count = base.RegisterCountAttribute();
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<DiferentialFormat> value)
		{
			this.Count = value.Count;
		}

		protected override void WriteChildElementsOverride(List<DiferentialFormat> value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				DiferentialFormat value2 = value[i];
				FormatElement formatElement = base.CreateChildElement<FormatElement>();
				formatElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref List<DiferentialFormat> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<DiferentialFormat> value)
		{
			FormatElement formatElement = element as FormatElement;
			DiferentialFormat item = new DiferentialFormat();
			formatElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
