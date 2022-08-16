using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetDefaultPropertyValueCommandContext<T> : WorksheetCommandContextBase
	{
		public IPropertyDefinition<T> Property
		{
			get
			{
				return this.property;
			}
		}

		public PropertyBagBase PropertyBag
		{
			get
			{
				return this.propertyBag;
			}
		}

		public T OldValue
		{
			get
			{
				return this.oldValue;
			}
			internal set
			{
				this.oldValue = value;
			}
		}

		public T NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public SetDefaultPropertyValueCommandContext(Worksheet worksheet, IPropertyDefinition<T> property, PropertyBagBase propertyBag, T newValue)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<IPropertyDefinition<T>>(property, "property");
			Guard.ThrowExceptionIfNull<PropertyBagBase>(propertyBag, "propertyBag");
			this.property = property;
			this.propertyBag = propertyBag;
			this.newValue = newValue;
		}

		readonly IPropertyDefinition<T> property;

		readonly PropertyBagBase propertyBag;

		T oldValue;

		readonly T newValue;
	}
}
