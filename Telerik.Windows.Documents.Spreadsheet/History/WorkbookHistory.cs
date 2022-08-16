using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	public class WorkbookHistory
	{
		public bool CanUndo
		{
			get
			{
				return this.undoStack.Count > 0;
			}
		}

		public bool CanRedo
		{
			get
			{
				return this.redoStack.Count > 0;
			}
		}

		public int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				Guard.ThrowExceptionIfLessThan<int>(1, value, "History depth should be a positive number");
				if (this.depth != value)
				{
					this.depth = value;
					this.OnDepthChanged();
				}
			}
		}

		public bool IsInUndoGroup
		{
			get
			{
				return this.undoGroupCounter.IsUpdateInProgress;
			}
		}

		internal bool IsActionMergeSuspended { get; set; }

		public bool IsEnabled
		{
			get
			{
				return this.isEnabled;
			}
			set
			{
				Guard.ThrowExceptionIfTrue(this.IsInUndoGroup, "IsInUndoGroup");
				if (this.isEnabled != value)
				{
					this.isEnabled = value;
					this.OnIsEnabledChanged();
				}
			}
		}

		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		internal WorkbookHistory(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.workbook = workbook;
			this.depth = int.MaxValue;
			this.undoStack = new LinkedList<WorkbookHistoryAction>();
			this.redoStack = new Stack<WorkbookHistoryAction>();
			this.changes = new WorkbookChangesCollection();
			this.undoGroupCounter = new BeginEndCounter();
		}

		void OnDepthChanged()
		{
			this.EnsureUndoStackDepth();
			this.redoStack.Clear();
		}

		void OnIsEnabledChanged()
		{
			if (!this.isEnabled)
			{
				this.Clear();
			}
		}

		public void BeginUndoGroup()
		{
			if (!this.IsEnabled)
			{
				return;
			}
			if (!this.IsInUndoGroup)
			{
				this.Workbook.SuspendLayoutUpdate();
				this.OnRecordExecuting();
			}
			this.undoGroupCounter.BeginUpdate();
		}

		public void EndUndoGroup()
		{
			this.EndUndoGroup(false);
		}

		public void CancelUndoGroup()
		{
			this.EndUndoGroup(true);
		}

		void EndUndoGroup(bool isCanceled)
		{
			if (!this.IsEnabled)
			{
				return;
			}
			int beginUpdateCounter = this.undoGroupCounter.BeginUpdateCounter;
			if (beginUpdateCounter == 1)
			{
				foreach (Worksheet worksheet in this.Workbook.Worksheets)
				{
					worksheet.ExpandCachedColumnWidths();
				}
			}
			this.undoGroupCounter.EndUpdate();
			if (!this.changes.IsEmpty)
			{
				if (isCanceled)
				{
					IEnumerable<WorkbookChange> enumerable = this.changes.PopChangesFromUndoGroupLevel(beginUpdateCounter);
					WorkbookHistoryAction workbookHistoryAction = new WorkbookHistoryAction(enumerable, string.Empty);
					this.state = WorkbookHistory.HistoryState.InCancelUndoGroup;
					workbookHistoryAction.Undo();
					if (!this.IsInUndoGroup)
					{
						this.Workbook.ResumeLayoutUpdate();
					}
					this.state = WorkbookHistory.HistoryState.Listening;
					return;
				}
				if (!this.IsInUndoGroup)
				{
					IEnumerable<WorkbookChange> enumerable2 = this.changes.PopChangesFromUndoGroupLevel(beginUpdateCounter);
					WorkbookHistoryAction workbookHistoryAction2 = new WorkbookHistoryAction(enumerable2, string.Empty);
					WorkbookHistoryAction workbookHistoryAction3 = null;
					if (this.undoStack.Count > 0)
					{
						workbookHistoryAction3 = this.undoStack.Last.Value;
					}
					bool flag = !this.IsActionMergeSuspended && workbookHistoryAction3 != null && workbookHistoryAction3.Merge();
					if (!flag)
					{
						this.undoStack.AddLast(workbookHistoryAction2);
						this.EnsureUndoStackDepth();
					}
					this.Workbook.ResumeLayoutUpdate();
					this.redoStack.Clear();
					this.OnRecordExecuted(workbookHistoryAction2, flag);
					return;
				}
			}
			else if (!this.IsInUndoGroup)
			{
				this.Workbook.ResumeLayoutUpdate();
			}
		}

		internal bool RecordSheetChange(WorkbookChange change)
		{
			if (!this.IsEnabled)
			{
				return false;
			}
			if (this.state != WorkbookHistory.HistoryState.Listening)
			{
				return false;
			}
			if (this.undoStack.Count > this.depth)
			{
				return false;
			}
			bool isInUndoGroup = this.IsInUndoGroup;
			if (!isInUndoGroup)
			{
				this.BeginUndoGroup();
			}
			try
			{
				this.changes.Add(this.undoGroupCounter.BeginUpdateCounter, change);
			}
			finally
			{
				if (!isInUndoGroup)
				{
					this.EndUndoGroup();
				}
			}
			return true;
		}

		public bool Undo()
		{
			if (this.IsInUndoGroup)
			{
				throw new InvalidOperationException("Cannot Undo while in begin undo group.");
			}
			if (!this.CanUndo)
			{
				return false;
			}
			this.state = WorkbookHistory.HistoryState.InUndo;
			WorkbookHistoryAction value = this.undoStack.Last.Value;
			this.RestoreActiveSheet(value);
			this.OnUndoExecuting(value);
			bool flag = this.UndoCore();
			if (flag)
			{
				this.OnUndoExecuted(value);
			}
			this.state = WorkbookHistory.HistoryState.Listening;
			return flag;
		}

		void RestoreActiveSheet(WorkbookHistoryAction action)
		{
			this.Workbook.ActiveSheet = action.ActiveSheet;
		}

		public bool Redo()
		{
			if (this.IsInUndoGroup)
			{
				throw new InvalidOperationException("Cannot Redo while in begin undo group.");
			}
			if (!this.CanRedo)
			{
				return false;
			}
			this.state = WorkbookHistory.HistoryState.InRedo;
			WorkbookHistoryAction workbookHistoryAction = this.redoStack.Peek();
			this.RestoreActiveSheet(workbookHistoryAction);
			this.OnRedoExecuting(workbookHistoryAction);
			bool flag = this.RedoCore();
			if (flag)
			{
				this.OnRedoExecuted(workbookHistoryAction);
			}
			this.state = WorkbookHistory.HistoryState.Listening;
			return flag;
		}

		bool UndoCore()
		{
			WorkbookHistoryAction value = this.undoStack.Last.Value;
			this.undoStack.RemoveLast();
			value.Undo();
			if (this.redoStack.Count >= this.depth)
			{
				return false;
			}
			this.redoStack.Push(value);
			return true;
		}

		bool RedoCore()
		{
			WorkbookHistoryAction workbookHistoryAction = this.redoStack.Pop();
			workbookHistoryAction.Redo();
			if (this.undoStack.Count >= this.depth)
			{
				return false;
			}
			this.undoStack.AddLast(workbookHistoryAction);
			return true;
		}

		public void Clear()
		{
			if (this.IsInUndoGroup)
			{
				throw new InvalidOperationException("Clear Undo while in begin undo group");
			}
			this.undoStack.Clear();
			this.redoStack.Clear();
			this.OnCleared();
		}

		void EnsureUndoStackDepth()
		{
			while (this.undoStack.Count > this.depth)
			{
				this.undoStack.RemoveFirst();
			}
		}

		internal void Clear(Sheet associatedSheetToClear)
		{
			LinkedList<WorkbookHistoryAction>.Enumerator enumerator = this.undoStack.GetEnumerator();
			LinkedListNode<WorkbookHistoryAction> linkedListNode = this.undoStack.First;
			List<WorkbookHistoryAction> list = new List<WorkbookHistoryAction>();
			while (enumerator.MoveNext())
			{
				LinkedListNode<WorkbookHistoryAction> next = linkedListNode.Next;
				WorkbookHistoryAction value = linkedListNode.Value;
				if (value.ActiveSheet.Equals(associatedSheetToClear))
				{
					list.Add(value);
				}
				if (next != null)
				{
					linkedListNode = next;
				}
			}
			foreach (WorkbookHistoryAction value2 in list)
			{
				this.undoStack.Remove(value2);
			}
			this.redoStack.Clear();
		}

		public event EventHandler RecordExecuting;

		protected virtual void OnRecordExecuting()
		{
			if (this.RecordExecuting != null)
			{
				this.RecordExecuting(this, EventArgs.Empty);
			}
		}

		public event EventHandler<WorkbookHistoryRecordExecutedEventArgs> RecordExecuted;

		protected virtual void OnRecordExecuted(WorkbookHistoryAction historyAction, bool isMerged)
		{
			if (this.RecordExecuted != null)
			{
				WorkbookHistoryRecordExecutedEventArgs e = new WorkbookHistoryRecordExecutedEventArgs(historyAction, isMerged);
				this.RecordExecuted(this, e);
			}
		}

		public event EventHandler<WorkbookHistoryEventArgs> UndoExecuting;

		protected virtual void OnUndoExecuting(WorkbookHistoryAction historyAction)
		{
			if (this.UndoExecuting != null)
			{
				WorkbookHistoryEventArgs e = new WorkbookHistoryEventArgs(historyAction);
				this.UndoExecuting(this, e);
			}
		}

		public event EventHandler<WorkbookHistoryEventArgs> UndoExecuted;

		protected virtual void OnUndoExecuted(WorkbookHistoryAction historyAction)
		{
			if (this.UndoExecuted != null)
			{
				WorkbookHistoryEventArgs e = new WorkbookHistoryEventArgs(historyAction);
				this.UndoExecuted(this, e);
			}
		}

		public event EventHandler<WorkbookHistoryEventArgs> RedoExecuting;

		protected virtual void OnRedoExecuting(WorkbookHistoryAction historyAction)
		{
			if (this.RedoExecuting != null)
			{
				WorkbookHistoryEventArgs e = new WorkbookHistoryEventArgs(historyAction);
				this.RedoExecuting(this, e);
			}
		}

		public event EventHandler<WorkbookHistoryEventArgs> RedoExecuted;

		protected virtual void OnRedoExecuted(WorkbookHistoryAction historyAction)
		{
			if (this.RedoExecuted != null)
			{
				WorkbookHistoryEventArgs e = new WorkbookHistoryEventArgs(historyAction);
				this.RedoExecuted(this, e);
			}
		}

		public event EventHandler Cleared;

		protected virtual void OnCleared()
		{
			if (this.Cleared != null)
			{
				this.Cleared(this, EventArgs.Empty);
			}
		}

		public const int DefaultDepth = 2147483647;

		readonly Workbook workbook;

		int depth;

		bool isEnabled;

		WorkbookHistory.HistoryState state;

		readonly BeginEndCounter undoGroupCounter;

		readonly WorkbookChangesCollection changes;

		readonly LinkedList<WorkbookHistoryAction> undoStack;

		readonly Stack<WorkbookHistoryAction> redoStack;

		enum HistoryState
		{
			Listening,
			InUndo,
			InRedo,
			InCancelUndoGroup
		}
	}
}
