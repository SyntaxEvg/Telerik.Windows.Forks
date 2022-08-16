using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.History
{
	class WorkbookChangesCollection
	{
		public bool IsEmpty
		{
			get
			{
				return this.cahngesLevels.Count == 0;
			}
		}

		public WorkbookChangesCollection()
		{
			this.cahngesLevels = new Stack<Tuple<int, Stack<WorkbookChange>>>();
		}

		public void Add(int undoGroupLevel, WorkbookChange change)
		{
			if (this.IsEmpty || this.cahngesLevels.Peek().Item1 < undoGroupLevel)
			{
				this.cahngesLevels.Push(new Tuple<int, Stack<WorkbookChange>>(undoGroupLevel, new Stack<WorkbookChange>()));
			}
			this.cahngesLevels.Peek().Item2.Push(change);
		}

		public IEnumerable<WorkbookChange> PopChangesFromUndoGroupLevel(int undoGroupLevel)
		{
			List<WorkbookChange> list = new List<WorkbookChange>();
			while (!this.IsEmpty && this.cahngesLevels.Peek().Item1 >= undoGroupLevel)
			{
				list.AddRange(this.cahngesLevels.Pop().Item2);
			}
			list.Reverse();
			return list;
		}

		readonly Stack<Tuple<int, Stack<WorkbookChange>>> cahngesLevels;
	}
}
