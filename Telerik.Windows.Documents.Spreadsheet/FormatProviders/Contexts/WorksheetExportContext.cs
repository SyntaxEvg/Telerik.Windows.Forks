using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class WorksheetExportContext
	{
		public Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		public CellPropertyDataInfo ValuePropertyDataInfo
		{
			get
			{
				return this.valuePropertyDataInfo;
			}
		}

		public WorksheetExportContext(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.valuePropertyDataInfo = new CellPropertyDataInfo();
			this.Initialize();
		}

		void Initialize()
		{
			this.CalculateCellPropertyPropertyUsedValueRanges<ICellValue>(this.valuePropertyDataInfo, CellPropertyDefinitions.ValueProperty, true);
		}

		void CalculateCellPropertyPropertyUsedValueRanges<T>(CellPropertyDataInfo propertyDataInfo, IPropertyDefinition<T> propertyDefinition, bool ignoreDefaultValues)
		{
			ICompressedList<T> propertyValueCollection = this.worksheet.Cells.PropertyBag.GetPropertyValueCollection<T>(propertyDefinition);
			WorksheetExportContext.CalculateCellPropertyPropertyUsedValueRanges<T>(propertyDataInfo, propertyValueCollection, ignoreDefaultValues);
		}

		internal static void CalculateCellPropertyPropertyUsedValueRanges<T>(CellPropertyDataInfo propertyDataInfo, ICompressedList<T> compressedList, bool ignoreDefaultValues)
		{
			T defaultValue = compressedList.GetDefaultValue();
			foreach (Range<long, T> range in compressedList.GetNonDefaultRanges())
			{
				if (!ignoreDefaultValues || !TelerikHelper.EqualsOfT<T>(range.Value, defaultValue))
				{
					for (long num = range.Start; num <= range.End; num += 1L)
					{
						CellIndex cellIndex = WorksheetPropertyBagBase.ConvertLongToCellIndex(num);
						propertyDataInfo.SetRowUsedRangeValue(cellIndex.RowIndex, Range.CreateOrExpand(propertyDataInfo.GetRowUsedRange(cellIndex.RowIndex), cellIndex.ColumnIndex));
						propertyDataInfo.SetColumnUsedRangeValue(cellIndex.ColumnIndex, Range.CreateOrExpand(propertyDataInfo.GetColumnUsedRange(cellIndex.ColumnIndex), cellIndex.RowIndex));
					}
				}
			}
		}

		internal static CellPropertyDataInfo GetValuePropertyDataInfo(Worksheet worksheet)
		{
			WorksheetExportContext worksheetExportContext = new WorksheetExportContext(worksheet);
			return worksheetExportContext.ValuePropertyDataInfo;
		}

		public virtual IEnumerable<CellInfo> GetNonEmptyCellsInRow(int rowIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Range usedRange = this.ValuePropertyDataInfo.GetRowUsedRange(rowIndex);
			if (usedRange != null)
			{
				for (int columnIndex = usedRange.Start; columnIndex <= usedRange.End; columnIndex++)
				{
					yield return new CellInfo(rowIndex, columnIndex, this.worksheet.Cells);
				}
			}
			yield break;
		}

		readonly Worksheet worksheet;

		readonly CellPropertyDataInfo valuePropertyDataInfo;
	}
}
