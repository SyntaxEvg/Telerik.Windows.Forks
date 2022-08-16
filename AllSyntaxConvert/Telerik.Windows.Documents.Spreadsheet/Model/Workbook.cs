using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telerik.Windows.Documents.Common.Model.Data;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.History;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model.Protection;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Workbook : NotifyPropertyChangedBase, IDisposable
	{
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					this.name = value;
					this.UpdateWorkbookNameDependentValuesAndNames();
					this.OnNameChanged();
					base.OnPropertyChanged("Name");
				}
			}
		}

		public int ActiveTabIndex
		{
			get
			{
				return this.activeTabIndex;
			}
			set
			{
				this.activeTabIndex = value;
			}
		}

		internal NameManager NameManager
		{
			get
			{
				return this.nameManager;
			}
		}

		public NameCollection Names
		{
			get
			{
				return this.nameManager.WorkbookNameCollection;
			}
		}

		public SheetCollection Sheets
		{
			get
			{
				return this.sheets;
			}
		}

		public WorksheetCollection Worksheets
		{
			get
			{
				return this.worksheets;
			}
		}

		public Sheet ActiveSheet
		{
			get
			{
				return this.Sheets.ActiveSheet;
			}
			set
			{
				this.Sheets.ActiveSheet = value;
			}
		}

		public Worksheet ActiveWorksheet
		{
			get
			{
				return this.ActiveSheet as Worksheet;
			}
			set
			{
				this.ActiveSheet = value;
			}
		}

		public CellStyleCollection Styles
		{
			get
			{
				return this.styles;
			}
		}

		public WorkbookHistory History
		{
			get
			{
				return this.history;
			}
		}

		public DocumentTheme Theme
		{
			get
			{
				return this.theme;
			}
			set
			{
				if (!TelerikHelper.EqualsOfT<DocumentTheme>(this.theme, value))
				{
					this.ExecuteCommand<SetWorkbookPropertyCommandContext<DocumentTheme>>(WorkbookCommands.SetWorkbookTheme, new SetWorkbookPropertyCommandContext<DocumentTheme>(this, value));
				}
			}
		}

		public bool IsLayoutUpdateSuspended
		{
			get
			{
				return this.suspendLayoutUpdateCount.BeginUpdateCounter > 0;
			}
		}

		internal bool IsPropertyChangeSuspended
		{
			get
			{
				return this.suspendPropertyChangedCount.IsUpdateInProgress;
			}
		}

		public TimeSpan WorkbookContentChangedInterval
		{
			get
			{
				return this.workbookContentChangedInterval;
			}
			set
			{
				this.workbookContentChangedInterval = value;
				if (this.contentChangedExecutor != null)
				{
					this.contentChangedExecutor.WaitInterval = this.workbookContentChangedInterval;
				}
			}
		}

		internal ProtectionData ProtectionData
		{
			get
			{
				return this.protectionData;
			}
		}

		public bool IsProtected
		{
			get
			{
				return this.ProtectionData.Enforced;
			}
		}

		internal WorkbookProtectionOptions ProtectionOptions
		{
			get
			{
				return this.protectionOptions;
			}
			set
			{
				this.protectionOptions = value;
			}
		}

		internal bool ContainsProtectedSheets
		{
			get
			{
				return this.containsProtectedSheets;
			}
			set
			{
				if (this.containsProtectedSheets != value)
				{
					this.containsProtectedSheets = value;
					base.OnPropertyChanged("ContainsProtectedSheets");
					this.OnContainsProtectedSheetsChanged();
				}
			}
		}

		internal ResourceManager Resources
		{
			get
			{
				return this.resources;
			}
		}

		public Workbook()
		{
			this.suspendLayoutUpdateCount = new BeginEndCounter();
			this.suspendPropertyChangedCount = new BeginEndCounter();
			this.activeTabIndex = -1;
			this.nameManager = new NameManager(this);
			this.nameManager.Changing += this.NameManager_Changing;
			this.sheets = new SheetCollection(this);
			this.sheets.ActiveSheetChanged += this.Sheets_ActiveSheetChanged;
			this.sheets.Changing += this.Sheets_Changing;
			this.sheets.Changed += this.Sheets_Changed;
			this.worksheets = new WorksheetCollection(this.sheets);
			this.styles = new CellStyleCollection(this);
			this.history = new WorkbookHistory(this);
			this.protectionOptions = WorkbookProtectionOptions.Default;
			this.protectionData = new ProtectionData();
			this.name = "Book1";
			this.resources = new ResourceManager();
			this.theme = PredefinedThemeSchemes.DefaultTheme;
			this.worksheetLayout = new RadWorksheetLayout(new SizeI(SpreadsheetDefaultValues.ColumnCount, SpreadsheetDefaultValues.RowCount));
			this.workbookContentChangedInterval = TimeSpan.FromMilliseconds((double)Workbook.defaultContentChangedInterval);
			this.ThemeChanged += this.Workbook_ThemeChanged;
			this.NameChanged += this.Workbook_NameChanged;
		}

		void Sheets_ActiveSheetChanged(object sender, EventArgs e)
		{
			this.DetachFromActiveWorksheetEvents();
			this.lastActiveWorksheet = this.ActiveWorksheet;
			this.AttachToActiveWorksheetEvents();
			this.OnActiveSheetChanged();
		}

		void Sheets_Changing(object sender, SheetCollectionChangedEventArgs e)
		{
			if (e.ChangeType == SheetCollectionChangeType.Remove)
			{
				e.Sheet.IsProtectedChanged -= this.Sheet_ProtectionStateChanged;
				e.Sheet.PropertyChanged -= this.Sheet_PropertyChanged;
			}
		}

		void Sheets_Changed(object sender, SheetCollectionChangedEventArgs e)
		{
			if (e.ChangeType == SheetCollectionChangeType.Remove)
			{
				this.History.Clear();
			}
			if (e.ChangeType == SheetCollectionChangeType.Add)
			{
				e.Sheet.IsProtectedChanged += this.Sheet_ProtectionStateChanged;
				e.Sheet.PropertyChanged += this.Sheet_PropertyChanged;
			}
			this.CallOnWorkbookContentChanged();
		}

		void Worksheet_NameChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Cells_CellPropertyChanged(object sender, CellPropertyChangedEventArgs e)
		{
			if (!this.isMergedCellsChanging && !this.isHyperlinksChanging)
			{
				this.CallOnWorkbookContentChanged();
			}
			this.OnWorksheetCellPropertyChanged(((Cells)sender).Worksheet, e);
		}

		void Cells_MergedCellsChanging(object sender, EventArgs e)
		{
			this.isMergedCellsChanging = true;
		}

		void Cells_MergedCellsChanged(object sender, MergedCellRangesChangedEventArgs e)
		{
			this.isMergedCellsChanging = false;
			this.CallOnWorkbookContentChanged();
		}

		void Cells_CellRangeInsertedOrRemoved(object sender, CellRangeInsertedOrRemovedEventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Rows_RowsHeightsChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Columns_ColumnsWidthChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Hyperlinks_Changing(object sender, EventArgs e)
		{
			this.isHyperlinksChanging = true;
		}

		void Hyperlinks_Changed(object sender, EventArgs e)
		{
			this.isHyperlinksChanging = false;
			this.CallOnWorkbookContentChanged();
		}

		void Filter_FilterRangeChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Filter_FiltersChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void ShapesCollection_Changed(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Shape_ShapeChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Workbook_ThemeChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		void Workbook_NameChanged(object sender, EventArgs e)
		{
			this.CallOnWorkbookContentChanged();
		}

		internal RadWorksheetLayout GetWorksheetLayout(Worksheet worksheet, bool isForPrinting)
		{
			this.UpdateWorksheetLayout(this.worksheetLayout, worksheet, isForPrinting);
			return this.worksheetLayout;
		}

		void UpdateWorksheetLayout(RadWorksheetLayout worksheetLayout, Worksheet worksheet, bool isForPrinting)
		{
			worksheetLayout.SetWorksheet(worksheet, isForPrinting);
			if (worksheet != null)
			{
				WorksheetViewState worksheetViewState = (WorksheetViewState)((ISheet)worksheet).ViewState;
				CellIndex frozenCellIndex = worksheetViewState.GetFrozenCellIndex();
				worksheetLayout.Measure(frozenCellIndex);
			}
		}

		internal void SetThemeInternal(DocumentTheme value)
		{
			this.EnsureNotDisposed();
			if (!TelerikHelper.EqualsOfT<DocumentTheme>(this.theme, value))
			{
				this.theme = value;
				this.OnThemeChanged();
			}
		}

		public void SuspendLayoutUpdate()
		{
			this.EnsureNotDisposed();
			if (this.suspendLayoutUpdateCount.BeginUpdateCounter == 0)
			{
				foreach (ISheet sheet in this.Sheets)
				{
					sheet.SuspendLayoutUpdate();
				}
			}
			this.suspendLayoutUpdateCount.BeginUpdate();
		}

		public void ResumeLayoutUpdate()
		{
			this.EnsureNotDisposed();
			this.suspendLayoutUpdateCount.EndUpdate();
			if (this.suspendLayoutUpdateCount.BeginUpdateCounter == 0)
			{
				foreach (ISheet sheet in this.Sheets)
				{
					sheet.ResumeLayoutUpdate();
				}
			}
		}

		internal void SuspendPropertyChanged()
		{
			this.EnsureNotDisposed();
			if (this.suspendPropertyChangedCount.BeginUpdateCounter == 0)
			{
				foreach (Worksheet worksheet in this.Worksheets)
				{
					worksheet.Cells.PropertyBag.SuspendPropertyChanged();
				}
			}
			this.suspendPropertyChangedCount.BeginUpdate();
		}

		internal void ResumePropertyChanged()
		{
			this.EnsureNotDisposed();
			this.suspendPropertyChangedCount.EndUpdate();
			if (this.suspendPropertyChangedCount.BeginUpdateCounter == 0)
			{
				foreach (Worksheet worksheet in this.Worksheets)
				{
					worksheet.Cells.PropertyBag.ResumePropertyChanged();
				}
			}
		}

		internal void InvalidateLayout()
		{
			this.EnsureNotDisposed();
			foreach (ISheet sheet in this.Sheets)
			{
				sheet.InvalidateLayout();
			}
		}

		internal bool ExecuteCommand<T>(WorkbookCommandBase<T> command, T context) where T : WorkbookCommandContextBase
		{
			this.EnsureNotDisposed();
			return command.Execute(context);
		}

		public FindResult Find(FindOptions findOptions)
		{
			this.EnsureNotDisposed();
			IEnumerable<FindResult> enumerable = this.FindAll(findOptions);
			enumerable = FindAndReplaceHelper.OrderResults(findOptions, enumerable);
			return enumerable.FirstOrDefault<FindResult>();
		}

		public IEnumerable<FindResult> FindAll(FindOptions findOptions)
		{
			this.EnsureNotDisposed();
			List<FindResult> list = new List<FindResult>();
			IEnumerable<CellRange> searchRanges = findOptions.SearchRanges;
			if (findOptions.FindWithin == FindWithin.Workbook)
			{
				using (IEnumerator<Worksheet> enumerator = this.Worksheets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Worksheet worksheet = enumerator.Current;
						findOptions.SearchRanges = ((findOptions.FindWithin == FindWithin.Sheet && this.ActiveWorksheet == worksheet) ? searchRanges : null);
						list.AddRange(worksheet.FindAll(findOptions));
					}
					goto IL_7F;
				}
			}
			list.AddRange(this.ActiveWorksheet.FindAll(findOptions));
			IL_7F:
			findOptions.SearchRanges = searchRanges;
			return list;
		}

		public bool Replace(ReplaceOptions replaceOptions)
		{
			this.EnsureNotDisposed();
			return this.ActiveWorksheet.Replace(replaceOptions);
		}

		public int ReplaceAll(ReplaceOptions replaceOptions)
		{
			this.EnsureNotDisposed();
			int num = 0;
			using (new UpdateScope(new Action(this.History.BeginUndoGroup), new Action(this.History.EndUndoGroup)))
			{
				if (replaceOptions.FindWithin == FindWithin.Workbook)
				{
					using (IEnumerator<Worksheet> enumerator = this.Worksheets.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Worksheet worksheet = enumerator.Current;
							num += worksheet.ReplaceAll(replaceOptions);
						}
						goto IL_79;
					}
				}
				num = this.ActiveWorksheet.ReplaceAll(replaceOptions);
				IL_79:;
			}
			return num;
		}

		public void Protect(string password)
		{
			if (this.ProtectionData.Enforced)
			{
				throw new ProtectionException("Cannot protect workbook that is already protected", new InvalidOperationException(), "Spreadsheet_Protection_ProtectAProtectedWorkbook_Error");
			}
			this.ProtectionData.EnforceProtection(password);
			this.OnIsProtectedChanged();
		}

		public bool Unprotect(string password)
		{
			if (!this.protectionData.Enforced)
			{
				throw new ProtectionException("Cannot unprotect workbook that is not protected", new InvalidOperationException(), "Spreadsheet_Protection_UnprotectAnUnprotectedWorkbook_Error");
			}
			bool flag = this.ProtectionData.TryRemoveProtection(password);
			if (flag)
			{
				this.OnIsProtectedChanged();
			}
			return flag;
		}

		void AttachToActiveWorksheetEvents()
		{
			if (this.lastActiveWorksheet != null)
			{
				Worksheet worksheet = this.lastActiveWorksheet;
				worksheet.NameChanged += this.Worksheet_NameChanged;
				worksheet.Cells.CellPropertyChanged += this.Cells_CellPropertyChanged;
				worksheet.Cells.MergedCellsChanging += this.Cells_MergedCellsChanging;
				worksheet.Cells.MergedCellsChanged += this.Cells_MergedCellsChanged;
				worksheet.Cells.CellRangeInsertedOrRemoved += this.Cells_CellRangeInsertedOrRemoved;
				worksheet.Columns.ColumnsWidthChanged += this.Columns_ColumnsWidthChanged;
				worksheet.Rows.RowsHeightsChanged += this.Rows_RowsHeightsChanged;
				worksheet.Hyperlinks.Changing += this.Hyperlinks_Changing;
				worksheet.Hyperlinks.Changed += this.Hyperlinks_Changed;
				worksheet.Filter.FilterRangeChanged += this.Filter_FilterRangeChanged;
				worksheet.Filter.FiltersChanged += this.Filter_FiltersChanged;
				worksheet.Shapes.Changed += this.ShapesCollection_Changed;
				worksheet.Shapes.ShapeChanged += this.Shape_ShapeChanged;
			}
		}

		void DetachFromActiveWorksheetEvents()
		{
			if (this.lastActiveWorksheet != null)
			{
				Worksheet worksheet = this.lastActiveWorksheet;
				worksheet.NameChanged -= this.Worksheet_NameChanged;
				worksheet.Cells.CellPropertyChanged -= this.Cells_CellPropertyChanged;
				worksheet.Cells.MergedCellsChanging -= this.Cells_MergedCellsChanging;
				worksheet.Cells.MergedCellsChanged -= this.Cells_MergedCellsChanged;
				worksheet.Cells.CellRangeInsertedOrRemoved -= this.Cells_CellRangeInsertedOrRemoved;
				worksheet.Columns.ColumnsWidthChanged -= this.Columns_ColumnsWidthChanged;
				worksheet.Rows.RowsHeightsChanged -= this.Rows_RowsHeightsChanged;
				worksheet.Hyperlinks.Changing -= this.Hyperlinks_Changing;
				worksheet.Hyperlinks.Changed -= this.Hyperlinks_Changed;
				worksheet.Filter.FilterRangeChanged -= this.Filter_FilterRangeChanged;
				worksheet.Filter.FiltersChanged -= this.Filter_FiltersChanged;
				worksheet.Shapes.Changed -= this.ShapesCollection_Changed;
				worksheet.Shapes.ShapeChanged -= this.Shape_ShapeChanged;
			}
		}

		void EnsureContentChangedExecutorInitialized()
		{
			if (this.contentChangedExecutor == null)
			{
				this.contentChangedExecutor = new DelayedExecution(delegate()
				{
					this.OnWorkbookContentChanged();
				}, (long)this.WorkbookContentChangedInterval.TotalMilliseconds);
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool cleanUpManagedResources)
		{
			if (this.alreadyDisposed)
			{
				return;
			}
			if (cleanUpManagedResources && this.contentChangedExecutor != null)
			{
				this.contentChangedExecutor.Dispose();
			}
			this.alreadyDisposed = true;
		}

		void EnsureNotDisposed()
		{
			if (this.alreadyDisposed)
			{
				throw new ObjectDisposedException("Workbook");
			}
		}

		void Sheet_ProtectionStateChanged(object sender, EventArgs e)
		{
			this.UpdateContainsProtectedSheets();
		}

		internal void UpdateContainsProtectedSheets()
		{
			bool flag = false;
			for (int i = 0; i < this.sheets.Count; i++)
			{
				if (this.sheets[i].IsProtected)
				{
					flag = true;
					break;
				}
			}
			this.ContainsProtectedSheets = flag;
		}

		void UpdateWorkbookNameDependentValuesAndNames()
		{
			for (int i = 0; i < this.Worksheets.Count; i++)
			{
				this.Worksheets[i].Cells.UpdateWorkbookNameDependentCellValues(this);
				this.Worksheets[i].Names.UpdateWorkbookNameDependentNames(this);
			}
			this.Names.UpdateWorkbookNameDependentNames(this);
		}

		internal void UpdateWorksheetDependentValuesAndNames(Sheet renamedSheet)
		{
			Worksheet renamedWorksheet = (Worksheet)renamedSheet;
			for (int i = 0; i < this.Worksheets.Count; i++)
			{
				this.Worksheets[i].Cells.UpdateSheetNameDependentCellValues(renamedWorksheet);
				this.Worksheets[i].Names.UpdateSheetNameDependentNames(renamedWorksheet);
			}
			this.Names.UpdateSheetNameDependentNames(renamedWorksheet);
		}

		void NameManager_Changing(object sender, NameCollectionChangingEventArgs args)
		{
			if (!string.IsNullOrEmpty(args.OldName) && !string.IsNullOrEmpty(args.NewName) && !args.OldName.Equals(args.NewName, StringComparison.CurrentCultureIgnoreCase))
			{
				for (int i = 0; i < this.Worksheets.Count; i++)
				{
					this.Worksheets[i].Cells.UpdateSpreadsheetNameDependentCellValues(args.OldName, args.NewName);
					this.Worksheets[i].Names.UpdateSpreadsheetNameDependentNames(args.OldName, args.NewName);
				}
				this.Names.UpdateSpreadsheetNameDependentNames(args.OldName, args.NewName);
			}
		}

		void Sheet_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				this.UpdateWorksheetDependentValuesAndNames((Sheet)sender);
			}
		}

		public event EventHandler<CommandExecutingEventArgs> CommandExecuting;

		protected internal virtual void OnCommandExecuting(CommandExecutingEventArgs args)
		{
			EventHandler<CommandExecutingEventArgs> commandExecuting = this.CommandExecuting;
			if (commandExecuting != null)
			{
				foreach (EventHandler<CommandExecutingEventArgs> eventHandler in commandExecuting.GetInvocationList())
				{
					eventHandler(this, args);
					if (args.Canceled)
					{
						return;
					}
				}
			}
		}

		public event EventHandler<CommandExecutedEventArgs> CommandExecuted;

		protected internal virtual void OnCommandExecuted(CommandExecutedEventArgs args)
		{
			if (this.History.IsEnabled && !args.IsInUndo)
			{
				IUndoableWorkbookCommand undoableWorkbookCommand = args.Command as IUndoableWorkbookCommand;
				if (undoableWorkbookCommand != null)
				{
					this.history.RecordSheetChange(new WorkbookChange(undoableWorkbookCommand, args.CommandContext));
				}
			}
			if (args.Command.AffectsLayout(args.CommandContext))
			{
				args.CommandContext.InvalidateLayout();
			}
			if (this.CommandExecuted != null)
			{
				this.CommandExecuted(this, args);
			}
		}

		public event EventHandler<CommandErrorEventArgs> CommandError;

		protected internal virtual void OnCommandError(CommandErrorEventArgs args)
		{
			if (this.CommandError != null)
			{
				this.CommandError(this, args);
			}
		}

		public event EventHandler ThemeChanged;

		protected virtual void OnThemeChanged()
		{
			if (this.ThemeChanged != null)
			{
				this.ThemeChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler ActiveSheetChanged;

		protected virtual void OnActiveSheetChanged()
		{
			if (this.ActiveSheetChanged != null)
			{
				this.ActiveSheetChanged(this, EventArgs.Empty);
			}
		}

		event EventHandler workbookContentChanged;

		public event EventHandler WorkbookContentChanged
		{
			add
			{
				this.EnsureContentChangedExecutorInitialized();
				this.workbookContentChanged += value;
			}
			remove
			{
				this.workbookContentChanged -= value;
			}
		}

		protected virtual void OnWorkbookContentChanged()
		{
			if (this.workbookContentChanged != null)
			{
				this.workbookContentChanged(this, EventArgs.Empty);
			}
		}

		protected void CallOnWorkbookContentChanged()
		{
			if (this.contentChangedExecutor != null)
			{
				this.contentChangedExecutor.Execute();
			}
		}

		public event EventHandler IsProtectedChanged;

		protected virtual void OnIsProtectedChanged()
		{
			if (this.IsProtectedChanged != null)
			{
				this.IsProtectedChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler NameChanged;

		void OnNameChanged()
		{
			if (this.NameChanged != null)
			{
				this.NameChanged(this, EventArgs.Empty);
			}
		}

		internal event EventHandler ContainsProtectedSheetsChanged;

		void OnContainsProtectedSheetsChanged()
		{
			if (this.ContainsProtectedSheetsChanged != null)
			{
				this.ContainsProtectedSheetsChanged(this, EventArgs.Empty);
			}
		}

		void OnWorksheetCellPropertyChanged(Worksheet source, CellPropertyChangedEventArgs e)
		{
			foreach (Worksheet worksheet in this.Worksheets)
			{
				worksheet.Shapes.OnWorksheetCellPropertyChanged(source, e);
			}
		}

		static readonly int defaultContentChangedInterval = 30;

		readonly NameManager nameManager;

		readonly SheetCollection sheets;

		readonly WorksheetCollection worksheets;

		readonly CellStyleCollection styles;

		readonly WorkbookHistory history;

		DocumentTheme theme;

		readonly BeginEndCounter suspendLayoutUpdateCount;

		readonly BeginEndCounter suspendPropertyChangedCount;

		readonly RadWorksheetLayout worksheetLayout;

		Worksheet lastActiveWorksheet;

		bool isMergedCellsChanging;

		bool isHyperlinksChanging;

		TimeSpan workbookContentChangedInterval;

		DelayedExecution contentChangedExecutor;

		bool alreadyDisposed;

		string name;

		WorkbookProtectionOptions protectionOptions;

		readonly ProtectionData protectionData;

		int activeTabIndex;

		bool containsProtectedSheets;

		readonly ResourceManager resources;
	}
}
