using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.Shapes;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands.WorksheetCommands
{
	class SetImagePreferRelativeToOriginalResizeCommandContext : WorksheetCommandContextBase
	{
		public bool PreferRelativeToOriginalResize
		{
			get
			{
				return this.preferRelativeToOriginalResize;
			}
		}

		public bool OldPreferRelativeToOriginalResize
		{
			get
			{
				return this.oldPreferRelativeToOriginalResize;
			}
			set
			{
				this.oldPreferRelativeToOriginalResize = value;
			}
		}

		public FloatingImage Image
		{
			get
			{
				return this.image;
			}
		}

		public SetImagePreferRelativeToOriginalResizeCommandContext(Worksheet worksheet, FloatingImage shape, bool preferRelativeToOriginalResize)
			: base(worksheet)
		{
			Guard.ThrowExceptionIfNull<FloatingImage>(shape, "shape");
			this.preferRelativeToOriginalResize = preferRelativeToOriginalResize;
			this.image = shape;
		}

		readonly bool preferRelativeToOriginalResize;

		bool oldPreferRelativeToOriginalResize;

		readonly FloatingImage image;
	}
}
