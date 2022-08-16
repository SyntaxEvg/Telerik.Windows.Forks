using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Export;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export
{
	class RadFlowDocumentExportContext : IDocxExportContext, IOpenXmlExportContext
	{
		public RadFlowDocumentExportContext(RadFlowDocument document, DocxExportSettings settings)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			Guard.ThrowExceptionIfNull<DocxExportSettings>(settings, "settings");
			this.document = document;
			this.ExportSettings = settings;
			this.headerFooterToPartNumberDictionary = new Dictionary<HeaderFooterBase, int>();
			this.headerFooterToRelationshipIdDictionary = new Dictionary<HeaderFooterBase, string>();
			this.resourceToRelationshipIdDictionary = new Dictionary<IResource, string>();
			this.fieldContext = new FieldContext(false);
			this.bookmarkContext = new BookmarkContext(this.document);
			this.permissionContext = new PermissionContext(this.document);
			this.commentContext = new CommentContext(this.document);
			this.listsExportContext = new ListsExportContext(this.document);
			this.InitializeHeaders();
			this.InitializeFooters();
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.Document.Theme;
			}
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public DocxExportSettings ExportSettings { get; set; }

		public FieldContext FieldContext
		{
			get
			{
				return this.fieldContext;
			}
		}

		public BookmarkContext BookmarkContext
		{
			get
			{
				return this.bookmarkContext;
			}
		}

		public PermissionContext PermissionContext
		{
			get
			{
				return this.permissionContext;
			}
		}

		public CommentContext CommentContext
		{
			get
			{
				return this.commentContext;
			}
		}

		public ListsExportContext ListsExportContext
		{
			get
			{
				return this.listsExportContext;
			}
		}

		public ResourceManager Resources
		{
			get
			{
				return this.document.Resources;
			}
		}

		public IEnumerable<Section> GetSections()
		{
			return this.document.Sections;
		}

		public IEnumerable<Header> GetHeaders()
		{
			foreach (Section section in this.GetSections())
			{
				if (section.Headers.Even != null)
				{
					yield return section.Headers.Even;
				}
				if (section.Headers.Default != null)
				{
					yield return section.Headers.Default;
				}
				if (section.Headers.First != null)
				{
					yield return section.Headers.First;
				}
			}
			yield break;
		}

		public IEnumerable<Footer> GetFooters()
		{
			foreach (Section section in this.GetSections())
			{
				if (section.Footers.Even != null)
				{
					yield return section.Footers.Even;
				}
				if (section.Footers.Default != null)
				{
					yield return section.Footers.Default;
				}
				if (section.Footers.First != null)
				{
					yield return section.Footers.First;
				}
			}
			yield break;
		}

		public int GetHeaderFooterPartNumberByHeaderFooter(HeaderFooterBase headerFooter)
		{
			Guard.ThrowExceptionIfNull<HeaderFooterBase>(headerFooter, "headerFooter");
			return this.headerFooterToPartNumberDictionary[headerFooter];
		}

		public string GetRelationshipIdByHeaderFooter(HeaderFooterBase headerFooter)
		{
			Guard.ThrowExceptionIfNull<HeaderFooterBase>(headerFooter, "headerFooter");
			return this.headerFooterToRelationshipIdDictionary[headerFooter];
		}

		public void RegisterHeaderFooter(string relationshipId, HeaderFooterBase headerFooter)
		{
			Guard.ThrowExceptionIfNull<string>(relationshipId, "relationshipId");
			Guard.ThrowExceptionIfNull<HeaderFooterBase>(headerFooter, "headerFooter");
			this.headerFooterToRelationshipIdDictionary.Add(headerFooter, relationshipId);
		}

		public string GetRelationshipIdByResource(IResource resource)
		{
			Guard.ThrowExceptionIfNull<IResource>(resource, "resource");
			return this.resourceToRelationshipIdDictionary[resource];
		}

		public void RegisterResource(string relationshipId, IResource resource)
		{
			Guard.ThrowExceptionIfNullOrEmpty(relationshipId, "relationshipId");
			Guard.ThrowExceptionIfNull<IResource>(resource, "resource");
			this.resourceToRelationshipIdDictionary[resource] = relationshipId;
		}

		public ChartPart GetChartPartForChart(ChartShape chart)
		{
			throw new NotImplementedException();
		}

		public ChartShape GetChartForChartPart(ChartPart chartPart)
		{
			throw new NotImplementedException();
		}

		void InitializeHeaders()
		{
			int num = 1;
			foreach (Header key in this.GetHeaders())
			{
				this.headerFooterToPartNumberDictionary.Add(key, num++);
			}
		}

		void InitializeFooters()
		{
			int num = 1;
			foreach (Footer key in this.GetFooters())
			{
				this.headerFooterToPartNumberDictionary.Add(key, num++);
			}
		}

		readonly RadFlowDocument document;

		readonly Dictionary<IResource, string> resourceToRelationshipIdDictionary;

		readonly Dictionary<HeaderFooterBase, int> headerFooterToPartNumberDictionary;

		readonly Dictionary<HeaderFooterBase, string> headerFooterToRelationshipIdDictionary;

		readonly FieldContext fieldContext;

		readonly BookmarkContext bookmarkContext;

		readonly PermissionContext permissionContext;

		readonly CommentContext commentContext;

		readonly ListsExportContext listsExportContext;
	}
}
