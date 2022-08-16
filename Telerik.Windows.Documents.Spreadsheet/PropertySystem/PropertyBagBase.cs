using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	abstract class PropertyBagBase
	{
		protected virtual long FromItemIndex
		{
			get
			{
				return 0L;
			}
		}

		protected abstract long ToItemIndex { get; }

		public PropertyBagBase()
		{
			this.propertyToValueCollection = new Dictionary<IPropertyDefinition, ICompressedList>();
		}

		protected IEnumerable<IPropertyDefinition> GetRegisteredProperties()
		{
			return this.propertyToValueCollection.Keys;
		}

		protected void Copy(long fromIndex, long toIndex, bool respectUseSameValueAsPrevious)
		{
			foreach (KeyValuePair<IPropertyDefinition, ICompressedList> keyValuePair in this.propertyToValueCollection)
			{
				bool useSameValueAsPreviousOnInsert = keyValuePair.Key.UseSameValueAsPreviousOnInsert;
				ITranslateableCollection value = keyValuePair.Value;
				if (!respectUseSameValueAsPrevious || useSameValueAsPreviousOnInsert)
				{
					value.Copy(fromIndex, toIndex);
				}
			}
		}

		protected void Copy(long fromIndex, long itemsCount, long toIndex, bool respectUseSameValueAsPrevious)
		{
			foreach (KeyValuePair<IPropertyDefinition, ICompressedList> keyValuePair in this.propertyToValueCollection)
			{
				bool useSameValueAsPreviousOnInsert = keyValuePair.Key.UseSameValueAsPreviousOnInsert;
				ITranslateableCollection value = keyValuePair.Value;
				if (!respectUseSameValueAsPrevious || useSameValueAsPreviousOnInsert)
				{
					value.Copy(fromIndex, itemsCount, toIndex);
				}
			}
		}

		protected void Copy(IPropertyDefinition propertyDefinition, long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(propertyDefinition, "propertyDefinition");
			ITranslateableCollection translateableCollection = this.propertyToValueCollection[propertyDefinition];
			translateableCollection.Copy(fromIndex, toIndex);
		}

		protected void Clear(long index)
		{
			foreach (ITranslateableCollection translateableCollection in this.propertyToValueCollection.Values)
			{
				translateableCollection.Clear(index);
			}
		}

		protected void Clear(long fromIndex, long toIndex)
		{
			foreach (ITranslateableCollection translateableCollection in this.propertyToValueCollection.Values)
			{
				translateableCollection.Clear(fromIndex, toIndex);
			}
		}

		protected void Translate(long fromIndex, long toIndex, long offset, bool respectUseSameValueAsPrevious)
		{
			foreach (KeyValuePair<IPropertyDefinition, ICompressedList> keyValuePair in this.propertyToValueCollection)
			{
				bool useSameValueAsPreviousOnInsert = keyValuePair.Key.UseSameValueAsPreviousOnInsert;
				ITranslateableCollection value = keyValuePair.Value;
				value.Translate(fromIndex, toIndex, offset, !respectUseSameValueAsPrevious || useSameValueAsPreviousOnInsert);
			}
		}

		protected virtual void RegisterProperty<T>(IPropertyDefinition<T> property)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			this.propertyToValueCollection.Add(property, new CompressedList<T>(this.FromItemIndex, this.ToItemIndex, property.DefaultValue));
		}

		protected internal ICompressedList GetPropertyValueCollection(IPropertyDefinition property)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			return this.propertyToValueCollection[property];
		}

		protected internal ICompressedList<T> GetPropertyValueCollection<T>(IPropertyDefinition<T> property)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			return (ICompressedList<T>)this.propertyToValueCollection[property];
		}

		public T GetDefaultPropertyValue<T>(IPropertyDefinition<T> property)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			return propertyValueCollection.GetDefaultValue();
		}

		public void SetDefaultPropertyValue<T>(IPropertyDefinition<T> property, T defaultValue)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			propertyValueCollection.SetDefaultValue(defaultValue);
		}

		protected internal T GetPropertyValue<T>(IPropertyDefinition<T> property, long index)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			return propertyValueCollection.GetValue(index);
		}

		protected internal ICompressedList<T> GetPropertyValue<T>(IPropertyDefinition<T> property, long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			return propertyValueCollection.GetValue(fromIndex, toIndex);
		}

		protected internal ICompressedList GetNonTypedPropertyValue(IPropertyDefinition property, long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			ICompressedList propertyValueCollection = this.GetPropertyValueCollection(property);
			return propertyValueCollection.GetValue(fromIndex, toIndex);
		}

		protected void SetPropertyValue<T>(IPropertyDefinition<T> property, long index, T value)
		{
			this.SetPropertyValue<T>(property, index, index, value);
		}

		protected void SetPropertyValue<T>(IPropertyDefinition<T> property, long fromIndex, long toIndex, T value)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			propertyValueCollection.SetValue(fromIndex, toIndex, value);
			this.OnPropertyChanged(property, fromIndex, toIndex);
		}

		internal void SetNonTypedPropertyValue(IPropertyDefinition property, ICompressedList values)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			Guard.ThrowExceptionIfNull<ICompressedList>(values, "values");
			ICompressedList propertyValueCollection = this.GetPropertyValueCollection(property);
			propertyValueCollection.SetValue(values);
			this.OnPropertyChanged(property, values.FromIndex, values.ToIndex);
		}

		protected void UpdatePropertyValue<T>(IPropertyDefinition<T> property, long fromIndex, long toIndex, Func<T, T> newValueTransform)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			Guard.ThrowExceptionIfNull<Func<T, T>>(newValueTransform, "newValueTransform");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			ICompressedList<T> value = propertyValueCollection.GetValue(fromIndex, toIndex);
			ICompressedList<T> compressedList = new CompressedList<T>(value);
			foreach (Range<long, T> range in value)
			{
				T t = newValueTransform(range.Value);
				if (!TelerikHelper.EqualsOfT<T>(t, compressedList.GetDefaultValue()))
				{
					compressedList.SetValue(range.Start, range.End, t);
				}
			}
			propertyValueCollection.SetValue(compressedList);
			this.OnPropertyChanged(property, fromIndex, toIndex);
		}

		protected void SetPropertyValue(IPropertyDefinition property, ICompressedList values)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			Guard.ThrowExceptionIfNull<ICompressedList>(values, "values");
			ICompressedList propertyValueCollection = this.GetPropertyValueCollection(property);
			propertyValueCollection.SetValue(values);
			this.OnPropertyChanged(property, values.FromIndex, values.ToIndex);
		}

		protected void ClearPropertyValue<T>(IPropertyDefinition<T> property, long index)
		{
			this.ClearPropertyValue<T>(property, index, index);
		}

		protected void ClearPropertyValue<T>(IPropertyDefinition<T> property, long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			ICompressedList<T> propertyValueCollection = this.GetPropertyValueCollection<T>(property);
			propertyValueCollection.ClearValue(fromIndex, toIndex);
			this.OnPropertyChanged(property, fromIndex, toIndex);
		}

		internal void ClearPropertyValue(IPropertyDefinition property)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition>(property, "property");
			ICompressedList propertyValueCollection = this.GetPropertyValueCollection(property);
			long fromIndex = propertyValueCollection.FromIndex;
			long toIndex = propertyValueCollection.ToIndex;
			propertyValueCollection.ClearValue(fromIndex, toIndex);
			this.OnPropertyChanged(property, fromIndex, toIndex);
		}

		protected void CopyPropertiesFrom(PropertyBagBase fromProperties, long fromIndex, long toIndex)
		{
			foreach (IPropertyDefinition property in this.GetRegisteredProperties())
			{
				ICompressedList propertyValueCollection = fromProperties.GetPropertyValueCollection(property);
				this.SetPropertyValue(property, propertyValueCollection.GetValue(fromIndex, toIndex));
			}
		}

		protected virtual void OnPropertyChanged(IPropertyDefinition property, long fromIndex, long toIndex)
		{
		}

		readonly Dictionary<IPropertyDefinition, ICompressedList> propertyToValueCollection;
	}
}
