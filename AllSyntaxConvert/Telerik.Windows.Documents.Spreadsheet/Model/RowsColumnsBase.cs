using System;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class RowsColumnsBase : WorksheetEntityBase
	{
		protected Cells Cells
		{
			get
			{
				return base.Worksheet.Cells;
			}
		}

		public abstract int Count { get; }

		internal IProperty<bool> HiddenProperty
		{
			get
			{
				return this.hiddenProperty;
			}
		}

		internal IProperty<int> OutlineLevelProperty
		{
			get
			{
				return this.outlineLevelProperty;
			}
		}

		internal RowsColumnsBase(Worksheet worksheet)
			: base(worksheet)
		{
			this.hiddenProperty = base.CreateProperty<bool>(RowColumnPropertyBagBase.HiddenProperty);
			this.outlineLevelProperty = base.CreateProperty<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
		}

		public bool Insert(int index)
		{
			return this.Insert(index, 1);
		}

		public bool Insert(int index, int itemCount)
		{
			bool result = false;
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				result = this.InsertOverride(index, itemCount);
			}
			return result;
		}

		public void Remove(int index)
		{
			this.Remove(index, 1);
		}

		public void Remove(int index, int itemCount)
		{
			using (new UpdateScope(new Action(base.Worksheet.BeginUndoGroup), new Action(base.Worksheet.EndUndoGroup)))
			{
				this.RemoveOverride(index, itemCount);
			}
		}

		public abstract bool CanInsert(int index, int itemCount);

		protected abstract bool InsertOverride(int index, int itemCount);

		protected abstract void RemoveOverride(int index, int itemCount);

		readonly IProperty<bool> hiddenProperty;

		readonly IProperty<int> outlineLevelProperty;
	}
}
