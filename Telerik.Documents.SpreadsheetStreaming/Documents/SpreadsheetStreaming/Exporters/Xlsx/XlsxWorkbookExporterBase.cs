using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts;
using Telerik.Documents.SpreadsheetStreaming.Licensing;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Model.Relations;
using Telerik.Documents.SpreadsheetStreaming.Model.Themes;
using Telerik.Documents.SpreadsheetStreaming.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	abstract class XlsxWorkbookExporterBase : WorkbookExporterBase
	{
		internal XlsxWorkbookExporterBase(Stream stream)
		{
			Guard.ThrowExceptionIfNull<Stream>(stream, "stream");
			this.targetArchive = new ZipArchive(stream, ZipArchiveMode.Create, true, null);
			this.workbookRelationships = new Relationships(this.IsAppender);
			this.contentTypes = new ContentTypesRepository();
			this.theme = SpreadPredefinedThemeSchemes.DefaultTheme;
			this.cellStyles = new SpreadCellStyleCollection();
			LicenseCheck.Validate(this);
		}

		public virtual bool IsAppender
		{
			get
			{
				return false;
			}
		}

		public override SpreadCellStyleCollection CellStyles
		{
			get
			{
				return this.cellStyles;
			}
		}

		protected ZipArchive TargetArchive
		{
			get
			{
				return this.targetArchive;
			}
		}

		protected Relationships WorkbookRelationships
		{
			get
			{
				return this.workbookRelationships;
			}
		}

		protected ContentTypesRepository ContentTypes
		{
			get
			{
				return this.contentTypes;
			}
		}

		protected StylesRepository StylesRepository
		{
			get
			{
				if (this.stylesRepository == null)
				{
					this.stylesRepository = this.InitializeStylesRepository();
				}
				return this.stylesRepository;
			}
		}

		protected SpreadTheme Theme
		{
			get
			{
				return this.theme;
			}
		}

		public sealed override IWorksheetExporter CreateWorksheetExporterOverride(string name)
		{
			this.ValidateSheetName(name);
			int id = this.workbookRelationships.AddWorksheet(name);
			return new XlsxWorksheetExporter(this, id, this.StylesRepository, this.contentTypes, this.targetArchive);
		}

		public override IEnumerable<SheetInfo> GetSheetInfos()
		{
			return from relationship in this.WorkbookRelationships.GetRelationships(XlsxRelationshipTypes.WorksheetRelationshipType)
				orderby relationship.Index
				select new SheetInfo(relationship.Name);
		}

		internal sealed override void DisposeOverride()
		{
			this.targetArchive.Dispose();
			base.DisposeOverride();
		}

		internal sealed override void CompleteWriteOverride()
		{
			if (!this.WorkbookRelationships.GetRelationships(XlsxRelationshipTypes.WorksheetRelationshipType).Any<Relationship>())
			{
				throw new InvalidOperationException("The workbook must have at least one worksheet.");
			}
			this.WriteWorkbookPart();
			this.WriteStylesPart();
			this.WriteWorkbookRelationshipsPart();
			ResourcesHelper.CopyResourcesToZipArchive(this.TargetArchive, this.ContentTypes);
			this.WriteContentTypesPart();
		}

		protected abstract StylesRepository InitializeStylesRepository();

		protected virtual void WriteWorkbookPart()
		{
			using (WorkbookPart workbookPart = new WorkbookPart(new PartContext(this.targetArchive)))
			{
				this.contentTypes.Register(workbookPart);
				IEnumerable<Relationship> relationships = this.workbookRelationships.GetRelationships(XlsxRelationshipTypes.WorksheetRelationshipType);
				IOrderedEnumerable<Relationship> source = from p in relationships
					orderby p.Id
					select p;
				List<Relationship> relationships2 = source.ToList<Relationship>();
				workbookPart.WriteWorksheetRelationships(relationships2);
			}
		}

		protected virtual void WriteStylesPart()
		{
			using (StylesPart stylesPart = new StylesPart(new PartContext(this.targetArchive, this.theme)))
			{
				this.contentTypes.Register(stylesPart);
				stylesPart.Write(this.StylesRepository);
			}
		}

		protected void WriteWorkbookRelationshipsPart()
		{
			using (WorkbookRelationshipsPart workbookRelationshipsPart = new WorkbookRelationshipsPart(new PartContext(this.targetArchive)))
			{
				this.contentTypes.Register(workbookRelationshipsPart);
				workbookRelationshipsPart.WriteRelationships(this.workbookRelationships);
			}
		}

		protected void WriteContentTypesPart()
		{
			using (ContentTypesPart contentTypesPart = new ContentTypesPart(new PartContext(this.targetArchive)))
			{
				contentTypesPart.Write(this.contentTypes);
			}
		}

		void ValidateSheetName(string sheetName)
		{
			if (this.workbookRelationships.ContainsWorksheetName(sheetName))
			{
				throw new ArgumentException(string.Format("Sheet with name \"{0}\" already exists.", sheetName));
			}
			if (sheetName.Length >= XlsxWorkbookExporterBase.maxSheetNameLength)
			{
				throw new ArgumentException(string.Format("Sheet name should be limited to 31 characters.", new object[0]));
			}
			foreach (char item in sheetName)
			{
				if (XlsxWorkbookExporterBase.forbiddenSheetNameSymbols.Contains(item))
				{
					string arg = string.Join<char>(", ", XlsxWorkbookExporterBase.forbiddenSheetNameSymbols);
					throw new ArgumentException(string.Format("Sheet name contains at least one of the forbidden symbols: ({0}).", arg));
				}
			}
		}

		static readonly List<char> forbiddenSheetNameSymbols = new List<char>(new char[] { '\\', '/', '?', '*', '[', ']', ':' });

		static readonly int maxSheetNameLength = 31;

		readonly Relationships workbookRelationships;

		readonly ContentTypesRepository contentTypes;

		readonly ZipArchive targetArchive;

		readonly SpreadCellStyleCollection cellStyles;

		readonly SpreadTheme theme;

		StylesRepository stylesRepository;
	}
}
