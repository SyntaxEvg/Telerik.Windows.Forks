using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements
{
	class SheetsElement : DirectElementBase<List<Relationship>>
	{
		public override string ElementName
		{
			get
			{
				return "sheets";
			}
		}

		protected override void InitializeAttributesOverride(List<Relationship> value)
		{
		}

		protected override void WriteChildElementsOverride(List<Relationship> value)
		{
			foreach (Relationship value2 in value)
			{
				SheetElement sheetElement = base.CreateChildElement<SheetElement>();
				sheetElement.Write(value2);
			}
		}

		protected override void CopyAttributesOverride(ref List<Relationship> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<Relationship> value)
		{
			SheetElement sheetElement = element as SheetElement;
			Relationship relationship = null;
			sheetElement.Read(ref relationship);
			if (relationship != null)
			{
				relationship.Index = this.currentIndex++;
				value.Add(relationship);
			}
		}

		int currentIndex;
	}
}
