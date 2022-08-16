using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class WorksheetPropertyBagBase : PropertyBagBase
	{
		public WorksheetPropertyBagBase()
		{
			this.propertyToUsedCellRanges = new Dictionary<IPropertyDefinition, CellRange>();
			this.RegisterProperty<CellValueFormat>(CellPropertyDefinitions.FormatProperty);
			this.RegisterProperty<string>(CellPropertyDefinitions.StyleNameProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.LeftBorderProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.TopBorderProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.RightBorderProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.BottomBorderProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.DiagonalUpBorderProperty);
			this.RegisterProperty<CellBorder>(CellPropertyDefinitions.DiagonalDownBorderProperty);
			this.RegisterProperty<IFill>(CellPropertyDefinitions.FillProperty);
			this.RegisterProperty<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty);
			this.RegisterProperty<double>(CellPropertyDefinitions.FontSizeProperty);
			this.RegisterProperty<bool>(CellPropertyDefinitions.IsBoldProperty);
			this.RegisterProperty<bool>(CellPropertyDefinitions.IsItalicProperty);
			this.RegisterProperty<UnderlineType>(CellPropertyDefinitions.UnderlineProperty);
			this.RegisterProperty<ThemableColor>(CellPropertyDefinitions.ForeColorProperty);
			this.RegisterProperty<RadHorizontalAlignment>(CellPropertyDefinitions.HorizontalAlignmentProperty);
			this.RegisterProperty<RadVerticalAlignment>(CellPropertyDefinitions.VerticalAlignmentProperty);
			this.RegisterProperty<int>(CellPropertyDefinitions.IndentProperty);
			this.RegisterProperty<bool>(CellPropertyDefinitions.IsWrappedProperty);
			this.RegisterProperty<bool>(CellPropertyDefinitions.IsLockedProperty);
		}

		internal static long ConvertCellIndexToLong(CellIndex index)
		{
			return WorksheetPropertyBagBase.ConvertCellIndexToLong(index.RowIndex, index.ColumnIndex);
		}

		internal static long ConvertCellIndexToLong(int rowIndex, int columnIndex)
		{
			return (long)columnIndex * (long)SpreadsheetDefaultValues.RowCount + (long)rowIndex;
		}

		internal static CellIndex ConvertLongToCellIndex(long index)
		{
			int rowIndex;
			int columnIndex;
			WorksheetPropertyBagBase.ConvertLongToRowAndColumnIndexes(index, out rowIndex, out columnIndex);
			return new CellIndex(rowIndex, columnIndex);
		}

		internal static void ConvertLongToRowAndColumnIndexes(long index, out int rowIndex, out int columnIndex)
		{
			rowIndex = (int)(index % (long)SpreadsheetDefaultValues.RowCount);
			columnIndex = (int)(index / (long)SpreadsheetDefaultValues.RowCount);
		}

		internal static IEnumerable<LongRange> ConvertCellRangeToLongRanges(CellRange cellRange)
		{
			int fromRowIndex = cellRange.FromIndex.RowIndex;
			int toRowIndex = cellRange.ToIndex.RowIndex;
			for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
			{
				long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(fromRowIndex, i);
				long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(toRowIndex, i);
				yield return new LongRange(fromIndex, toIndex);
			}
			yield break;
		}

		internal static IEnumerable<CellRange> ConvertCellRangeToLongCellRanges(CellRange cellRange)
		{
			int fromRowIndex = cellRange.FromIndex.RowIndex;
			int toRowIndex = cellRange.ToIndex.RowIndex;
			for (int i = cellRange.FromIndex.ColumnIndex; i <= cellRange.ToIndex.ColumnIndex; i++)
			{
				yield return new CellRange(fromRowIndex, i, toRowIndex, i);
			}
			yield break;
		}

		internal static CellRange ConvertLongCellRangeToCellRange(long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			return new CellRange(WorksheetPropertyBagBase.ConvertLongToCellIndex(fromIndex), WorksheetPropertyBagBase.ConvertLongToCellIndex(toIndex));
		}

		public ICompressedList<T> GetPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			long fromIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.FromIndex);
			long toIndex = WorksheetPropertyBagBase.ConvertCellIndexToLong(cellRange.ToIndex);
			ICompressedList<T> compressedList = new CompressedList<T>(fromIndex, toIndex, property.DefaultValue);
			foreach (LongRange longRange in WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange))
			{
				ICompressedList<T> propertyValueRespectingStyle = this.GetPropertyValueRespectingStyle<T>(property, worksheet, longRange.Start, longRange.End);
				compressedList.SetValue(propertyValueRespectingStyle);
			}
			return compressedList;
		}

		public T GetPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long index)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			ICompressedList<T> propertyValueCollection = base.GetPropertyValueCollection<T>(property);
			T value = propertyValueCollection.GetValue(index);
			bool flag = propertyValueCollection.ContainsNonDefaultValues(index, index);
			if (!CellStyle.IsSupportedProperty(property) || flag)
			{
				return value;
			}
			ICompressedList<string> propertyValueCollection2 = base.GetPropertyValueCollection<string>(CellPropertyDefinitions.StyleNameProperty);
			string value2 = propertyValueCollection2.GetValue(index);
			CellStyle byName = worksheet.Workbook.Styles.GetByName(value2);
			if (byName != null)
			{
				if (byName.GetIsPropertyIncluded(property) && !TelerikHelper.EqualsOfT<T>(byName.GetPropertyValue<T>(property), base.GetDefaultPropertyValue<T>(property)))
				{
					return byName.GetPropertyValue<T>(property);
				}
				T result;
				if (this.TryGetRowOrColumnPropertyValueRespectingStyle<T>(property, worksheet, index, out result))
				{
					return result;
				}
			}
			return value;
		}

		public ICompressedList<T> GetPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			if (!CellStyle.IsSupportedProperty(property))
			{
				return this.GetLocalPropertyValueRespectingRowsColumns<T>(property, worksheet, fromIndex, toIndex);
			}
			ICompressedList<string> propertyValue = base.GetPropertyValue<string>(CellPropertyDefinitions.StyleNameProperty, fromIndex, toIndex);
			ICompressedList<T> propertyValue2 = base.GetPropertyValue<T>(property, fromIndex, toIndex);
			ICompressedList<T> compressedList = new CompressedList<T>(propertyValue2);
			foreach (Range<long, string> range in propertyValue)
			{
				CellStyle byName = worksheet.Workbook.Styles.GetByName(range.Value);
				if (byName != null)
				{
					bool isDefault = range.IsDefault;
					ICompressedList<T> compressedList2;
					if (byName.GetIsPropertyIncluded(property) && !isDefault)
					{
						compressedList.SetValue(range.Start, range.End, byName.GetPropertyValue<T>(property));
					}
					else if (this.TryGetRowOrColumnPropertyValueRespectingStyle<T>(property, worksheet, range.Start, range.End, out compressedList2))
					{
						foreach (Range<long, T> range2 in compressedList2.GetNonDefaultRanges())
						{
							compressedList.SetValue(range2.Start, range2.End, range2.Value);
						}
					}
				}
			}
			foreach (Range<long, T> range3 in propertyValue2.GetNonDefaultRanges())
			{
				compressedList.SetValue(range3.Start, range3.End, range3.Value);
			}
			return compressedList;
		}

		protected abstract ICompressedList<T> GetLocalPropertyValueRespectingRowsColumns<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex);

		protected virtual bool TryGetRowOrColumnPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long index, out T result)
		{
			result = default(T);
			return false;
		}

		protected virtual bool TryGetRowOrColumnPropertyValueRespectingStyle<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex, out ICompressedList<T> rowOrColumnPropertyValuesRespectingStyle)
		{
			rowOrColumnPropertyValuesRespectingStyle = null;
			return false;
		}

		internal void InvalidateAllPropertiesUsedCellRange()
		{
			foreach (IPropertyDefinition property in this.propertyToUsedCellRanges.Keys.ToList<IPropertyDefinition>())
			{
				this.InvalidatePropertyUsedCellRange(property);
			}
		}

		protected void InvalidatePropertyUsedCellRange(IPropertyDefinition property)
		{
			this.propertyToUsedCellRanges.Remove(property);
		}

		protected CellRange GetUsedCellRange(IPropertyDefinition property)
		{
			int num = SpreadsheetDefaultValues.ColumnCount - 1;
			CellRange cellRange = new CellRange(0, 0, SpreadsheetDefaultValues.RowCount - 1, SpreadsheetDefaultValues.ColumnCount - 1);
			List<LongRange> list = new List<LongRange>(WorksheetPropertyBagBase.ConvertCellRangeToLongRanges(cellRange));
			list.Reverse();
			int num2 = 0;
			int num3 = 0;
			foreach (LongRange longRange in list)
			{
				ICompressedList propertyValueCollection = base.GetPropertyValueCollection(property);
				long? lastNonEmptyRangeEnd = propertyValueCollection.GetLastNonEmptyRangeEnd(longRange.Start, longRange.End);
				if (lastNonEmptyRangeEnd != null)
				{
					num2 = (int)Math.Max((long)num2, lastNonEmptyRangeEnd.Value - longRange.Start);
					num3 = Math.Max(num3, num);
				}
				num--;
			}
			return new CellRange(0, 0, num2, num3);
		}

		public CellRange GetUsedCellRange(IEnumerable<IPropertyDefinition> propertyDefinitions)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<IPropertyDefinition>>(propertyDefinitions, "propertyDefinitions");
			List<Action> list = new List<Action>();
			using (IEnumerator<IPropertyDefinition> enumerator = propertyDefinitions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IPropertyDefinition property = enumerator.Current;
					if (!this.propertyToUsedCellRanges.ContainsKey(property))
					{
						Action item = delegate()
						{
							CellRange usedCellRange = this.GetUsedCellRange(property);
							lock (this.propertyToUsedCellRanges)
							{
								this.propertyToUsedCellRanges[property] = usedCellRange;
							}
						};
						list.Add(item);
					}
				}
			}
			if (list.Count > 0)
			{
				TasksHelper.DoAsync(list);
			}
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<IPropertyDefinition, CellRange> keyValuePair in this.propertyToUsedCellRanges)
			{
				if (propertyDefinitions.Contains(keyValuePair.Key) && keyValuePair.Value != null)
				{
					num = Math.Max(num, keyValuePair.Value.ToIndex.RowIndex);
					num2 = Math.Max(num2, keyValuePair.Value.ToIndex.ColumnIndex);
				}
			}
			return new CellRange(0, 0, num, num2);
		}

		public CellRange GetUsedCellRange()
		{
			IEnumerable<IPropertyDefinition> registeredProperties = base.GetRegisteredProperties();
			return this.GetUsedCellRange(registeredProperties);
		}

		readonly Dictionary<IPropertyDefinition, CellRange> propertyToUsedCellRanges;
	}
}
