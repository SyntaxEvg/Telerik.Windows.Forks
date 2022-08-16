using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import
{
	interface IDocxImportContext : IOpenXmlImportContext
	{
		RadFlowDocument Document { get; }

		FieldContext FieldContext { get; }

		BookmarkContext BookmarkContext { get; }

		PermissionContext PermissionContext { get; }

		ListsImportContext ListsImportContext { get; }

		CommentContext CommentContext { get; }

		Header GetHeaderByRelationshipId(string relationshipId);

		void RegisterHeader(Header header, string relationshipId);

		Footer GetFooterByRelationshipId(string relationshipId);

		void RegisterFooter(Footer footer, string relationshipId);

		Section GetCurrentSection();

		void CloseCurrentSection();

		void SuspendImport();

		void ResumeImport();
	}
}
