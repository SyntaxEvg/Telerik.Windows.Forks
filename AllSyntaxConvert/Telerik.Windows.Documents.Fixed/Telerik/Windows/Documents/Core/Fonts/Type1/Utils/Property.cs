using System;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters;
using Telerik.Windows.Documents.Core.PostScript.Data;

namespace Telerik.Windows.Documents.Core.Fonts.Type1.Utils
{
	class Property<T> : IProperty
	{
		public Property(PropertyDescriptor descriptor)
		{
			this.Descriptor = descriptor;
		}

		public Property(PropertyDescriptor descriptor, IConverter converter)
			: this(descriptor)
		{
			this.converter = converter;
		}

		public Property(PropertyDescriptor descriptor, T defaultValue)
			: this(descriptor)
		{
			this.value = defaultValue;
		}

		public Property(PropertyDescriptor descriptor, IConverter converter, T defaultValue)
			: this(descriptor)
		{
			this.value = defaultValue;
			this.converter = converter;
		}

		public PropertyDescriptor Descriptor { get; set; }

		public T GetValue()
		{
			return this.value;
		}

		public bool SetValue(object value)
		{
			if (value is T)
			{
				this.value = (T)((object)value);
				return true;
			}
			if (this.converter != null)
			{
				this.value = (T)((object)this.converter.Convert(typeof(T), value));
				return true;
			}
			return false;
		}

		readonly IConverter converter;

		T value;
	}
}
