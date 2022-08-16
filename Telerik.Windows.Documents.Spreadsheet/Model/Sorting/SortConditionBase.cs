using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.Model.Filtering;
using Telerik.Windows.Documents.Spreadsheet.Model.Sorting.Conditions;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public abstract class SortConditionBase<T> : ISortCondition, ITranslatable, ICopyable<ISortCondition>
	{
		public int RelativeIndex
		{
			get
			{
				return this.relativeIndex;
			}
		}

		protected abstract IPropertyDefinition<T> PropertyDefinition { get; }

		public abstract IComparer<SortValue> Comparer { get; }

		protected SortConditionBase(int relativeIndex)
		{
			this.relativeIndex = relativeIndex;
		}

		public virtual object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			return cells.PropertyBag.GetPropertyValueRespectingStyle<T>(this.PropertyDefinition, cells.Worksheet, index);
		}

		object ITranslatable.Copy(int newRelativeColumnIndex)
		{
			return this.Copy(newRelativeColumnIndex);
		}

		internal abstract ISortCondition Copy(int newRelativeColumnIndex);

		ISortCondition ICopyable<ISortCondition>.Copy(CopyContext context)
		{
			ISortCondition sortCondition = this.Copy(this.RelativeIndex);
			IWorksheetSortCondition worksheetSortCondition = sortCondition as IWorksheetSortCondition;
			if (worksheetSortCondition != null)
			{
				worksheetSortCondition.SetWorksheet(context.TargetWorksheet);
			}
			return sortCondition;
		}

		public override bool Equals(object obj)
		{
			SortConditionBase<T> sortConditionBase = obj as SortConditionBase<T>;
			return sortConditionBase != null && this.RelativeIndex.Equals(sortConditionBase.RelativeIndex);
		}

		public override int GetHashCode()
		{
			return this.RelativeIndex.GetHashCode();
		}

		readonly int relativeIndex;
	}
}
