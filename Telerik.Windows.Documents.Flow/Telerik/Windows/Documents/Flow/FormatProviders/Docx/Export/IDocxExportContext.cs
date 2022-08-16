using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export
{
	interface IDocxExportContext : IOpenXmlExportContext
	{
		RadFlowDocument Document { get; }

		DocxExportSettings ExportSettings { get; }

		FieldContext FieldContext { get; }

		BookmarkContext BookmarkContext { get; }

		PermissionContext PermissionContext { get; }

		CommentContext CommentContext { get; }

		ListsExportContext ListsExportContext { get; }

		IEnumerable<Section> GetSections();

		IEnumerable<Header> GetHeaders();

		IEnumerable<Footer> GetFooters();

		string GetRelationshipIdByHeaderFooter(HeaderFooterBase headerFooter);

		void RegisterHeaderFooter(string relationshipId, HeaderFooterBase headerFooter);

		int GetHeaderFooterPartNumberByHeaderFooter(HeaderFooterBase headerFooter);
	}
}
