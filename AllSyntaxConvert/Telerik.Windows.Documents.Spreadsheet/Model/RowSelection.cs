using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class RowSelection : RowColumnSelectionBase
	{
		internal override RowColumnPropertyBagBase PropertyBag
		{
			get
			{
				return base.Worksheet.Rows.PropertyBag;
			}
		}

		internal override WorksheetEntityBase WorksheetEntity
		{
			get
			{
				return base.Worksheet.Rows;
			}
		}

		internal RowSelection(Worksheet worksheet, IEnumerable<CellRange> cellRanges)
			: base(worksheet, cellRanges)
		{
		}

		public override void SetBorders(CellBorders value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetRowBordersCommandContext context = new SetRowBordersCommandContext(this.Worksheet, cellRange, value);
				this.Worksheet.ExecuteCommand<SetRowBordersCommandContext>(WorkbookCommands.SetRowBorders, context);
			});
		}

		protected override void GetFromToIndexFromRange(CellRange cellRange, out int fromIndex, out int toIndex)
		{
			fromIndex = cellRange.FromIndex.RowIndex;
			toIndex = cellRange.ToIndex.RowIndex;
		}

		internal override void PreserveAsLocalPropertyIfStyleIsApplied<T>(CellRange cellRange, IProperty<T> property, T value)
		{
			IPropertyDefinition<T> propertyDefinition = property.PropertyDefinition as IPropertyDefinition<T>;
			IProperty<T> property2;
			if (base.Worksheet.Cells.TryGetProperyFromPropertyDefinition<T>(propertyDefinition, out property2))
			{
				CellsPropertyBag propertyBag = base.Worksheet.Cells.PropertyBag;
				ICompressedList<string> propertyValueCollection = propertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
				foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
				{
					ICompressedList<string> value2 = propertyValueCollection.GetValue(longRange.Start, longRange.End);
					Range<long, string>[] array = value2.GetNonDefaultRanges().ToArray<Range<long, string>>();
					foreach (Range<long, string> range in array)
					{
						CellStyle byName = base.Worksheet.Workbook.Styles.GetByName(range.Value);
						if (byName.GetIsPropertyIncluded(propertyDefinition))
						{
							CellRange cellRange2 = WorksheetPropertyBagBase.ConvertLongCellRangeToCellRange(range.Start, range.End);
							property2.SetValue(cellRange2, value);
						}
					}
				}
			}
		}

		public RangePropertyValue<RowHeight> GetHeight()
		{
			return base.GetPropertyValue<RowHeight>(RowsPropertyBag.HeightProperty);
		}

		public void SetHeight(RowHeight value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetRowColumnPropertyCommandContext<RowHeight> context = new SetRowColumnPropertyCommandContext<RowHeight>(this.Worksheet, RowsPropertyBag.HeightProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, value);
				this.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<RowHeight>>(WorkbookCommands.SetRowHeight, context);
			});
		}

		internal void SetHeight(RowHeight value, bool considerHidden)
		{
			if (!considerHidden)
			{
				this.SetHeight(value);
			}
			if (value.Value == 0.0)
			{
				this.SetHidden(true);
				return;
			}
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				this.ClearHidden();
				this.SetHeight(value);
			}
		}

		public void ClearHeight()
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				ClearRowColumnPropertyCommandContext<RowHeight> context = new ClearRowColumnPropertyCommandContext<RowHeight>(base.Worksheet, RowsPropertyBag.HeightProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex);
				base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<RowHeight>>(WorkbookCommands.SetRowHeight, context);
			});
		}

		public void AutoFitHeight()
		{
			this.SetHeight(RowHeight.AutoFit);
		}

		internal override void SetOutlineLevelOnRange(CellRange cellRange, int value)
		{
			SetRowColumnPropertyCommandContext<int> context = new SetRowColumnPropertyCommandContext<int>(base.Worksheet, RowColumnPropertyBagBase.OutlineLevelProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, value);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<int>>(WorkbookCommands.SetRowOutlineLevel, context);
		}

		public override RangePropertyValue<int> GetOutlineLevel()
		{
			return base.GetPropertyValue<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
		}

		internal override void ClearOutlineLevelOnRange(CellRange cellRange)
		{
			ClearRowColumnPropertyCommandContext<int> context = new ClearRowColumnPropertyCommandContext<int>(base.Worksheet, RowColumnPropertyBagBase.OutlineLevelProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<int>>(WorkbookCommands.SetRowOutlineLevel, context);
		}

		internal override RowColumnSelectionBase GetRowColumnSelection(int fromIndex, int toIndex)
		{
			return base.Worksheet.Rows[fromIndex, toIndex];
		}

		public void SetHidden(bool value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetRowColumnPropertyCommandContext<bool> context = new SetRowColumnPropertyCommandContext<bool>(this.Worksheet, RowColumnPropertyBagBase.HiddenProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, value);
				this.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<bool>>(WorkbookCommands.SetRowHidden, context);
			});
		}

		public RangePropertyValue<bool> GetHidden()
		{
			return base.GetPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty);
		}

		public void ClearHidden()
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				ClearRowColumnPropertyCommandContext<bool> context = new ClearRowColumnPropertyCommandContext<bool>(base.Worksheet, RowColumnPropertyBagBase.HiddenProperty, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex);
				base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<bool>>(WorkbookCommands.SetRowHidden, context);
			});
		}

		public bool Insert()
		{
			IOrderedEnumerable<CellRange> cellRanges = from r in base.CellRanges
				orderby r.FromIndex.RowIndex descending
				select r;
			return base.ExecuteForEachRangeInsideBeginEndUpdate(cellRanges, delegate(CellRange cellRange)
			{
				base.Worksheet.Rows.Insert(cellRange.FromIndex.RowIndex, cellRange.RowCount);
			}, (CellRange cellRange) => base.Worksheet.Rows.CanInsert(cellRange.FromIndex.RowIndex, cellRange.RowCount));
		}

		public void Remove()
		{
			ICompressedList<bool> compressedList = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.RowCount - 1));
			foreach (CellRange cellRange2 in base.CellRanges)
			{
				compressedList.SetValue((long)cellRange2.FromIndex.RowIndex, (long)cellRange2.ToIndex.RowIndex, true);
			}
			List<CellRange> list = new List<CellRange>();
			foreach (Range<long, bool> range in compressedList)
			{
				if (range.Value)
				{
					list.Add(new CellRange((int)range.Start, 0, (int)range.End, 0));
				}
			}
			list.Reverse();
			base.ExecuteForEachRangeInsideBeginEndUpdate(list, delegate(CellRange cellRange)
			{
				base.Worksheet.Rows.Remove(cellRange.FromIndex.RowIndex, cellRange.RowCount);
			}, null);
		}
	}
}
