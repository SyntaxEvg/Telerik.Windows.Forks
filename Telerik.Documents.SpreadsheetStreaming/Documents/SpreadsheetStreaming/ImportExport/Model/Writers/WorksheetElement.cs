using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers
{
	class WorksheetElement : ConsecutiveElementBase
	{
		public WorksheetElement()
		{
			base.RegisterChildElement<SheetViewsElement>();
			base.RegisterChildElement<ColumnsElement>();
			base.RegisterChildElement<SheetDataElement>();
			base.RegisterChildElement<MergedCellsElementWriter>(typeof(SheetDataElement));
		}

		public override string ElementName
		{
			get
			{
				return "worksheet";
			}
		}

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

		public SheetViewsElement GetSheetViewsElementWriter()
		{
			return base.GetRegisteredChildElement<SheetViewsElement>();
		}

		public ColumnsElement GetColumnsElementWriter()
		{
			return base.GetRegisteredChildElement<ColumnsElement>();
		}

		public ColumnsElement GetColumnsElementReader()
		{
			if (base.ReadToElement<ColumnsElement>())
			{
				return base.GetRegisteredChildElement<ColumnsElement>();
			}
			return null;
		}

		public SheetDataElement GetSheetDataElementWriter()
		{
			return base.GetRegisteredChildElement<SheetDataElement>();
		}

		public SheetDataElement GetSheetDataElementReader()
		{
			if (base.ReadToElement<SheetDataElement>())
			{
				return base.GetRegisteredChildElement<SheetDataElement>();
			}
			return null;
		}

		public MergedCellsElementWriter GetMergedCellsElementWriter()
		{
			return base.GetRegisteredChildElement<MergedCellsElementWriter>();
		}

		public MergedCellsElementWriter GetMergedCellsElementReader()
		{
			if (base.ReadToElement<MergedCellsElementWriter>())
			{
				return base.GetRegisteredChildElement<MergedCellsElementWriter>();
			}
			return null;
		}
	}
}
