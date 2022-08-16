using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers
{
	class WorkbookElementWriter : DirectElementBase<List<Relationship>>
	{
		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetMLNamespace;
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		public override string ElementName
		{
			get
			{
				return "workbook";
			}
		}

		protected override void InitializeAttributesOverride(List<Relationship> value)
		{
		}

		protected override void WriteChildElementsOverride(List<Relationship> value)
		{
			SheetsElement sheetsElement = base.CreateChildElement<SheetsElement>();
			sheetsElement.Write(value);
		}

		protected override void CopyAttributesOverride(ref List<Relationship> value)
		{
		}

		protected override void ReadChildElementOverride(ElementBase element, ref List<Relationship> value)
		{
			SheetsElement sheetsElement = element as SheetsElement;
			sheetsElement.Read(ref value);
		}
	}
}
