using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class PropertyDefinition<T> : IPropertyDefinition<T>, IPropertyDefinition
	{
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public T DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
		}

		public bool AffectsLayout
		{
			get
			{
				return this.affectsLayout;
			}
		}

		public bool UseSameValueAsPreviousOnInsert
		{
			get
			{
				return this.useSameValueAsPreviousOnInsert;
			}
		}

		public StylePropertyGroup StylePropertyGroup
		{
			get
			{
				return this.stylePropertyGroup;
			}
		}

		public PropertyDefinition(string name, bool affectsLayout, StylePropertyGroup stylePropertyGroup, T defaultValue = default(T), bool useSameValueAsPreviousOnInsert = true)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			this.name = name;
			this.affectsLayout = affectsLayout;
			this.stylePropertyGroup = stylePropertyGroup;
			this.defaultValue = defaultValue;
			this.useSameValueAsPreviousOnInsert = useSameValueAsPreviousOnInsert;
		}

		readonly string name;

		readonly T defaultValue;

		readonly bool affectsLayout;

		readonly bool useSameValueAsPreviousOnInsert;

		readonly StylePropertyGroup stylePropertyGroup;
	}
}
