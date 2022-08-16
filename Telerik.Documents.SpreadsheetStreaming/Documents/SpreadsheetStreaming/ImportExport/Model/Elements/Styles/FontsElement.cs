using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Attributes;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Styles
{
	class FontsElement : DirectElementBase<List<FontProperties>>
	{
		public FontsElement()
		{
			this.count = base.RegisterCountAttribute();
		}

		public override string ElementName
		{
			get
			{
				return "fonts";
			}
		}

		int Count
		{
			set
			{
				this.count.Value = value;
			}
		}

		protected override void InitializeAttributesOverride(List<FontProperties> value)
		{
			this.Count = value.Count;
		}

		protected override void WriteChildElementsOverride(List<FontProperties> value)
		{
			for (int i = 0; i < value.Count; i++)
			{
				FontElement fontElement = base.CreateChildElement<FontElement>();
				fontElement.Write(value[i]);
			}
		}

		protected override void CopyAttributesOverride(ref List<FontProperties> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<FontProperties> value)
		{
			FontElement fontElement = element as FontElement;
			FontProperties item = new FontProperties();
			fontElement.Read(ref item);
			value.Add(item);
		}

		readonly OpenXmlCountAttribute count;
	}
}
