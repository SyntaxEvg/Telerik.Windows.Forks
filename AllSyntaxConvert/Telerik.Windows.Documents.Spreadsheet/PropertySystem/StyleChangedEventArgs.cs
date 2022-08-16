using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public class StyleChangedEventArgs : EventArgs
	{
		public IEnumerable<IPropertyDefinition> ChangedProperties
		{
			get
			{
				return this.changedProperties;
			}
		}

		public StyleChangedEventArgs(IEnumerable<IPropertyDefinition> changedProperties)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<IPropertyDefinition>>(changedProperties, "changedProperties");
			this.changedProperties = changedProperties;
		}

		readonly IEnumerable<IPropertyDefinition> changedProperties;
	}
}
