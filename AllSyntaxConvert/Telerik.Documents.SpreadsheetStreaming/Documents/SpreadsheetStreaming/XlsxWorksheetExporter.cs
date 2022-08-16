using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Parts;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Writers.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Utilities;
using Telerik.Windows.Zip;

namespace Telerik.Documents.SpreadsheetStreaming
{
	sealed class XlsxWorksheetExporter : EntityBase, IWorksheetExporter, IDisposable
	{
		internal XlsxWorksheetExporter(XlsxWorkbookExporterBase workbook, int id, StylesRepository stylesRepository, ContentTypesRepository contentTypes, ZipArchive archive)
		{
			this.workbook = workbook;
			this.stylesRepository = stylesRepository;
			this.part = new WorksheetPartWriter(new PartContext(archive, id.ToString()));
			contentTypes.Register(this.part);
			this.mergedCellsRanges = new List<SpreadCellRange>();
			this.childManager = new ExporterChildManager();
			this.childManager.RegisterChild<XlsxWorksheetViewExporter>(new Func<EntityBase>(this.CreateViewExporter));
			this.childManager.RegisterChild<XlsxColumnExporter>(new Func<EntityBase>(this.CreateColumnExproter));
			this.childManager.RegisterChild<XlsxRowExporter>(new Func<EntityBase>(this.CreateRowExproter));
			this.childManager.RegisterChild<XlsxMergedCellsExporter>(new Func<EntityBase>(this.CreateMergedCellsExproter));
		}

		internal int RowIndex { get; set; }

		internal int ColumnIndex { get; set; }

		public IWorksheetViewExporter CreateWorksheetViewExporter()
		{
			if (this.hasSheetView)
			{
				throw new InvalidOperationException("Only one sheet view can be exported.");
			}
			XlsxWorksheetViewExporter registeredChild = this.childManager.GetRegisteredChild<XlsxWorksheetViewExporter>();
			this.hasSheetView = true;
			return registeredChild;
		}

		public IColumnExporter CreateColumnExporter()
		{
			this.childManager.EnsureCanGetElement<XlsxColumnExporter>();
			if (this.columnRangesQueue == null)
			{
				this.columnRangesQueue = new Queue<ColumnRange>();
			}
			XlsxColumnExporter registeredChild = this.childManager.GetRegisteredChild<XlsxColumnExporter>();
			this.ColumnIndex++;
			return registeredChild;
		}

		public void SkipColumns(int count)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, count, "count");
			this.childManager.EnsureCanGetElement<XlsxColumnExporter>();
			this.ColumnIndex += count;
		}

		public IRowExporter CreateRowExporter()
		{
			this.childManager.EnsureCanGetElement<XlsxRowExporter>();
			this.EnsureColumsAreExported();
			XlsxRowExporter registeredChild = this.childManager.GetRegisteredChild<XlsxRowExporter>();
			this.RowIndex++;
			return registeredChild;
		}

		public void SkipRows(int count)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, count, "count");
			this.childManager.EnsureCanGetElement<XlsxRowExporter>();
			this.RowIndex += count;
		}

		public void MergeCells(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			using (XlsxMergedCellsExporter registeredChild = this.childManager.GetRegisteredChild<XlsxMergedCellsExporter>())
			{
				registeredChild.MergeCells(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
			}
		}

		internal void NotifyColumnDisposing()
		{
			if (this.columnRangesQueue.Count > 1)
			{
				this.WriteColumn(this.columnRangesQueue.Dequeue());
			}
		}

		internal override void CompleteWriteOverride()
		{
			if (this.RowIndex == 0)
			{
				throw new InvalidOperationException("The worksheet must have at least one row.");
			}
			base.CompleteWriteOverride();
		}

		internal sealed override void DisposeOverride()
		{
			this.EnsureColumsAreExported();
			this.workbook.NotifyWorksheetDisposing();
			this.workbook = null;
			this.part.Dispose();
		}

		XlsxWorksheetViewExporter CreateViewExporter()
		{
			SheetViewsElement sheetViewsElementWriter = this.part.RootElement.GetSheetViewsElementWriter();
			return new XlsxWorksheetViewExporter(sheetViewsElementWriter);
		}

		XlsxColumnExporter CreateColumnExproter()
		{
			return new XlsxColumnExporter(this.columnRangesQueue, this.ColumnIndex, new Action(this.NotifyColumnDisposing));
		}

		XlsxRowExporter CreateRowExproter()
		{
			SheetDataElement sheetDataElementWriter = this.part.RootElement.GetSheetDataElementWriter();
			sheetDataElementWriter.EnsureWritingStarted();
			return new XlsxRowExporter(sheetDataElementWriter, this.stylesRepository, this.RowIndex);
		}

		XlsxMergedCellsExporter CreateMergedCellsExproter()
		{
			MergedCellsElementWriter mergedCellsElementWriter = this.part.RootElement.GetMergedCellsElementWriter();
			mergedCellsElementWriter.EnsureWritingStarted();
			return new XlsxMergedCellsExporter(mergedCellsElementWriter, this.mergedCellsRanges);
		}

		void EnsureColumsAreExported()
		{
			if (this.columnRangesQueue != null)
			{
				while (this.columnRangesQueue.Count > 0)
				{
					ColumnRange columnRange = this.columnRangesQueue.Dequeue();
					this.WriteColumn(columnRange);
				}
			}
		}

		void WriteColumn(ColumnRange columnRange)
		{
			ColumnsElement columnsElementWriter = this.part.RootElement.GetColumnsElementWriter();
			columnsElementWriter.EnsureWritingStarted();
			ColumnElement columnElement = columnsElementWriter.CreateColumnElementWriter();
			columnElement.Write(columnRange);
		}

		readonly ExporterChildManager childManager;

		readonly StylesRepository stylesRepository;

		readonly WorksheetPartWriter part;

		readonly List<SpreadCellRange> mergedCellsRanges;

		XlsxWorkbookExporterBase workbook;

		Queue<ColumnRange> columnRangesQueue;

		bool hasSheetView;
	}
}
