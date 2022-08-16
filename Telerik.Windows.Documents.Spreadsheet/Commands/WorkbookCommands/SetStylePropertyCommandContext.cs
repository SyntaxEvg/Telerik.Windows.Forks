using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetStylePropertyCommandContext : WorkbookCommandContextBase
	{
		public StylePropertyBase StyleProperty
		{
			get
			{
				return this.styleProperty;
			}
		}

		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public SetStylePropertyCommandContext(Workbook workbook, StylePropertyBase styleProperty, object oldValue, object newValue)
			: base(workbook)
		{
			Guard.ThrowExceptionIfNull<StylePropertyBase>(styleProperty, "property");
			this.styleProperty = styleProperty;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		readonly StylePropertyBase styleProperty;

		readonly object oldValue;

		readonly object newValue;
	}
}
