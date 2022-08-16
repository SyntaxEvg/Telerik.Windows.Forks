using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class RowColumnPropertyBagBase : WorksheetPropertyBagBase
	{
		protected abstract WidthHeightBase HiddenSize { get; }

		public RowColumnPropertyBagBase()
		{
			base.RegisterProperty<bool>(RowColumnPropertyBagBase.HiddenProperty);
			base.RegisterProperty<int>(RowColumnPropertyBagBase.OutlineLevelProperty);
		}

		internal bool CanInsert()
		{
			return true;
		}

		public void Insert(int index, int itemsCount)
		{
			base.Translate((long)index, this.ToItemIndex, (long)itemsCount, true);
			this.ClearPropertyValue<bool>(RowColumnPropertyBagBase.HiddenProperty, index, index + itemsCount - 1);
		}

		public void Remove(int index, int itemsCount)
		{
			base.Translate((long)index, this.ToItemIndex, (long)(-(long)itemsCount), false);
		}

		internal abstract int GetRowColumnIndex(int rowIndex, int columnIndex);

		internal int GetRowColumnIndex(CellIndex index)
		{
			return this.GetRowColumnIndex(index.RowIndex, index.ColumnIndex);
		}

		public T GetPropertyValue<T>(IPropertyDefinition<T> property, int index)
		{
			return base.GetPropertyValue<T>(property, (long)index);
		}

		public ICompressedList<T> GetPropertyValue<T>(IPropertyDefinition<T> property, int fromIndex, int toIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(fromIndex, toIndex, "toIndex");
			return base.GetPropertyValue<T>(property, (long)fromIndex, (long)toIndex);
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, int index, T value)
		{
			base.SetPropertyValue<T>(property, (long)index, value);
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, int fromIndex, int toIndex, T value)
		{
			base.SetPropertyValue<T>(property, (long)fromIndex, (long)toIndex, value);
		}

		public void SetPropertyValue<T>(IPropertyDefinition<T> property, ICompressedList<T> values)
		{
			base.SetPropertyValue(property, values);
		}

		public void ClearPropertyValue<T>(IPropertyDefinition<T> property, int index)
		{
			base.ClearPropertyValue<T>(property, (long)index);
		}

		public void ClearPropertyValue<T>(IPropertyDefinition<T> property, int fromIndex, int toIndex)
		{
			Guard.ThrowExceptionIfLessThan<int>(fromIndex, toIndex, "toIndex");
			base.ClearPropertyValue<T>(property, (long)fromIndex, (long)toIndex);
		}

		protected override ICompressedList<T> GetLocalPropertyValueRespectingRowsColumns<T>(IPropertyDefinition<T> property, Worksheet worksheet, long fromIndex, long toIndex)
		{
			return base.GetPropertyValue<T>(property, fromIndex, toIndex);
		}

		public ICompressedList<T> GetSizePropertyValueRespectingHidden<T>(IPropertyDefinition<T> property) where T : WidthHeightBase
		{
			ICompressedList<T> propertyValueCollection = base.GetPropertyValueCollection<T>(property);
			if (property != RowsPropertyBag.HeightProperty && property != ColumnsPropertyBag.WidthProperty)
			{
				return propertyValueCollection;
			}
			ICompressedList<bool> propertyValueCollection2 = base.GetPropertyValueCollection<bool>(RowColumnPropertyBagBase.HiddenProperty);
			ICompressedList<T> compressedList = new CompressedList<T>(propertyValueCollection);
			foreach (Range<long, T> range in propertyValueCollection.GetNonDefaultRanges())
			{
				compressedList.SetValue(range.Start, range.End, range.Value);
			}
			foreach (Range<long, bool> range2 in propertyValueCollection2)
			{
				if (range2.Value)
				{
					T value = this.HiddenSize as T;
					compressedList.SetValue(range2.Start, range2.End, value);
				}
			}
			return compressedList;
		}

		protected override void OnPropertyChanged(IPropertyDefinition property, long fromIndex, long toIndex)
		{
			RowColumnPropertyChangedEventArgs args = new RowColumnPropertyChangedEventArgs(property, (int)fromIndex, (int)toIndex);
			this.OnPropertyChanged(args);
		}

		public event EventHandler<RowColumnPropertyChangedEventArgs> PropertyChanged;

		void OnPropertyChanged(RowColumnPropertyChangedEventArgs args)
		{
			Guard.ThrowExceptionIfNull<RowColumnPropertyChangedEventArgs>(args, "args");
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, args);
			}
		}

		public static readonly IPropertyDefinition<bool> HiddenProperty = new PropertyDefinition<bool>("Hidden", true, StylePropertyGroup.None, false, true);

		public static readonly IPropertyDefinition<int> OutlineLevelProperty = new PropertyDefinition<int>("OutlineLevel", false, StylePropertyGroup.None, 0, true);

		public static readonly int MaxOutlineLevel = 7;
	}
}
