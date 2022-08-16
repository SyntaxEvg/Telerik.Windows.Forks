using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public sealed class CustomValuesSortCondition : OrderedSortConditionBase<ICellValue>
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
				return this.comparer;
			}
		}

		public string[] CustomList
		{
			get
			{
				return this.customList;
			}
		}

		public CustomValuesSortCondition(int relativeIndex, string[] customList, SortOrder sortOrder)
			: base(relativeIndex, sortOrder)
		{
			this.customList = customList;
			this.comparer = new CustomValuesSortConditionComparer(this.CustomList, sortOrder);
		}

		internal override ISortCondition Copy(int newRelativeColumnIndex)
		{
			return new CustomValuesSortCondition(newRelativeColumnIndex, this.CustomList, base.SortOrder);
		}

		public override bool Equals(object obj)
		{
			CustomValuesSortCondition customValuesSortCondition = obj as CustomValuesSortCondition;
			if (customValuesSortCondition == null)
			{
				return false;
			}
			if (this.customList.Length != customValuesSortCondition.customList.Length)
			{
				return false;
			}
			for (int i = 0; i < this.customList.Length; i++)
			{
				if (!TelerikHelper.EqualsOfT<string>(this.customList[i], customValuesSortCondition.customList[i]))
				{
					return false;
				}
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.customList.GetHashCodeOrZero());
		}

		readonly string[] customList;

		readonly CustomValuesSortConditionComparer comparer;
	}
}
