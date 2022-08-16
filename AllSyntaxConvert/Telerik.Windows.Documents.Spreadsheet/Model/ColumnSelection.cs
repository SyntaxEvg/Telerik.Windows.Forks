using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class ColumnSelection : RowColumnSelectionBase
	{
		internal override RowColumnPropertyBagBase PropertyBag
		{
			get
			{
				return base.Worksheet.Columns.PropertyBag;
			}
		}

		internal override WorksheetEntityBase WorksheetEntity
		{
			get
			{
				return base.Worksheet.Columns;
			}
		}

		internal ColumnSelection(Worksheet worksheet, IEnumerable<CellRange> cellRanges)
			: base(worksheet, cellRanges)
		{
		}

		public override void SetBorders(CellBorders value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetColumnBordersCommandContext context = new SetColumnBordersCommandContext(this.Worksheet, cellRange, value);
				this.Worksheet.ExecuteCommand<SetColumnBordersCommandContext>(WorkbookCommands.SetColumnBorders, context);
			});
		}

		protected override void GetFromToIndexFromRange(CellRange cellRange, out int fromIndex, out int toIndex)
		{
			fromIndex = cellRange.FromIndex.ColumnIndex;
			toIndex = cellRange.ToIndex.ColumnIndex;
		}

		internal override void PreserveOldRowColumnPropertyAsLocalIfNeeded<T>(CellRange cellRange, IProperty<T> property, T value)
		{
			if (property.PropertyDefinition != CellPropertyDefinitions.StyleNameProperty)
			{
				IPropertyDefinition<T> propertyDefinition = property.PropertyDefinition as IPropertyDefinition<T>;
				this.PreserveOldRowColumnPropertyAsLocalIfNeededInternal<T>(cellRange, propertyDefinition, value);
				return;
			}
			string styleName = value as string;
			CellStyle byName = base.Worksheet.Workbook.Styles.GetByName(styleName);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.BottomBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.BottomBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<IFill>(cellRange, byName, CellPropertyDefinitions.FillProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<ThemableFontFamily>(cellRange, byName, CellPropertyDefinitions.FontFamilyProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<double>(cellRange, byName, CellPropertyDefinitions.FontSizeProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<ThemableColor>(cellRange, byName, CellPropertyDefinitions.ForeColorProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellValueFormat>(cellRange, byName, CellPropertyDefinitions.FormatProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<RadHorizontalAlignment>(cellRange, byName, CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<int>(cellRange, byName, CellPropertyDefinitions.IndentProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<bool>(cellRange, byName, CellPropertyDefinitions.IsBoldProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<bool>(cellRange, byName, CellPropertyDefinitions.IsItalicProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<bool>(cellRange, byName, CellPropertyDefinitions.IsLockedProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<bool>(cellRange, byName, CellPropertyDefinitions.IsWrappedProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.LeftBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.RightBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<string>(cellRange, byName, CellPropertyDefinitions.StyleNameProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<CellBorder>(cellRange, byName, CellPropertyDefinitions.TopBorderProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<UnderlineType>(cellRange, byName, CellPropertyDefinitions.UnderlineProperty);
			this.PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<RadVerticalAlignment>(cellRange, byName, CellPropertyDefinitions.VerticalAlignmentProperty);
		}

		internal override void PreserveAsLocalPropertyIfStyleIsApplied<T>(CellRange cellRange, IProperty<T> property, T value)
		{
			IPropertyDefinition<T> propertyDefinition = property.PropertyDefinition as IPropertyDefinition<T>;
			IProperty<T> property2;
			if (base.Worksheet.Cells.TryGetProperyFromPropertyDefinition<T>(propertyDefinition, out property2))
			{
				CellsPropertyBag propertyBag = base.Worksheet.Cells.PropertyBag;
				RowsPropertyBag propertyBag2 = base.Worksheet.Rows.PropertyBag;
				ICompressedList<string> propertyValueCollection = propertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
				ICompressedList<string> propertyValueCollection2 = propertyBag2.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
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
				ICompressedList<string> value3 = propertyValueCollection2.GetValue((long)cellRange.FromIndex.RowIndex, (long)cellRange.ToIndex.RowIndex);
				Range<long, string>[] array3 = value3.GetNonDefaultRanges().ToArray<Range<long, string>>();
				foreach (Range<long, string> range2 in array3)
				{
					CellStyle byName2 = base.Worksheet.Workbook.Styles.GetByName(range2.Value);
					if (byName2.GetIsPropertyIncluded(propertyDefinition))
					{
						for (int k = cellRange.FromIndex.ColumnIndex; k <= cellRange.ToIndex.ColumnIndex; k++)
						{
							CellRange cellRange3 = new CellRange((int)range2.Start, k, (int)range2.End, k);
							property2.SetValue(cellRange3, value);
						}
					}
				}
			}
		}

		void PreserveOldRowColumnPropertyAsLocalFromStyleIfNeeded<T>(CellRange cellRange, CellStyle style, IPropertyDefinition<T> propertyDefinition)
		{
			if (style.GetIsPropertyIncluded(propertyDefinition))
			{
				T propertyValue = style.GetPropertyValue<T>(propertyDefinition);
				this.PreserveOldRowColumnPropertyAsLocalIfNeededInternal<T>(cellRange, propertyDefinition, propertyValue);
			}
		}

		void PreserveOldRowColumnPropertyAsLocalIfNeededInternal<T>(CellRange cellRange, IPropertyDefinition<T> propertyDefinition, T value)
		{
			if (!base.Worksheet.Rows.ContainsPropertyDefinition(propertyDefinition))
			{
				return;
			}
			ICompressedList<T> propertyValueRespectingStyle = base.Worksheet.Rows.PropertyBag.GetPropertyValueRespectingStyle<T>(propertyDefinition, base.Worksheet, 0L, (long)SpreadsheetDefaultValues.RowCount - 1L);
			Range<long, T>[] array = propertyValueRespectingStyle.GetNonDefaultRanges().ToArray<Range<long, T>>();
			foreach (Range<long, T> range in array)
			{
				CellRange other = new CellRange((int)range.Start, 0, (int)range.End, SpreadsheetDefaultValues.ColumnCount - 1);
				if (cellRange.IntersectsWith(other))
				{
					CellRange cellRange2 = cellRange.Intersect(other);
					IProperty<T> property;
					if (base.Worksheet.Cells.TryGetProperyFromPropertyDefinition<T>(propertyDefinition, out property))
					{
						base.Worksheet.Cells[cellRange2].SetPropertyValue<T>(property, value);
					}
				}
			}
		}

		public RangePropertyValue<ColumnWidth> GetWidth()
		{
			return base.GetPropertyValue<ColumnWidth>(base.Worksheet.Columns.WidthProperty);
		}

		public void SetWidth(ColumnWidth value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.SetWidth(cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex, value);
			});
		}

		internal void SetWidth(ColumnWidth value, bool considerHidden)
		{
			if (!considerHidden)
			{
				this.SetWidth(value);
			}
			if (value.Value == 0.0)
			{
				this.SetHidden(true);
				return;
			}
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				this.ClearHidden();
				this.SetWidth(value);
			}
		}

		void SetWidth(int fromIndex, int toIndex, ColumnWidth value)
		{
			SetRowColumnPropertyCommandContext<ColumnWidth> context = new SetRowColumnPropertyCommandContext<ColumnWidth>(base.Worksheet, ColumnsPropertyBag.WidthProperty, fromIndex, toIndex, value);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<ColumnWidth>>(WorkbookCommands.SetColumnWidth, context);
		}

		public void ClearWidth()
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.ClearWidth(cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex);
			});
		}

		void ClearWidth(int fromIndex, int toIndex)
		{
			ClearRowColumnPropertyCommandContext<ColumnWidth> context = new ClearRowColumnPropertyCommandContext<ColumnWidth>(base.Worksheet, ColumnsPropertyBag.WidthProperty, fromIndex, toIndex);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<ColumnWidth>>(WorkbookCommands.SetColumnWidth, context);
		}

		public void AutoFitWidth()
		{
			this.AutoFitWidth(false, false);
		}

		public void ExpandToFitNumberValuesWidth()
		{
			if (!base.Worksheet.IsLayoutUpdateSuspended)
			{
				this.AutoFitWidth(true, false);
				return;
			}
			base.Worksheet.CacheExpandToFitColumns(base.CellRanges);
		}

		internal void AutoFitWidth(bool expandOnly, bool considerHidden = false)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				ColumnWidth[] columnWidths = new ColumnWidth[cellRange.ColumnCount];
				List<Action> list = new List<Action>();
				for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
				{
					int currentColumnIndex = i;
					list.Add(delegate
					{
						ColumnWidth columnWidth;
						if (expandOnly)
						{
							ColumnWidth value = this.Worksheet.Columns[currentColumnIndex].GetWidth().Value;
							if (value.IsCustom)
							{
								return;
							}
							columnWidth = LayoutHelper.CalculateAutoColumnWidth(this.Worksheet, currentColumnIndex, cellRange.FromIndex.RowIndex, cellRange.ToIndex.RowIndex, true);
							if (columnWidth == null || value.Value > columnWidth.Value)
							{
								return;
							}
						}
						else
						{
							columnWidth = LayoutHelper.CalculateAutoColumnWidth(this.Worksheet, currentColumnIndex);
						}
						columnWidths[currentColumnIndex - cellRange.FromIndex.ColumnIndex] = columnWidth;
					});
				}
				if (list.Count > 1)
				{
					TasksHelper.DoAsync(list);
				}
				else
				{
					for (int j = 0; j < list.Count; j++)
					{
						list[j]();
					}
				}
				for (int k = cellRange.FromIndex.ColumnIndex; k <= cellRange.ToIndex.ColumnIndex; k++)
				{
					if (columnWidths[k - cellRange.FromIndex.ColumnIndex] != null)
					{
						if (considerHidden)
						{
							this.ClearHidden(k, k);
						}
						this.SetWidth(k, k, columnWidths[k - cellRange.FromIndex.ColumnIndex]);
					}
				}
			});
		}

		internal override void SetOutlineLevelOnRange(CellRange cellRange, int value)
		{
			SetRowColumnPropertyCommandContext<int> context = new SetRowColumnPropertyCommandContext<int>(base.Worksheet, RowColumnPropertyBagBase.OutlineLevelProperty, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex, value);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<int>>(WorkbookCommands.SetColumnOutlineLevel, context);
		}

		public override RangePropertyValue<int> GetOutlineLevel()
		{
			return base.GetPropertyValue<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
		}

		internal override void ClearOutlineLevelOnRange(CellRange cellRange)
		{
			ClearRowColumnPropertyCommandContext<int> context = new ClearRowColumnPropertyCommandContext<int>(base.Worksheet, RowColumnPropertyBagBase.OutlineLevelProperty, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<int>>(WorkbookCommands.SetColumnOutlineLevel, context);
		}

		internal override RowColumnSelectionBase GetRowColumnSelection(int fromIndex, int toIndex)
		{
			return base.Worksheet.Columns[fromIndex, toIndex];
		}

		public void SetHidden(bool value)
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				SetRowColumnPropertyCommandContext<bool> context = new SetRowColumnPropertyCommandContext<bool>(this.Worksheet, RowColumnPropertyBagBase.HiddenProperty, cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex, value);
				this.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<bool>>(WorkbookCommands.SetColumnHidden, context);
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
				this.ClearHidden(cellRange.FromIndex.ColumnIndex, cellRange.ToIndex.ColumnIndex);
			});
		}

		void ClearHidden(int fromIndex, int toIndex)
		{
			ClearRowColumnPropertyCommandContext<bool> context = new ClearRowColumnPropertyCommandContext<bool>(base.Worksheet, RowColumnPropertyBagBase.HiddenProperty, fromIndex, toIndex);
			base.Worksheet.ExecuteCommand<SetRowColumnPropertyCommandContext<bool>>(WorkbookCommands.SetColumnHidden, context);
		}

		public bool Insert()
		{
			IOrderedEnumerable<CellRange> cellRanges = from r in base.CellRanges
				orderby r.FromIndex.ColumnIndex descending
				select r;
			return base.ExecuteForEachRangeInsideBeginEndUpdate(cellRanges, delegate(CellRange cellRange)
			{
				base.Worksheet.Columns.Insert(cellRange.FromIndex.ColumnIndex, cellRange.ColumnCount);
			}, (CellRange cellRange) => base.Worksheet.Columns.CanInsert(cellRange.FromIndex.ColumnIndex, cellRange.ColumnCount));
		}

		public void Remove()
		{
			ICompressedList<bool> compressedList = new CompressedList<bool>(0L, (long)(SpreadsheetDefaultValues.ColumnCount - 1));
			foreach (CellRange cellRange2 in base.CellRanges)
			{
				compressedList.SetValue((long)cellRange2.FromIndex.ColumnIndex, (long)cellRange2.ToIndex.ColumnIndex, true);
			}
			List<CellRange> list = new List<CellRange>();
			foreach (Range<long, bool> range in compressedList)
			{
				if (range.Value)
				{
					list.Add(new CellRange(0, (int)range.Start, 0, (int)range.End));
				}
			}
			list.Reverse();
			base.ExecuteForEachRangeInsideBeginEndUpdate(list, delegate(CellRange cellRange)
			{
				base.Worksheet.Columns.Remove(cellRange.FromIndex.ColumnIndex, cellRange.ColumnCount);
			}, null);
		}
	}
}
