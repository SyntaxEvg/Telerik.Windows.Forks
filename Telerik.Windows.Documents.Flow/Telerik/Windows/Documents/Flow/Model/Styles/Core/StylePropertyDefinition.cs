using System;
using System.ComponentModel;

namespace Telerik.Windows.Documents.Flow.Model.Styles.Core
{
	public class StylePropertyDefinition<TValue> : IStylePropertyDefinition
	{
		internal StylePropertyDefinition(string propertyName, TValue defaultValue, StylePropertyType stylePropertyType)
		{
			this.GlobalPropertyIndex = GlobalPropertyIndexGenerator.GetNext(stylePropertyType);
			this.StylePropertyType = stylePropertyType;
			this.PropertyName = propertyName;
			this.DefaultValue = defaultValue;
			this.Validation = new StylePropertyValidation();
			this.propertyType = typeof(TValue);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public int GlobalPropertyIndex { get; set; }

		[EditorBrowsable(EditorBrowsableState.Never)]
		public Type PropertyType
		{
			get
			{
				return this.propertyType;
			}
		}

		public string PropertyName { get; set; }

		public TValue DefaultValue { get; set; }

		public StylePropertyType StylePropertyType { get; set; }

		public StylePropertyValidation Validation { get; set; }

		public object GetDefaultValueAsObject()
		{
			return this.DefaultValue;
		}

		readonly Type propertyType;
	}
}
