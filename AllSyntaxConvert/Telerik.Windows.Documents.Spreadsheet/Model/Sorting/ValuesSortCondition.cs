using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public sealed class ValuesSortCondition : OrderedSortConditionBase<ICellValue>
	{
		protected override IPropertyDefinition<ICellValue> PropertyDefinition
		{
			get
			{
				return CellPropertyDefinitions.ValueProperty;
			}
		}

		public override IComparer<SortValue> Comparer
		{
			get
			{
				switch (base.SortOrder)
				{
				case SortOrder.Ascending:
					return ValuesSortCondition.DefaultValuesSortComparer;
				case SortOrder.Descending:
					return ValuesSortCondition.InvertedValuesSortComparer;
				default:
					return null;
				}
			}
		}

		public ValuesSortCondition(int relativeIndex, SortOrder sortOrder)
			: base(relativeIndex, sortOrder)
		{
		}

		internal override ISortCondition Copy(int newRelativeColumnIndex)
		{
			return new ValuesSortCondition(newRelativeColumnIndex, base.SortOrder);
		}

		static readonly ValuesSortConditionComparer DefaultValuesSortComparer = new ValuesSortConditionComparer(false);

		static readonly ValuesSortConditionComparer InvertedValuesSortComparer = new ValuesSortConditionComparer(true);
	}
}
