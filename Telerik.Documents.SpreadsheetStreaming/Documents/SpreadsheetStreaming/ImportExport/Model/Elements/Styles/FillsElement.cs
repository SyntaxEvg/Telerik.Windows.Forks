using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FillsElement : DirectElementBase<List<ISpreadFill>>
	{
		public FillsElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "fills";
			}
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<ISpreadFill> value)
		{
			this.Count = value.Count;
		}

		protected override void WriteChildElementsOverride(List<ISpreadFill> value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				FillElement fillElement = base.CreateChildElement<FillElement>();
				fillElement.Write(value[i]);
			}
		}

		protected override void CopyAttributesOverride(ref List<ISpreadFill> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<ISpreadFill> value)
		{
			FillElement fillElement = element as FillElement;
			ISpreadFill item = null;
			fillElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
