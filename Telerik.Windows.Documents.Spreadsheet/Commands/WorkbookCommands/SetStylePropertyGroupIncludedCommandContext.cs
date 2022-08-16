using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorkbookCommands
{
	class SetStylePropertyGroupIncludedCommandContext : WorkbookCommandContextBase
	{
		public StylePropertyGroup StylePropertyGroup
		{
			get
			{
				return this.stylePropertyGroup;
			}
		}

		public CellStyle Style
		{
			get
			{
				return this.style;
			}
		}

		public bool OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		public bool NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		public SetStylePropertyGroupIncludedCommandContext(Workbook workbook, CellStyle style, StylePropertyGroup stylePropertyGroup, bool oldValue, bool newValue)
			: base(workbook)
		{
			Guard.ThrowExceptionIfNull<CellStyle>(style, "style");
			Guard.ThrowExceptionIfNull<StylePropertyGroup>(stylePropertyGroup, "stylePropertyGroup");
			this.style = style;
			this.stylePropertyGroup = stylePropertyGroup;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		readonly CellStyle style;

		readonly StylePropertyGroup stylePropertyGroup;

		readonly bool oldValue;

		readonly bool newValue;
	}
}
