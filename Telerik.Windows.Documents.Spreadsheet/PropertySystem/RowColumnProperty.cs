using System;
using Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class RowColumnProperty<TValue, TContext> : PropertyBase<TValue, TContext> where TContext : SetRowColumnPropertyCommandContext<TValue>
	{
		protected abstract RowColumnPropertyBagBase PropertyBag { get; }

		public RowColumnProperty(Worksheet worksheet, IPropertyDefinition<TValue> propertyDefinition)
			: base(worksheet, propertyDefinition)
		{
		}

		RangePropertyValue<TValue> GetRangePropertyValue<T>(IPropertyDefinition<TValue> property, int fromIndex, int toIndex)
		{
			TValue tvalue = default(TValue);
			bool isIndeterminate = false;
			ICompressedList<TValue> propertyValueRespectingStyle = this.PropertyBag.GetPropertyValueRespectingStyle<TValue>(property, base.Worksheet, (long)fromIndex, (long)toIndex);
			bool flag = true;
			foreach (Range<long, TValue> range in propertyValueRespectingStyle)
			{
				if (flag)
				{
					tvalue = range.Value;
					flag = false;
				}
				else if (!TelerikHelper.EqualsOfT<TValue>(tvalue, range.Value))
				{
					tvalue = base.GetDefaultValue();
					isIndeterminate = true;
				}
			}
			return new RangePropertyValue<TValue>(isIndeterminate, tvalue);
		}

		internal abstract int GetRowColumnIndex(CellIndex index);

		public override RangePropertyValue<TValue> GetValue(CellRange cellRange)
		{
			int rowColumnIndex = this.GetRowColumnIndex(cellRange.FromIndex);
			int rowColumnIndex2 = this.GetRowColumnIndex(cellRange.ToIndex);
			return this.GetRangePropertyValue<TValue>(base.PropertyDefinition, rowColumnIndex, rowColumnIndex2);
		}

		protected override TContext CreateSetPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<TValue> property, CellRange cellRange, TValue value)
		{
			int rowColumnIndex = this.GetRowColumnIndex(cellRange.FromIndex);
			int rowColumnIndex2 = this.GetRowColumnIndex(cellRange.ToIndex);
			return (TContext)((object)Activator.CreateInstance(typeof(TContext), new object[] { worksheet, property, rowColumnIndex, rowColumnIndex2, value }));
		}

		protected override TContext CreateClearPropertyCommandContext(Worksheet worksheet, IPropertyDefinition<TValue> property, CellRange cellRange)
		{
			int rowColumnIndex = this.GetRowColumnIndex(cellRange.FromIndex);
			int rowColumnIndex2 = this.GetRowColumnIndex(cellRange.ToIndex);
			return new ClearRowColumnPropertyCommandContext<TValue>(worksheet, property, rowColumnIndex, rowColumnIndex2) as TContext;
		}
	}
}
