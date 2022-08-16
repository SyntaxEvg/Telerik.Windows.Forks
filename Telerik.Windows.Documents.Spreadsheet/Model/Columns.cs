using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Columns : RowsColumnsBase
	{
		internal override PropertyBagBase PropertyBagBase
		{
			get
			{
				return this.PropertyBag;
			}
		}

		public override int Count
		{
			get
			{
				return base.Worksheet.Cells.ColumnCount;
			}
		}

		internal ColumnsPropertyBag PropertyBag
		{
			get
			{
				return this.propertyBag;
			}
		}

		internal IProperty<ColumnWidth> WidthProperty
		{
			get
			{
				return this.widthProperty;
			}
		}

		public ColumnSelection this[int index]
		{
			get
			{
				return this.GetColumnSelection(index);
			}
		}

		public ColumnSelection this[int fromIndex, int toIndex]
		{
			get
			{
				return this.GetColumnSelection(fromIndex, toIndex);
			}
		}

		public ColumnSelection this[CellIndex cellIndex]
		{
			get
			{
				return this.GetColumnSelection(cellIndex);
			}
		}

		public ColumnSelection this[CellRange cellRange]
		{
			get
			{
				return this.GetColumnSelection(cellRange);
			}
		}

		public ColumnSelection this[IEnumerable<CellRange> cellRanges]
		{
			get
			{
				return this.GetColumnSelection(cellRanges);
			}
		}

		internal Columns(Worksheet worksheet)
			: base(worksheet)
		{
			this.propertyBag = new ColumnsPropertyBag();
			this.PropertyBag.PropertyChanged += this.PropertyBag_PropertyChanged;
			this.widthProperty = base.CreateProperty<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
		}

		void PropertyBag_PropertyChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			if (e.Property == ColumnsPropertyBag.WidthProperty)
			{
				this.OnColumnsWidthChanged(e);
			}
		}

		public override bool CanInsert(int index, int itemCount)
		{
			return this.PropertyBag.CanInsert() && base.Cells.PropertyBag.CanInsertColumn(itemCount);
		}

		protected override bool InsertOverride(int index, int itemCount)
		{
			InsertColumnCommandContext context = new InsertColumnCommandContext(base.Worksheet, index, itemCount);
			bool result = base.Worksheet.ExecuteCommand<InsertColumnCommandContext>(WorkbookCommands.InsertColumn, context);
			CellRange cellRange = CellRange.FromColumnRange(index, index + itemCount - 1);
			base.Cells.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, RangeType.Columns, false);
			return result;
		}

		protected override void RemoveOverride(int index, int itemCount)
		{
			RemoveColumnCommandContext context = new RemoveColumnCommandContext(base.Worksheet, index, itemCount);
			base.Worksheet.ExecuteCommand<RemoveColumnCommandContext>(WorkbookCommands.RemoveColumn, context);
			CellRange cellRange = CellRange.FromColumnRange(index, index + itemCount - 1);
			base.Cells.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, RangeType.Columns, true);
		}

		internal bool GetIsHidden(int columnIndex)
		{
			return this.PropertyBag.GetPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty, columnIndex);
		}

		internal override IProperty<T> CreatePropertyOverride<T>(IPropertyDefinition<T> propertyDefinition)
		{
			return new ColumnProperty<T>(base.Worksheet, propertyDefinition);
		}

		public ColumnSelection GetColumnSelection(int index)
		{
			return this.GetColumnSelection(index, index);
		}

		public ColumnSelection GetColumnSelection(int fromIndex, int toIndex)
		{
			return this.GetColumnSelection(CellRange.FromColumnRange(fromIndex, toIndex));
		}

		public ColumnSelection GetColumnSelection(CellIndex cellIndex)
		{
			return this.GetColumnSelection(new CellRange(cellIndex, cellIndex));
		}

		public ColumnSelection GetColumnSelection(CellRange cellRange)
		{
			return this.GetColumnSelection(new CellRange[] { cellRange });
		}

		public ColumnSelection GetColumnSelection(IEnumerable<CellRange> cellRanges)
		{
			return new ColumnSelection(base.Worksheet, cellRanges);
		}

		public ColumnWidth GetDefaultWidth()
		{
			return this.PropertyBag.GetDefaultPropertyValue<ColumnWidth>(ColumnsPropertyBag.WidthProperty);
		}

		public void SetDefaultWidth(ColumnWidth width)
		{
			SetDefaultPropertyValueCommandContext<ColumnWidth> context = new SetDefaultPropertyValueCommandContext<ColumnWidth>(base.Worksheet, ColumnsPropertyBag.WidthProperty, this.PropertyBag, width);
			base.Worksheet.ExecuteCommand<SetDefaultPropertyValueCommandContext<ColumnWidth>>(WorkbookCommands.SetDefaultColumnWidth, context);
		}

		public event EventHandler<RowColumnPropertyChangedEventArgs> ColumnsWidthChanged;

		void OnColumnsWidthChanged(RowColumnPropertyChangedEventArgs args)
		{
			if (this.ColumnsWidthChanged != null)
			{
				this.ColumnsWidthChanged(this, args);
			}
		}

		readonly ColumnsPropertyBag propertyBag;

		readonly IProperty<ColumnWidth> widthProperty;
	}
}
