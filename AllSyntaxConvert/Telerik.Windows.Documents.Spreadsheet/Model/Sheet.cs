using System;
using Telerik.Windows.Documents.Spreadsheet.Commands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model.Printing;
using Telerik.Windows.Documents.Spreadsheet.Model.Protection;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class Sheet : NotifyPropertyChangedBase, ISheet, IDisposable
	{
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				Guard.ThrowExceptionIfNull<string>(value, "value");
				if (this.name != value)
				{
					if (this.Workbook != null)
					{
						if (!SheetCollection.IsValidSheetName(value))
						{
							throw new LocalizableException(SpreadsheetStrings.InvalidSheetNameMessage, new InvalidOperationException(SpreadsheetStrings.InvalidSheetNameMessage), "Spreadsheet_ErrorExpressions_InvalidSheetNameErrorMessage", null);
						}
						if (!this.Workbook.Sheets.IsUniqueSheetNameInputed(value, this.name))
						{
							throw new LocalizableException(string.Format(SpreadsheetStrings.ExistingSheetName, this.name), new InvalidOperationException(string.Format("Sheet with Name={0} already exist.", this.name)), "Spreadsheet_ErrorExpressions_ExistingSheetName", new string[] { this.name });
						}
					}
					this.name = value;
					this.OnNameChanged();
					base.OnPropertyChanged("Name");
				}
			}
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
			set
			{
				if (this.workbook != value)
				{
					if (value != null && this.workbook != null)
					{
						throw new LocalizableException(SpreadsheetStrings.SheetAlreadyAdded, new InvalidOperationException(SpreadsheetStrings.SheetAlreadyAdded), "Spreadsheet_ErrorExpressions_SheetAlreadyAdded", null);
					}
					this.workbook = value;
					base.OnPropertyChanged("Workbook");
				}
			}
		}

		public SheetVisibility Visibility
		{
			get
			{
				return this.visibility;
			}
			set
			{
				if (this.visibility != value)
				{
					this.visibility = value;
					base.OnPropertyChanged("Visibility");
				}
			}
		}

		public abstract SheetType Type { get; }

		public bool IsLayoutUpdateSuspended
		{
			get
			{
				return this.suspendLayoutUpdateCounter.IsUpdateInProgress;
			}
		}

		ISheetViewState ISheet.ViewState
		{
			get
			{
				if (this.viewState.IsInvalidated)
				{
					this.viewState.IsInvalidated = false;
					this.OnViewStateUpdateNeeded();
				}
				return this.viewState;
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

		protected abstract SheetPageSetupBase SheetPageSetup { get; }

		internal bool IsResumingLayoutUpdate { get; set; }

		internal Sheet(string name, Workbook workbook)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.Name = name;
			this.workbook = workbook;
			this.viewState = this.CreateViewState();
			this.suspendLayoutUpdateCounter = new BeginEndCounter();
			this.protectionData = new ProtectionData();
		}

		protected abstract ISheetViewState CreateViewState();

		void ISheet.SuspendLayoutUpdate()
		{
			this.SuspendLayoutUpdate();
		}

		void ISheet.ResumeLayoutUpdate()
		{
			this.ResumeLayoutUpdate();
		}

		protected virtual void SuspendLayoutUpdate()
		{
			this.suspendLayoutUpdateCounter.BeginUpdate();
		}

		protected virtual void ResumeLayoutUpdate()
		{
			this.suspendLayoutUpdateCounter.EndUpdate();
			if (!this.IsLayoutUpdateSuspended)
			{
				if (this.hasLayoutInvalidation)
				{
					this.RaiseOnLayoutInvalidated();
					this.IsResumingLayoutUpdate = true;
				}
				this.OnLayoutUpdateResumed();
			}
		}

		internal virtual void OnLayoutUpdateResumed()
		{
		}

		public void BeginUndoGroup()
		{
			this.Workbook.History.BeginUndoGroup();
		}

		public void EndUndoGroup()
		{
			this.Workbook.History.EndUndoGroup();
		}

		internal void EnsureSheetNotProtected()
		{
			if (this.ProtectionData.Enforced)
			{
				throw new ProtectionException("Cannot protect a sheet that is already protected.", "Spreadsheet_Protection_ProtectAProtectedSheet_Error");
			}
		}

		internal void EnsureSheetProtected()
		{
			if (!this.ProtectionData.Enforced)
			{
				throw new ProtectionException("Cannot unprotect a sheet that is not protected.", "Spreadsheet_Protection_UnprotectAnUnprotectedSheet_Error");
			}
		}

		internal void ProtectSheet(string password)
		{
			this.ProtectionData.EnforceProtection(password);
			this.Workbook.History.Clear(this);
			this.OnIsProtectedChanged();
		}

		internal bool UnprotectSheet(string password)
		{
			this.EnsureSheetProtected();
			bool flag = this.ProtectionData.TryRemoveProtection(password);
			if (flag)
			{
				this.OnIsProtectedChanged();
			}
			return flag;
		}

		void ISheet.InvalidateLayout()
		{
			this.InvalidateLayout();
		}

		internal void InvalidateLayout()
		{
			this.hasLayoutInvalidation = true;
			if (this.IsLayoutUpdateSuspended)
			{
				return;
			}
			this.IsResumingLayoutUpdate = false;
			this.RaiseOnLayoutInvalidated();
		}

		internal bool ExecuteCommand<T>(WorkbookCommandBase<T> command, T context) where T : SheetCommandContextBase
		{
			return command.Execute(context);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.alreadyDisposed)
			{
				if (disposing)
				{
					this.workbook = null;
				}
				this.alreadyDisposed = true;
			}
		}

		void RaiseOnLayoutInvalidated()
		{
			this.hasLayoutInvalidation = false;
			this.OnLayoutInvalidated();
		}

		public event EventHandler LayoutInvalidated;

		protected virtual void OnLayoutInvalidated()
		{
			Guard.ThrowExceptionIfTrue(this.IsLayoutUpdateSuspended, "IsUpdateInProgress");
			if (this.LayoutInvalidated != null)
			{
				this.LayoutInvalidated(this, EventArgs.Empty);
			}
		}

		internal event EventHandler ViewStateUpdateNeeded;

		void OnViewStateUpdateNeeded()
		{
			if (this.ViewStateUpdateNeeded != null)
			{
				this.ViewStateUpdateNeeded(this, EventArgs.Empty);
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

		public event EventHandler IsProtectedChanged;

		protected virtual void OnIsProtectedChanged()
		{
			if (this.IsProtectedChanged != null)
			{
				this.IsProtectedChanged(this, EventArgs.Empty);
			}
		}

		string name;

		Workbook workbook;

		SheetVisibility visibility;

		bool hasLayoutInvalidation;

		readonly ISheetViewState viewState;

		bool alreadyDisposed;

		readonly BeginEndCounter suspendLayoutUpdateCounter;

		readonly ProtectionData protectionData;
	}
}
