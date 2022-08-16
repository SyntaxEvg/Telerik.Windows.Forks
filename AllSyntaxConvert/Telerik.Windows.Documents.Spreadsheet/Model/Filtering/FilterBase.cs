using System;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public abstract class FilterBase<T> : IFilter, ITranslatable, ICopyable<IFilter>
	{
		public int RelativeColumnIndex { get; set; }

		protected abstract IPropertyDefinition<T> PropertyDefinition { get; }

		protected FilterBase(int relativeColumnIndex)
		{
			this.RelativeColumnIndex = relativeColumnIndex;
		}

		public virtual object GetValue(Cells cells, int rowIndex, int columnIndex)
		{
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			return cells.PropertyBag.GetPropertyValueRespectingStyle<T>(this.PropertyDefinition, cells.Worksheet, index);
		}

		public abstract bool ShouldShowValue(object value);

		object ITranslatable.Copy(int newRelativeColumnIndex)
		{
			return this.Copy(newRelativeColumnIndex);
		}

		internal abstract IFilter Copy(int newRelativeColumnIndex);

		IFilter ICopyable<IFilter>.Copy(CopyContext context)
		{
			IFilter filter = this.Copy(this.RelativeColumnIndex);
			IWorksheetFilter worksheetFilter = filter as IWorksheetFilter;
			if (worksheetFilter != null)
			{
				worksheetFilter.SetWorksheet(context.TargetWorksheet);
			}
			return filter;
		}
	}
}
