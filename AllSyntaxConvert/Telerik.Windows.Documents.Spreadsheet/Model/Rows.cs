using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class Rows : RowsColumnsBase
	{
		public override int Count
		{
			get
			{
				return base.Worksheet.Cells.RowCount;
			}
		}

		internal RowsPropertyBag PropertyBag
		{
			get
			{
				return this.propertyBag;
			}
		}

		internal override PropertyBagBase PropertyBagBase
		{
			get
			{
				return this.PropertyBag;
			}
		}

		internal IProperty<RowHeight> HeightProperty
		{
			get
			{
				return this.heightProperty;
			}
		}

		public RowSelection this[int index]
		{
			get
			{
				return this.GetRowSelection(index);
			}
		}

		public RowSelection this[int fromIndex, int toIndex]
		{
			get
			{
				return this.GetRowSelection(fromIndex, toIndex);
			}
		}

		public RowSelection this[CellIndex cellIndex]
		{
			get
			{
				return this.GetRowSelection(cellIndex);
			}
		}

		public RowSelection this[CellRange cellRange]
		{
			get
			{
				return this.GetRowSelection(cellRange);
			}
		}

		public RowSelection this[IEnumerable<CellRange> cellRanges]
		{
			get
			{
				return this.GetRowSelection(cellRanges);
			}
		}

		internal Rows(Worksheet worksheet)
			: base(worksheet)
		{
			this.propertyBag = new RowsPropertyBag();
			this.PropertyBag.PropertyChanged += this.PropertyBag_PropertyChanged;
			this.heightProperty = base.CreateProperty<RowHeight>(RowsPropertyBag.HeightProperty);
		}

		void PropertyBag_PropertyChanged(object sender, RowColumnPropertyChangedEventArgs e)
		{
			if (e.Property == RowsPropertyBag.HeightProperty)
			{
				this.OnRowsHeightChanged(e);
			}
		}

		public override bool CanInsert(int index, int itemCount)
		{
			return this.PropertyBag.CanInsert() && base.Cells.PropertyBag.CanInsertRow(itemCount);
		}

		protected override bool InsertOverride(int index, int itemCount)
		{
			InsertRowCommandContext context = new InsertRowCommandContext(base.Worksheet, index, itemCount);
			bool result = base.Worksheet.ExecuteCommand<InsertRowCommandContext>(WorkbookCommands.InsertRow, context);
			CellRange cellRange = CellRange.FromRowRange(index, index + itemCount - 1);
			base.Cells.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, RangeType.Rows, false);
			return result;
		}

		protected override void RemoveOverride(int index, int itemCount)
		{
			RemoveRowCommandContext context = new RemoveRowCommandContext(base.Worksheet, index, itemCount);
			base.Worksheet.ExecuteCommand<RemoveRowCommandContext>(WorkbookCommands.RemoveRow, context);
			CellRange cellRange = CellRange.FromRowRange(index, index + itemCount - 1);
			base.Cells.UpdateCellRangeInsertedOrRemovedDependentCollections(cellRange, RangeType.Rows, true);
		}

		internal bool GetIsHidden(int rowIndex)
		{
			return this.PropertyBag.GetPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty, rowIndex);
		}

		internal override IProperty<T> CreatePropertyOverride<T>(IPropertyDefinition<T> propertyDefinition)
		{
			return new RowProperty<T>(base.Worksheet, propertyDefinition);
		}

		public RowSelection GetRowSelection(int index)
		{
			return this.GetRowSelection(index, index);
		}

		public RowSelection GetRowSelection(int fromIndex, int toIndex)
		{
			return this.GetRowSelection(CellRange.FromRowRange(fromIndex, toIndex));
		}

		public RowSelection GetRowSelection(CellIndex cellIndex)
		{
			return this.GetRowSelection(new CellRange(cellIndex, cellIndex));
		}

		public RowSelection GetRowSelection(CellRange cellRange)
		{
			return this.GetRowSelection(new CellRange[] { cellRange });
		}

		public RowSelection GetRowSelection(IEnumerable<CellRange> cellRanges)
		{
			return new RowSelection(base.Worksheet, cellRanges);
		}

		public RowHeight GetDefaultHeight()
		{
			return this.PropertyBag.GetDefaultPropertyValue<RowHeight>(RowsPropertyBag.HeightProperty);
		}

		public void SetDefaultHeight(RowHeight height)
		{
			SetDefaultPropertyValueCommandContext<RowHeight> context = new SetDefaultPropertyValueCommandContext<RowHeight>(base.Worksheet, RowsPropertyBag.HeightProperty, this.PropertyBag, height);
			base.Worksheet.ExecuteCommand<SetDefaultPropertyValueCommandContext<RowHeight>>(WorkbookCommands.SetDefaultRowHeight, context);
		}

		public event EventHandler<RowColumnPropertyChangedEventArgs> RowsHeightsChanged;

		void OnRowsHeightChanged(RowColumnPropertyChangedEventArgs args)
		{
			if (this.RowsHeightsChanged != null)
			{
				this.RowsHeightsChanged(this, args);
			}
		}

		readonly RowsPropertyBag propertyBag;

		readonly IProperty<RowHeight> heightProperty;
	}
}
