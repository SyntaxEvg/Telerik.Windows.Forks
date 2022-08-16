using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.History;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class RowColumnSelectionBase : SelectionBase
	{
		internal abstract RowColumnPropertyBagBase PropertyBag { get; }

		internal RowColumnSelectionBase(Worksheet worksheet, IEnumerable<CellRange> cellRanges)
			: base(worksheet, cellRanges)
		{
		}

		RangePropertyValue<T> GetRangePropertyValue<T>(IPropertyDefinition<T> property, int fromIndex, int toIndex)
		{
			T value = default(T);
			bool isIndeterminate = false;
			ICompressedList<T> propertyValue = this.PropertyBag.GetPropertyValue<T>(property, fromIndex, toIndex);
			int num = 0;
			foreach (Range<long, T> range in propertyValue)
			{
				if (num >= 1)
				{
					value = this.PropertyBag.GetDefaultPropertyValue<T>(property);
					isIndeterminate = true;
					break;
				}
				value = range.Value;
				num++;
			}
			return new RangePropertyValue<T>(isIndeterminate, value);
		}

		protected RangePropertyValue<T> GetPropertyValue<T>(IPropertyDefinition<T> property)
		{
			return base.GetPropertyValue<T>(delegate(CellRange cellRange)
			{
				int fromIndex;
				int toIndex;
				this.GetFromToIndexFromRange(cellRange, out fromIndex, out toIndex);
				return this.GetRangePropertyValue<T>(property, fromIndex, toIndex);
			}, base.CellRanges, this.PropertyBag.GetDefaultPropertyValue<T>(property));
		}

		protected abstract void GetFromToIndexFromRange(CellRange cellRange, out int fromIndex, out int toIndex);

		internal RowColumnSelectionBase GetRowColumnSelection(int index)
		{
			return this.GetRowColumnSelection(index, index);
		}

		internal abstract RowColumnSelectionBase GetRowColumnSelection(int fromIndex, int toIndex);

		internal override void ClearCellPropertiesIfNeeded<T>(CellRange cellRange, IProperty<T> property)
		{
			IProperty<T> property2;
			if (base.Worksheet.Cells.TryGetProperyFromPropertyDefinition<T>(property.PropertyDefinition, out property2))
			{
				base.Worksheet.Cells[cellRange].ClearPropertyValue(property2, false);
			}
		}

		internal override void PreserveOldStylePropertiesAsLocalIfNeeded<T>(CellRange cellRange, IProperty<T> property, T value)
		{
			if (property.PropertyDefinition == CellPropertyDefinitions.StyleNameProperty)
			{
				Workbook workbook = base.Worksheet.Workbook;
				using (new UpdateScope(new Action(workbook.SuspendLayoutUpdate), new Action(workbook.ResumeLayoutUpdate)))
				{
					CellStyle byName = base.Worksheet.Workbook.Styles.GetByName(value as string);
					RangePropertyValue<string> styleName = base.GetStyleName();
					if (styleName.IsIndeterminate)
					{
						ICompressedList<string> propertyValueCollection = this.PropertyBag.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
						int rowColumnIndex = this.PropertyBag.GetRowColumnIndex(cellRange.FromIndex);
						int rowColumnIndex2 = this.PropertyBag.GetRowColumnIndex(cellRange.ToIndex);
						using (IEnumerator<Range<long, string>> enumerator = propertyValueCollection.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Range<long, string> range = enumerator.Current;
								if ((long)rowColumnIndex >= range.Start && (long)rowColumnIndex2 <= range.End)
								{
									int fromIndex = (int)Math.Max((long)rowColumnIndex, range.Start);
									int toIndex = (int)Math.Max((long)rowColumnIndex2, range.End);
									RowColumnSelectionBase rowColumnSelection = this.GetRowColumnSelection(fromIndex, toIndex);
									base.PreserveAllOldStylePropertiesAsLocalIfNeeded(rowColumnSelection, byName);
								}
							}
							goto IL_128;
						}
					}
					base.PreserveAllOldStylePropertiesAsLocalIfNeeded(this, byName);
					IL_128:;
				}
			}
		}

		internal override void Clear(ClearType type, bool clearIsLocked)
		{
			using (new UpdateScope(new Action(base.BeginUpdate), new Action(base.EndUpdate)))
			{
				if (type == ClearType.All)
				{
					using (IEnumerator<IProperty> enumerator = this.WorksheetEntity.Properties.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IProperty property = enumerator.Current;
							if (property.PropertyDefinition != CellPropertyDefinitions.IsLockedProperty || clearIsLocked)
							{
								base.ClearPropertyValue(property, true);
							}
						}
						goto IL_A6;
					}
				}
				if (type == ClearType.Formats)
				{
					foreach (IProperty property2 in this.WorksheetEntity.Properties)
					{
						base.ClearPropertyValue(property2, true);
					}
				}
				IL_A6:;
			}
		}

		public bool Group()
		{
			return this.ShiftOutlineLevel(delegate(RowColumnSelectionBase rowColumnSelection)
			{
				int value = rowColumnSelection.GetOutlineLevel().Value;
				if (value < RowColumnPropertyBagBase.MaxOutlineLevel)
				{
					rowColumnSelection.SetOutlineLevel(value + 1);
					return true;
				}
				return false;
			});
		}

		public bool Ungroup()
		{
			return this.ShiftOutlineLevel(delegate(RowColumnSelectionBase rowColumnSelection)
			{
				int value = rowColumnSelection.GetOutlineLevel().Value;
				if (value > 0)
				{
					int num = value - 1;
					if (num != 0)
					{
						rowColumnSelection.SetOutlineLevel(value - 1);
					}
					else
					{
						rowColumnSelection.ClearOutlineLevel();
					}
					return true;
				}
				return false;
			});
		}

		bool ShiftOutlineLevel(Func<RowColumnSelectionBase, bool> shiftAction)
		{
			if (base.CellRanges.Count<CellRange>() > 1)
			{
				return false;
			}
			bool result = false;
			CellRange cellRange = base.CellRanges.First<CellRange>();
			WorkbookHistory history = base.Worksheet.Workbook.History;
			using (new UpdateScope(new Action(history.BeginUndoGroup), new Action(history.EndUndoGroup)))
			{
				int rowColumnIndex = this.PropertyBag.GetRowColumnIndex(cellRange.FromIndex);
				int rowColumnIndex2 = this.PropertyBag.GetRowColumnIndex(cellRange.ToIndex);
				for (int i = rowColumnIndex; i <= rowColumnIndex2; i++)
				{
					RowColumnSelectionBase rowColumnSelection = this.GetRowColumnSelection(i);
					result = shiftAction(rowColumnSelection);
				}
			}
			return result;
		}

		public void SetOutlineLevel(int value)
		{
			Guard.ThrowExceptionIfOutOfRange<int>(0, RowColumnPropertyBagBase.MaxOutlineLevel, value, "value");
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.SetOutlineLevelOnRange(cellRange, value);
			});
		}

		public abstract RangePropertyValue<int> GetOutlineLevel();

		public void ClearOutlineLevel()
		{
			base.ExecuteForEachRangeInsideBeginEndUpdate(delegate(CellRange cellRange)
			{
				this.ClearOutlineLevelOnRange(cellRange);
			});
		}

		internal abstract void SetOutlineLevelOnRange(CellRange cellRange, int value);

		internal abstract void ClearOutlineLevelOnRange(CellRange cellRange);
	}
}
