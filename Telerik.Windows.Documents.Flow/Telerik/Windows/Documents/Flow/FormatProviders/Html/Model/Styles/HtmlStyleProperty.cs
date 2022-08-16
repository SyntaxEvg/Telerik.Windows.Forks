using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	class HtmlStyleProperty
	{
		public HtmlStyleProperty(string name, string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			this.name = name;
			this.values = new HtmlStylePropertyValues(value);
		}

		public HtmlStyleProperty(HtmlStyleProperty property, HtmlStyleProperty baseProperty)
		{
			Guard.ThrowExceptionIfNull<HtmlStyleProperty>(property, "property");
			this.name = property.Name;
			this.values = property.values;
			this.baseProperty = baseProperty;
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string UnparsedValue
		{
			get
			{
				return this.values.UnparsedValue;
			}
		}

		public HtmlStylePropertyValues Values
		{
			get
			{
				return this.values;
			}
		}

		public HtmlStyleProperty BaseProperty
		{
			get
			{
				return this.baseProperty;
			}
		}

		readonly string name;

		readonly HtmlStyleProperty baseProperty;

		readonly HtmlStylePropertyValues values;
	}
}
