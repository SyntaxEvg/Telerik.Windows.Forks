using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Core.Fonts.Type1.Type1Format.Converters;
using Telerik.Windows.Documents.Core.Fonts.Type1.Utils;

namespace Telerik.Windows.Documents.Core.PostScript.Data
{
	abstract class PostScriptObject
	{
		public PostScriptObject()
		{
			this.properties = new Dictionary<string, IProperty>();
		}

		public void Load(PostScriptDictionary fromDict)
		{
			foreach (KeyValuePair<string, object> keyValuePair in fromDict)
			{
				IProperty property;
				if (this.properties.TryGetValue(keyValuePair.Key, out property))
				{
					property.SetValue(keyValuePair.Value);
				}
			}
		}

		protected Property<T> CreateProperty<T>(PropertyDescriptor descriptor)
		{
			Property<T> property = new Property<T>(descriptor);
			this.RegisterProperty(property);
			return property;
		}

		protected Property<T> CreateProperty<T>(PropertyDescriptor descriptor, IConverter converter)
		{
			Property<T> property = new Property<T>(descriptor, converter);
			this.RegisterProperty(property);
			return property;
		}

		protected Property<T> CreateProperty<T>(PropertyDescriptor descriptor, T defaultValue)
		{
			Property<T> property = new Property<T>(descriptor, defaultValue);
			this.RegisterProperty(property);
			return property;
		}

		protected Property<T> CreateProperty<T>(PropertyDescriptor descriptor, IConverter converter, T defaultValue)
		{
			Property<T> property = new Property<T>(descriptor, converter, defaultValue);
			this.RegisterProperty(property);
			return property;
		}

		void RegisterProperty(PropertyDescriptor descriptor, IProperty property)
		{
			if (descriptor.Name != null)
			{
				this.properties[descriptor.Name] = property;
			}
		}

		void RegisterProperty(IProperty property)
		{
			this.RegisterProperty(property.Descriptor, property);
		}

		readonly Dictionary<string, IProperty> properties;
	}
}
