using System;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet
{
	class CellValueElement : DirectElementBase<string>
	{
		public override string ElementName
		{
			get
			{
				return "v";
			}
		}

		protected override void InitializeAttributesOverride(string value)
		{
			base.InnerText = value;
		}

		protected override void WriteChildElementsOverride(string value)
		{
		}

		protected override void CopyAttributesOverride(ref string value)
		{
			value = base.InnerText;
		}

		protected override void ReadChildElementOverride(ElementBase element, ref string value)
		{
		}
	}
}
