using System;
using System.Collections.Generic;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;
using Telerik.Documents.SpreadsheetStreaming.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.Importers
{
	class XlsxWorkbookImporter : IWorkbookImporter, IDisposable
	{
		internal XlsxWorkbookImporter(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.theme = SpreadPredefinedThemeSchemes.DefaultTheme;
			this.archive = new ZipArchive(stream, ZipArchiveMode.Read, true, null);
			this.contentTypes = new ContentTypesRepository();
			this.workbookRelationships = new Relationships(true);
			this.stylesRepository = new StylesRepository();
			this.ReadContentTypesPart();
			this.ReadWorkbookRelationshipsPart();
			this.ReadStylesPart();
			this.ReadWorkbookPart();
		}

		~XlsxWorkbookImporter()
		{
			this.Dispose(false);
		}

		public IEnumerable<IWorksheetImporter> WorksheetImporters
		{
			get
			{
				foreach (Relationship relationship in this.workbookRelationships.GetRelationships(XlsxRelationshipTypes.WorksheetRelationshipType))
				{
					yield return new XlsxWorksheetImporter(this.archive, relationship.Name, relationship.Target);
				}
				yield break;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.archive.Dispose();
			}
		}

		void ReadContentTypesPart()
		{
			using (ContentTypesPart contentTypesPart = new ContentTypesPart(new PartContext(this.archive)))
			{
				contentTypesPart.Read(this.contentTypes);
			}
		}

		void ReadWorkbookRelationshipsPart()
		{
			using (WorkbookRelationshipsPart workbookRelationshipsPart = new WorkbookRelationshipsPart(new PartContext(this.archive)))
			{
				workbookRelationshipsPart.ReadRelationships(this.workbookRelationships);
			}
		}

		void ReadStylesPart()
		{
			using (StylesPart stylesPart = new StylesPart(new PartContext(this.archive, this.theme)))
			{
				stylesPart.Read(this.stylesRepository);
			}
		}

		void ReadWorkbookPart()
		{
			List<Relationship> list = new List<Relationship>();
			using (WorkbookPart workbookPart = new WorkbookPart(new PartContext(this.archive)))
			{
				workbookPart.Read(list);
			}
			foreach (Relationship relationship in list)
			{
				this.workbookRelationships.AddRelationship(relationship);
			}
		}

		readonly ZipArchive archive;

		readonly ContentTypesRepository contentTypes;

		readonly Relationships workbookRelationships;

		readonly StylesRepository stylesRepository;

		readonly SpreadTheme theme;
	}
}
