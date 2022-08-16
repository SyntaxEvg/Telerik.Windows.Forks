using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Contexts;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;
using Telerik.Windows.Documents.Model.Drawing.Charts;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import
{
	class RadFlowDocumentImportContext : IDocxImportContext, IOpenXmlImportContext
	{
		public RadFlowDocumentImportContext()
		{
			this.relationshipIdToHeader = new Dictionary<string, Header>();
			this.relationshipIdToFooter = new Dictionary<string, Footer>();
			this.relationshipIdToResource = new Dictionary<string, IResource>();
		}

		public RadFlowDocument Document
		{
			get
			{
				return this.document;
			}
		}

		public FieldContext FieldContext
		{
			get
			{
				return this.fieldContext;
			}
		}

		public CommentContext CommentContext
		{
			get
			{
				return this.commentContext;
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

		public ListsImportContext ListsImportContext
		{
			get
			{
				return this.listsImportContext;
			}
		}

		public ResourceManager Resources
		{
			get
			{
				return this.Document.Resources;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.Document.Theme;
			}
			set
			{
				this.Document.Theme = value;
			}
		}

		public bool IsImportSuspended
		{
			get
			{
				return this.documentElementsImportSuspendCounter != 0;
			}
		}

		public void BeginImport()
		{
			this.document = new RadFlowDocument(false);
			this.fieldContext = new FieldContext(true);
			this.bookmarkContext = new BookmarkContext(this.document);
			this.permissionContext = new PermissionContext(this.document);
			this.commentContext = new CommentContext(this.document);
			this.listsImportContext = new ListsImportContext();
		}

		public void EndImport()
		{
		}

		public void RegisterHeader(Header header, string relationshipId)
		{
			this.relationshipIdToHeader.Add(relationshipId, header);
		}

		public void RegisterFooter(Footer footer, string relationshipId)
		{
			this.relationshipIdToFooter.Add(relationshipId, footer);
		}

		public Section GetCurrentSection()
		{
			if (this.currentSection == null)
			{
				this.currentSection = new Section(this.document);
			}
			return this.currentSection;
		}

		public void CloseCurrentSection()
		{
			this.Document.Sections.Add(this.currentSection);
			this.currentSection = null;
		}

		public Header GetHeaderByRelationshipId(string relationshipId)
		{
			return this.relationshipIdToHeader[relationshipId];
		}

		public Footer GetFooterByRelationshipId(string relationshipId)
		{
			return this.relationshipIdToFooter[relationshipId];
		}

		public IResource GetResourceByRelationshipId(string relationshipId)
		{
			return this.relationshipIdToResource[relationshipId];
		}

		public void RegisterResource(string relationshipId, IResource resource)
		{
			this.relationshipIdToResource[relationshipId] = resource;
		}

		public IResource GetResourceByResourceKey(string relationshipId)
		{
			return this.relationshipIdToResource[relationshipId];
		}

		public void SuspendImport()
		{
			this.documentElementsImportSuspendCounter++;
		}

		public void ResumeImport()
		{
			this.documentElementsImportSuspendCounter--;
		}

		public ChartShape GetChartForChartPart(ChartPart chartPart)
		{
			throw new NotImplementedException();
		}

		public void RegisterChartPartForChart(ChartShape chart, ChartPart chartPart)
		{
			throw new NotImplementedException();
		}

		public FormulaChartData GetFormulaChartData(string formula)
		{
			throw new NotImplementedException();
		}

		public void RegisterSeriesGroupAwaitingAxisGroupName(ISupportAxes seriesGroup, int catAxisId, int valAxisId)
		{
			throw new NotImplementedException();
		}

		public void RegisterAxisGroup(AxisGroupName axisGroupName, int thisId, int otherId)
		{
			throw new NotImplementedException();
		}

		public void PairSeriesGroupsWithAxes()
		{
			throw new NotImplementedException();
		}

		readonly Dictionary<string, Header> relationshipIdToHeader;

		readonly Dictionary<string, Footer> relationshipIdToFooter;

		readonly Dictionary<string, IResource> relationshipIdToResource;

		FieldContext fieldContext;

		BookmarkContext bookmarkContext;

		PermissionContext permissionContext;

		CommentContext commentContext;

		ListsImportContext listsImportContext;

		Section currentSection;

		int documentElementsImportSuspendCounter;

		RadFlowDocument document;
	}
}
